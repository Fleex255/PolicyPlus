using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace PolicyPlus.csharp.Models
{
    // This class implements just enough of IPolicySource to work with PolFile.ApplyDifference It is
    // not a valid policy source for any policy loader
    public class RegFile : IPolicySource
    {
        private const string RegSignature = "Windows Registry Editor Version 5.00";
        private string _prefix; // REG files require fully-rooted key paths, while other policy sources disallow them
        private string _sourceSubtree; // Accept only writes under this policy path (not needed if only going to use Apply)
        private readonly List<RegFileKey> _keys = new();

        private static string EscapeValue(string text)
        {
            // Escape quotes and slashes for a value name or string data
            var sb = new System.Text.StringBuilder();
            for (int n = 0, loopTo = text.Length - 1; n <= loopTo; n++)
            {
                var character = text[n];
                if (character is '"' or '\\')
                {
                    _ = sb.Append('\\');
                }

                _ = sb.Append(character);
            }
            return sb.ToString();
        }

        private static string UnescapeValue(string text)
        {
            // The reverse of EscapeValue
            var sb = new System.Text.StringBuilder();
            var escaping = false;
            for (int n = 0, loopTo = text.Length - 1; n <= loopTo; n++)
            {
                if (escaping)
                {
                    _ = sb.Append(text[n]);
                    escaping = false;
                }
                else if (text[n] == '\\')
                {
                    escaping = true;
                }
                else
                {
                    _ = sb.Append(text[n]);
                }
            }
            return sb.ToString();
        }

        private static string? ReadNonCommentingLine(StreamReader reader, char? stopAt = default)
        {
            while (true)
            {
                if (reader.EndOfStream)
                {
                    return null;
                }

                if (stopAt.HasValue && reader.Peek() == (int)char.GetNumericValue(stopAt.Value))
                {
                    return null;
                }

                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith(";"))
                {
                    continue;
                }

                return line;
            }
        }

        public static RegFile Load(StreamReader reader, string prefix)
        {
            if ((reader.ReadLine() ?? "") != RegSignature)
            {
                throw new InvalidDataException("Incorrect REG signature");
            }

            var reg = new RegFile();
            reg.SetPrefix(prefix);
            while (true) // Read all the keys
            {
                var keyHeader = ReadNonCommentingLine(reader);
                if (keyHeader is null)
                {
                    break;
                }

                var keyName = keyHeader.Substring(1, keyHeader.Length - 2); // Remove the brackets
                if (keyName.StartsWith("-"))
                {
                    // It's a deleter
                    var deleterKey = new RegFileKey { Name = keyName.Substring(1), IsDeleter = true };
                    reg._keys.Add(deleterKey);
                }
                else
                {
                    var key = new RegFileKey { Name = keyName };
                    while (true) // Read all the values
                    {
                        var valueLine = ReadNonCommentingLine(reader, '[');
                        if (valueLine is null)
                        {
                            break;
                        }

                        var valueName = "";
                        string data;
                        if (valueLine.StartsWith("@"))
                        {
                            data = valueLine.Substring(2);
                        }
                        else
                        {
                            var parts = valueLine.Split("\"=", 2);
                            valueName = UnescapeValue(parts[0].Substring(1));
                            data = parts[1];
                        }
                        var value = new RegFileValue { Name = valueName };
                        if (data == "-")
                        {
                            value.IsDeleter = true;
                        }
                        else if (data.StartsWith("\""))
                        {
                            value.Kind = RegistryValueKind.String;
                            value.Data = UnescapeValue(data.Substring(1, data.Length - 2));
                        }
                        else if (data.StartsWith("dword:"))
                        {
                            value.Kind = RegistryValueKind.DWord;
                            value.Data = uint.Parse(data.Substring(6), System.Globalization.NumberStyles.HexNumber);
                        }
                        else if (data.StartsWith("hex"))
                        {
                            var indexOfClosingParen = data.IndexOf(')');
                            string curHexLine;
                            if (indexOfClosingParen != -1)
                            {
                                value.Kind = (RegistryValueKind)int.Parse(data.Substring(4, indexOfClosingParen - 4), System.Globalization.NumberStyles.HexNumber);
                                curHexLine = data.Substring(indexOfClosingParen + 2);
                            }
                            else
                            {
                                value.Kind = RegistryValueKind.Binary;
                                curHexLine = data.Substring(4);
                            }
                            var allDehexedBytes = new List<byte>();
                            while (true) // Read all the hex lines
                            {
                                foreach (var b in curHexLine!.Trim().TrimEnd('\\', ',').Split(',').Where(s => !string.IsNullOrEmpty(s)))
                                {
                                    allDehexedBytes.Add(byte.Parse(b, System.Globalization.NumberStyles.HexNumber));
                                }

                                if (curHexLine.EndsWith(@"\"))
                                {
                                    curHexLine = reader.ReadLine();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            value.Data = PolFile.BytesToObject(allDehexedBytes.ToArray(), value.Kind);
                        }
                        key.Values.Add(value);
                    }
                    reg._keys.Add(key);
                }
            }
            return reg;
        }

        public static RegFile Load(string file, string prefix)
        {
            using var reader = new StreamReader(file);
            return Load(reader, prefix);
        }

        public void Save(StreamWriter writer)
        {
            writer.WriteLine(RegSignature);
            writer.WriteLine();
            foreach (var key in _keys)
            {
                if (key.IsDeleter)
                {
                    writer.WriteLine("[-" + key.Name + "]");
                }
                else
                {
                    writer.WriteLine("[" + key.Name + "]");
                    foreach (var value in key.Values)
                    {
                        if (value is null)
                        {
                            continue;
                        }

                        int posInRow;
                        if (string.IsNullOrEmpty(value.Name))
                        {
                            writer.Write("@");
                            posInRow = 1;
                        }
                        else
                        {
                            var quotedName = "\"" + EscapeValue(value.Name) + "\"";
                            writer.Write(quotedName);
                            posInRow = quotedName.Length;
                        }
                        writer.Write("=");
                        posInRow++;
                        if (value.IsDeleter)
                        {
                            writer.WriteLine("-");
                        }
                        else
                        {
                            switch (value.Kind)
                            {
                                case RegistryValueKind.String:
                                    {
                                        writer.Write("\"");
                                        writer.Write(EscapeValue(value.Data.ToString()));
                                        writer.WriteLine("\"");
                                        break;
                                    }
                                case RegistryValueKind.DWord:
                                    {
                                        writer.Write("dword:");
                                        writer.WriteLine(Convert.ToString(Convert.ToUInt32(value.Data), 16).PadLeft(8, '0'));
                                        break;
                                    }

                                default:
                                    {
                                        writer.Write("hex");
                                        posInRow += 3;
                                        if (value.Kind != RegistryValueKind.Binary)
                                        {
                                            writer.Write("(");
                                            writer.Write(Convert.ToString((int)value.Kind, 16));
                                            writer.Write(")");
                                            posInRow += 3;
                                        }
                                        writer.Write(":");
                                        posInRow++;
                                        var bytes = PolFile.ObjectToBytes(value.Data, value.Kind);
                                        for (int n = 0, loopTo = bytes.Length - 2; n <= loopTo; n++)
                                        {
                                            writer.Write(Convert.ToString(bytes[n], 16).PadLeft(2, '0'));
                                            writer.Write(",");
                                            posInRow += 3;
                                            if (posInRow >= 78)
                                            {
                                                writer.WriteLine(@"\");
                                                writer.Write("  ");
                                                posInRow = 2;
                                            }
                                        }
                                        if (bytes.Length > 0)
                                        {
                                            writer.WriteLine(Convert.ToString(bytes[bytes.Length - 1], 16).PadLeft(2, '0'));
                                        }
                                        else
                                        {
                                            writer.WriteLine();
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }
                writer.WriteLine();
            }
        }

        public void Save(string file)
        {
            using var writer = new StreamWriter(file, false);
            Save(writer);
        }

        private string UnprefixKeyName(string name)
        {
            if (name.StartsWith(_prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                return name.Substring(_prefix.Length);
            }

            return name;
        }

        private string PrefixKeyName(string name) => _prefix + name;

        private RegFileKey? GetKey(string name) => _keys.Find(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        private RegFileKey? GetKeyByUnprefixedName(string name) => GetKey(PrefixKeyName(name));

        private RegFileKey GetOrCreateKey(string name)
        {
            var key = GetKey(name);
            if (key is not null)
            {
                return key;
            }

            key = new RegFileKey { Name = name };
            _keys.Add(key);
            return key;
        }

        private RegFileKey? GetNonDeleterKey(string name) => _keys.Find(k => !k.IsDeleter && k.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        private bool IsSourceKeyAcceptable(string key) => string.IsNullOrEmpty(_sourceSubtree) || key.Equals(_sourceSubtree, StringComparison.InvariantCultureIgnoreCase) || key.StartsWith(_sourceSubtree + @"\", StringComparison.InvariantCultureIgnoreCase);

        public bool ContainsValue(string key, string value) => throw new NotImplementedException();

        public object GetValue(string key, string value) => throw new NotImplementedException();

        public bool WillDeleteValue(string key, string value) => throw new NotImplementedException();

        public List<string> GetValueNames(string key) => throw new NotImplementedException();

        public void SetValue(string key, string value, object data, RegistryValueKind dataType)
        {
            if (!IsSourceKeyAcceptable(key))
            {
                return;
            }

            var fullKeyName = PrefixKeyName(key);
            var keyRecord = GetNonDeleterKey(fullKeyName);
            if (keyRecord is null)
            {
                keyRecord = new RegFileKey { Name = fullKeyName };
                _keys.Add(keyRecord);
            }
            _ = keyRecord.Values.Remove(keyRecord.GetValue(value));
            keyRecord.Values.Add(new RegFileValue { Name = value, Kind = dataType, Data = data });
        }

        public void ForgetValue(string key, string value) => throw new NotImplementedException();

        public void DeleteValue(string key, string value)
        {
            if (!IsSourceKeyAcceptable(key))
            {
                return;
            }

            var fullKeyName = PrefixKeyName(key);
            var keyRecord = GetOrCreateKey(fullKeyName);
            if (keyRecord.IsDeleter)
            {
                return;
            }

            _ = keyRecord.Values.Remove(keyRecord.GetValue(value));
            keyRecord.Values.Add(new RegFileValue { Name = value, IsDeleter = true });
        }

        public void ClearKey(string key)
        {
            if (!IsSourceKeyAcceptable(key))
            {
                return;
            }

            var fullName = PrefixKeyName(key);
            _ = _keys.Remove(GetKey(fullName));
            _keys.Add(new RegFileKey { Name = fullName, IsDeleter = true });
        }

        public void ForgetKeyClearance(string key)
        {
            if (!IsSourceKeyAcceptable(key))
            {
                return;
            }

            var keyRecord = GetKeyByUnprefixedName(key);
            if (keyRecord is null)
            {
                return;
            }

            if (keyRecord.IsDeleter)
            {
                _ = _keys.Remove(keyRecord);
            }
        }

        public void Apply(IPolicySource target)
        {
            foreach (var key in _keys)
            {
                var unprefixedKeyName = UnprefixKeyName(key.Name);
                if (key.IsDeleter)
                {
                    target.ClearKey(unprefixedKeyName);
                }
                else
                {
                    foreach (var value in key.Values)
                    {
                        if (value.IsDeleter)
                        {
                            target.DeleteValue(unprefixedKeyName, value.Name);
                        }
                        else
                        {
                            target.SetValue(unprefixedKeyName, value.Name, value.Data, value.Kind);
                        }
                    }
                }
            }
        }

        public void SetPrefix(string prefix)
        {
            if (!prefix.EndsWith(@"\"))
            {
                prefix += @"\";
            }

            _prefix = prefix;
        }

        public void SetSourceBranch(string branch)
        {
            if (branch.EndsWith(@"\"))
            {
                branch = branch.TrimEnd('\\');
            }

            _sourceSubtree = branch;
        }

        public string GuessPrefix()
        {
            // Try to determine a reasonable prefix from the data present
            if (_keys.Count == 0)
            {
                return @"HKEY_LOCAL_MACHINE\"; // Can't do much without any data
            }

            var firstKeyName = _keys[0].Name;
            if (firstKeyName.StartsWith(@"HKEY_USERS\"))
            {
                // The user SID should be part of the prefix
                var secondSlashPos = firstKeyName.IndexOf(@"\", 11);
                return firstKeyName.Substring(0, secondSlashPos + 1);
            }

            // Other hives should be just fine
            var firstSlashPos = firstKeyName.IndexOf(@"\");
            return firstKeyName.Substring(0, firstSlashPos + 1);
        }

        public bool HasDefaultValues() => _keys.Any(k => k.Values.Any(v => string.IsNullOrEmpty(v.Name)));

        private class RegFileKey
        {
            public string Name;
            public bool IsDeleter;
            public readonly List<RegFileValue> Values = new();

            public RegFileValue? GetValue(string value) => Values.Find(v => v.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private class RegFileValue
        {
            public string Name;
            public object Data;
            public RegistryValueKind Kind;
            public bool IsDeleter;
        }
    }
}