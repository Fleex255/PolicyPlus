using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace PolicyPlus
{
    // This class implements just enough of IPolicySource to work with PolFile.ApplyDifference
    // It is not a valid policy source for any policy loader
    public class RegFile : IPolicySource
    {
        private const string RegSignature = "Windows Registry Editor Version 5.00";
        private string Prefix; // REG files require fully-rooted key paths, while other policy sources disallow them
        private string SourceSubtree; // Accept only writes under this policy path (not needed if only going to use Apply)
        private List<RegFileKey> Keys = new List<RegFileKey>();
        private static string EscapeValue(string Text)
        {
            // Escape quotes and slashes for a value name or string data
            var sb = new System.Text.StringBuilder();
            for (int n = 0, loopTo = Text.Length - 1; n <= loopTo; n++)
            {
                char character = Text[n];
                if (character == '"' | character == '\\')
                    sb.Append('\\');
                sb.Append(character);
            }
            return sb.ToString();
        }
        private static string UnescapeValue(string Text)
        {
            // The reverse of EscapeValue
            var sb = new System.Text.StringBuilder();
            bool escaping = false;
            for (int n = 0, loopTo = Text.Length - 1; n <= loopTo; n++)
            {
                if (escaping)
                {
                    sb.Append(Text[n]);
                    escaping = false;
                }
                else if (Text[n] == '\\')
                {
                    escaping = true;
                }
                else
                {
                    sb.Append(Text[n]);
                }
            }
            return sb.ToString();
        }
        private static string ReadNonCommentingLine(StreamReader Reader, char? StopAt = default)
        {
            do
            {
                if (Reader.EndOfStream)
                    return null;
                if (StopAt.HasValue && Reader.Peek() == Strings.Asc(StopAt.Value))
                    return null;
                string line = Reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || Strings.Trim(line).StartsWith(";"))
                    continue;
                return line;
            }
            while (true);
        }
        public static RegFile Load(StreamReader Reader, string Prefix)
        {
            if ((Reader.ReadLine() ?? "") != RegSignature)
                throw new InvalidDataException("Incorrect REG signature");
            var reg = new RegFile();
            reg.SetPrefix(Prefix);
            do // Read all the keys
            {
                string keyHeader = ReadNonCommentingLine(Reader);
                if (keyHeader is null)
                    break;
                string keyName = keyHeader.Substring(1, keyHeader.Length - 2); // Remove the brackets
                if (keyName.StartsWith("-"))
                {
                    // It's a deleter
                    var deleterKey = new RegFileKey() { Name = keyName.Substring(1), IsDeleter = true };
                    reg.Keys.Add(deleterKey);
                }
                else
                {
                    var key = new RegFileKey() { Name = keyName };
                    do // Read all the values
                    {
                        string valueLine = ReadNonCommentingLine(Reader, '[');
                        if (valueLine is null)
                            break;
                        string valueName = "";
                        string data;
                        if (valueLine.StartsWith("@"))
                        {
                            data = valueLine.Substring(2);
                        }
                        else
                        {
                            string[] parts = Strings.Split(valueLine, "\"=", 2);
                            valueName = UnescapeValue(parts[0].Substring(1));
                            data = parts[1];
                        }
                        var value = new RegFileValue() { Name = valueName };
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
                            int indexOfClosingParen = data.IndexOf(')');
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
                            do // Read all the hex lines
                            {
                                var hexBytes = Strings.Split(Strings.Trim(curHexLine).TrimEnd('\\', ','), ",").Where(s => !string.IsNullOrEmpty(s));
                                foreach (var b in hexBytes)
                                    allDehexedBytes.Add(byte.Parse(b, System.Globalization.NumberStyles.HexNumber));
                                if (curHexLine.EndsWith(@"\"))
                                {
                                    curHexLine = Reader.ReadLine();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            while (true);
                            value.Data = PolFile.BytesToObject(allDehexedBytes.ToArray(), value.Kind);
                        }
                        key.Values.Add(value);
                    }
                    while (true);
                    reg.Keys.Add(key);
                }
            }
            while (true);
            return reg;
        }
        public static RegFile Load(string File, string Prefix)
        {
            using (var reader = new StreamReader(File))
            {
                return Load(reader, Prefix);
            }
        }
        public void Save(StreamWriter Writer)
        {
            Writer.WriteLine(RegSignature);
            Writer.WriteLine();
            foreach (var key in Keys)
            {
                if (key.IsDeleter)
                {
                    Writer.WriteLine("[-" + key.Name + "]");
                }
                else
                {
                    Writer.WriteLine("[" + key.Name + "]");
                    foreach (var value in key.Values)
                    {
                        int posInRow = 0; // To split hex across lines
                        if (string.IsNullOrEmpty(value.Name))
                        {
                            Writer.Write("@");
                            posInRow = 1;
                        }
                        else
                        {
                            string quotedName = "\"" + EscapeValue(value.Name) + "\"";
                            Writer.Write(quotedName);
                            posInRow = quotedName.Length;
                        }
                        Writer.Write("=");
                        posInRow += 1;
                        if (value.IsDeleter)
                        {
                            Writer.WriteLine("-");
                        }
                        else
                        {
                            switch (value.Kind)
                            {
                                case RegistryValueKind.String:
                                    {
                                        Writer.Write("\"");
                                        Writer.Write(EscapeValue(Conversions.ToString(value.Data)));
                                        Writer.WriteLine("\"");
                                        break;
                                    }
                                case RegistryValueKind.DWord:
                                    {
                                        Writer.Write("dword:");
                                        Writer.WriteLine(Convert.ToString(Conversions.ToUInteger(value.Data), 16).PadLeft(8, '0'));
                                        break;
                                    }

                                default:
                                    {
                                        Writer.Write("hex");
                                        posInRow += 3;
                                        if (value.Kind != RegistryValueKind.Binary)
                                        {
                                            Writer.Write("(");
                                            Writer.Write(Convert.ToString((int)value.Kind, 16));
                                            Writer.Write(")");
                                            posInRow += 3;
                                        }
                                        Writer.Write(":");
                                        posInRow += 1;
                                        byte[] bytes = PolFile.ObjectToBytes(value.Data, value.Kind);
                                        for (int n = 0, loopTo = bytes.Length - 2; n <= loopTo; n++)
                                        {
                                            Writer.Write(Convert.ToString(bytes[n], 16).PadLeft(2, '0'));
                                            Writer.Write(",");
                                            posInRow += 3;
                                            if (posInRow >= 78)
                                            {
                                                Writer.WriteLine(@"\");
                                                Writer.Write("  ");
                                                posInRow = 2;
                                            }
                                        }
                                        if (bytes.Length > 0)
                                        {
                                            Writer.WriteLine(Convert.ToString(bytes[bytes.Length - 1], 16).PadLeft(2, '0'));
                                        }
                                        else
                                        {
                                            Writer.WriteLine();
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }
                Writer.WriteLine();
            }
        }
        public void Save(string File)
        {
            using (var writer = new StreamWriter(File, false))
            {
                Save(writer);
            }
        }
        private string UnprefixKeyName(string Name)
        {
            if (Name.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
                return Name.Substring(Prefix.Length);
            else
                return Name;
        }
        private string PrefixKeyName(string Name)
        {
            return Prefix + Name;
        }
        private RegFileKey GetKey(string Name)
        {
            return Keys.FirstOrDefault(k => k.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
        }
        private RegFileKey GetKeyByUnprefixedName(string Name)
        {
            return GetKey(PrefixKeyName(Name));
        }
        private RegFileKey GetOrCreateKey(string Name)
        {
            var key = GetKey(Name);
            if (key is null)
            {
                key = new RegFileKey() { Name = Name };
                Keys.Add(key);
            }
            return key;
        }
        private RegFileKey GetNonDeleterKey(string Name)
        {
            return Keys.FirstOrDefault(k => !k.IsDeleter && k.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase));
        }
        private bool IsSourceKeyAcceptable(string Key)
        {
            return string.IsNullOrEmpty(SourceSubtree) || Key.Equals(SourceSubtree, StringComparison.InvariantCultureIgnoreCase) || Key.StartsWith(SourceSubtree + @"\", StringComparison.InvariantCultureIgnoreCase);
        }
        public bool ContainsValue(string Key, string Value)
        {
            throw new NotImplementedException();
        }
        public object GetValue(string Key, string Value)
        {
            throw new NotImplementedException();
        }
        public bool WillDeleteValue(string Key, string Value)
        {
            throw new NotImplementedException();
        }
        public List<string> GetValueNames(string Key)
        {
            throw new NotImplementedException();
        }
        public void SetValue(string Key, string Value, object Data, RegistryValueKind DataType)
        {
            if (!IsSourceKeyAcceptable(Key))
                return;
            string fullKeyName = PrefixKeyName(Key);
            var keyRecord = GetNonDeleterKey(fullKeyName);
            if (keyRecord is null)
            {
                keyRecord = new RegFileKey() { Name = fullKeyName };
                Keys.Add(keyRecord);
            }
            keyRecord.Values.Remove(keyRecord.GetValue(Value));
            keyRecord.Values.Add(new RegFileValue() { Name = Value, Kind = DataType, Data = Data });
        }
        public void ForgetValue(string Key, string Value)
        {
            throw new NotImplementedException();
        }
        public void DeleteValue(string Key, string Value)
        {
            if (!IsSourceKeyAcceptable(Key))
                return;
            string fullKeyName = PrefixKeyName(Key);
            var keyRecord = GetOrCreateKey(fullKeyName);
            if (keyRecord.IsDeleter)
                return;
            keyRecord.Values.Remove(keyRecord.GetValue(Value));
            keyRecord.Values.Add(new RegFileValue() { Name = Value, IsDeleter = true });
        }
        public void ClearKey(string Key)
        {
            if (!IsSourceKeyAcceptable(Key))
                return;
            string fullName = PrefixKeyName(Key);
            Keys.Remove(GetKey(fullName));
            Keys.Add(new RegFileKey() { Name = fullName, IsDeleter = true });
        }
        public void ForgetKeyClearance(string Key)
        {
            if (!IsSourceKeyAcceptable(Key))
                return;
            var keyRecord = GetKeyByUnprefixedName(Key);
            if (keyRecord is null)
                return;
            if (keyRecord.IsDeleter)
                Keys.Remove(keyRecord);
        }
        public void Apply(IPolicySource Target)
        {
            foreach (var key in Keys)
            {
                string unprefixedKeyName = UnprefixKeyName(key.Name);
                if (key.IsDeleter)
                {
                    Target.ClearKey(unprefixedKeyName);
                }
                else
                {
                    foreach (var value in key.Values)
                    {
                        if (value.IsDeleter)
                        {
                            Target.DeleteValue(unprefixedKeyName, value.Name);
                        }
                        else
                        {
                            Target.SetValue(unprefixedKeyName, value.Name, value.Data, value.Kind);
                        }
                    }
                }
            }
        }
        public void SetPrefix(string Prefix)
        {
            if (!Prefix.EndsWith(@"\"))
                Prefix += @"\";
            this.Prefix = Prefix;
        }
        public void SetSourceBranch(string Branch)
        {
            if (Branch.EndsWith(@"\"))
                Branch = Branch.TrimEnd('\\');
            SourceSubtree = Branch;
        }
        public string GuessPrefix()
        {
            // Try to determine a reasonable prefix from the data present
            if (Keys.Count == 0)
                return @"HKEY_LOCAL_MACHINE\"; // Can't do much without any data
            string firstKeyName = Keys[0].Name;
            if (firstKeyName.StartsWith(@"HKEY_USERS\"))
            {
                // The user SID should be part of the prefix
                int secondSlashPos = firstKeyName.IndexOf(@"\", 11);
                return firstKeyName.Substring(0, secondSlashPos + 1);
            }
            else
            {
                // Other hives should be just fine
                int firstSlashPos = firstKeyName.IndexOf(@"\");
                return firstKeyName.Substring(0, firstSlashPos + 1);
            }
        }
        public bool HasDefaultValues()
        {
            return Keys.Any(k => k.Values.Any(v => string.IsNullOrEmpty(v.Name)));
        }
        private class RegFileKey
        {
            public string Name;
            public bool IsDeleter;
            public List<RegFileValue> Values = new List<RegFileValue>();
            public RegFileValue GetValue(string Value)
            {
                return Values.FirstOrDefault(v => v.Name.Equals(Value, StringComparison.InvariantCultureIgnoreCase));
            }
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