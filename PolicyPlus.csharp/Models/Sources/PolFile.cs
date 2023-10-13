using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PolicyPlus.csharp.Models.Sources;

namespace PolicyPlus.csharp.Models
{
    public partial class PolFile : IPolicySource
    {
        // The sortedness is important because key clearances need to be processed before the
        // addition of their values Fortunately, clearing entries start with **, which sorts before
        // normal values
        private readonly SortedDictionary<string, PolEntryData> _entries = new(); // Keys are lowercase Registry keys and values, separated by "\\"

        private readonly Dictionary<string, string> _casePreservation = new(); // Keep track of the original cases

        private const int semicolonAscii = 59;

        private string GetDictKey(string key, string value)
        {
            var origCase = key + @"\\" + value;
            var lowerCase = origCase.ToLowerInvariant();
            if (!_casePreservation.ContainsKey(lowerCase))
            {
                _casePreservation.Add(lowerCase, origCase);
            }

            return lowerCase;
        }

        public static PolFile Load(string file)
        {
            using var fPol = new FileStream(file, FileMode.Open, FileAccess.Read);
            using var binary = new BinaryReader(fPol);
            return Load(binary);
        }

        public static PolFile Load(BinaryReader stream)
        {
            ValidatePolSignature(stream);
            ValidatePolVersion(stream);
            return ParsePol(stream);
        }

        private static PolFile ParsePol(BinaryReader stream)
        {
            var pol = new PolFile();
            while (stream.BaseStream.Position != stream.BaseStream.Length)
            {
                var ped = new PolEntryData();
                stream.BaseStream.Position += 2L; // Skip the "[" character
                var key = ReadSz(stream);
                stream.BaseStream.Position += 2L; // Skip ";"
                var value = ReadSz(stream);

                if (stream.ReadUInt16() != semicolonAscii)
                {
                    stream.BaseStream.Position += 2L; // MS documentation indicates there might be an extra null before the ";" after the value name
                }

                ped.Kind = (RegistryValueKind)stream.ReadInt32();
                stream.BaseStream.Position += 2L; // ";"
                var length = stream.ReadUInt32();
                stream.BaseStream.Position += 2L; // ";"
                var data = new byte[(int)(length - 1L + 1)];
                _ = stream.Read(data, 0, (int)length);
                ped.Data = data;
                stream.BaseStream.Position += 2L; // "]"

                pol._entries.Add(pol.GetDictKey(key, value), ped);
            }
            return pol;
        }

        private static string ReadSz(BinaryReader stream)
        {
            var sb = new System.Text.StringBuilder();
            while (true)
            {
                int charCode = stream.ReadUInt16();
                if (charCode == 0)
                {
                    break;
                }

                _ = sb.Append((char)charCode);
            }
            return sb.ToString();
        }

        private static void ValidatePolVersion(BinaryReader stream)
        {
            if (stream.ReadUInt32() != 1L)
            {
                throw new InvalidDataException("Unknown (newer) version of POL format");
            }
        }

        private static void ValidatePolSignature(BinaryReader stream)
        {
            if (stream.ReadUInt32() != 0x67655250L)
            {
                throw new InvalidDataException("Missing PReg signature");
            }
        }

        public void Save(string file)
        {
            using var fPol = new FileStream(file, FileMode.Create);
            using var binary = new BinaryWriter(fPol, System.Text.Encoding.Unicode);
            Save(binary);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(0x67655250U);
            writer.Write(1);
            foreach (var kv in _entries)
            {
                writer.Write('[');
                var pathparts = _casePreservation[kv.Key].Split(@"\\", 2);
                WriteSz(pathparts[0]); // Key name
                writer.Write(';');
                WriteSz(pathparts[1]); // Value name
                writer.Write(';');
                writer.Write((int)kv.Value.Kind);
                writer.Write(';');
                writer.Write(kv.Value.Data.Length);
                writer.Write(';');
                writer.Write(kv.Value.Data);
                writer.Write(']');
            }

            void WriteSz(string text)
            {
                foreach (var c in text)
                {
                    writer.Write(c);
                }

                writer.Write((short)0);
            }
        }

        public void DeleteValue(string key, string value)
        {
            ForgetValue(key, value);
            if (WillDeleteValue(key, value))
            {
                return;
            }

            var ped = PolEntryData.FromDword(32U); // It's what Microsoft does
            _entries.Add(GetDictKey(key, "**del." + value), ped);
        }

        public void ForgetValue(string key, string value)
        {
            var dictKey = GetDictKey(key, value);
            if (_entries.ContainsKey(dictKey))
            {
                _ = _entries.Remove(dictKey);
            }

            var deleterKey = GetDictKey(key, "**del." + value);
            if (_entries.ContainsKey(deleterKey))
            {
                _ = _entries.Remove(deleterKey);
            }
        }

        public void SetValue(string key, string value, object data, RegistryValueKind dataType)
        {
            var dictKey = GetDictKey(key, value);
            if (_entries.ContainsKey(dictKey))
            {
                _ = _entries.Remove(dictKey);
            }

            _entries.Add(dictKey, PolEntryData.FromArbitrary(data, dataType));
        }

        public bool ContainsValue(string key, string value) => !WillDeleteValue(key, value) && _entries.ContainsKey(GetDictKey(key, value));

        public object? GetValue(string key, string value) => ContainsValue(key, value) ? _entries[GetDictKey(key, value)].AsArbitrary() : null;

        public bool WillDeleteValue(string key, string value)
        {
            var willDelete = false;
            var keyRoot = GetDictKey(key, string.Empty);
            foreach (var kv in _entries.Where(e => e.Key.StartsWith(keyRoot)))
            {
                if ((kv.Key ?? string.Empty) == (GetDictKey(key, "**del." + value) ?? string.Empty))
                {
                    willDelete = true;
                }
                else if (kv.Key.StartsWith(GetDictKey(key, "**delvals"))) // MS POL files also use "**delvals."
                {
                    willDelete = true;
                }
                else if ((kv.Key ?? string.Empty) == (GetDictKey(key, "**deletevalues") ?? string.Empty))
                {
                    var lowerVal = value.ToLowerInvariant();
                    var deletedValues = kv.Value.AsString().Split(";");
                    if (deletedValues.Any(s => (s.ToLowerInvariant() ?? string.Empty) == (lowerVal ?? string.Empty)))
                    {
                        willDelete = true;
                    }
                }
                else if ((kv.Key ?? string.Empty) == (GetDictKey(key, value) ?? string.Empty))
                {
                    willDelete = false; // In case the key is cleared out before setting the values
                }
            }
            return willDelete;
        }

        public List<string> GetValueNames(string key) => GetValueNames(key, true);

        public List<string> GetValueNames(string key, bool onlyValues)
        {
            var prefix = GetDictKey(key, string.Empty);
            return (from k in _entries.Keys where k.StartsWith(prefix) select _casePreservation[k].Split(@"\\", 2)[1] into valName where !(onlyValues && valName.StartsWith("**")) select valName).ToList();
        }

        public void ApplyDifference(PolFile? oldVersion, IPolicySource target)
        {
            // Figure out which values have changed and commit only the changes
            oldVersion ??= new PolFile();

            var oldEntries = oldVersion._entries.Keys.Where(k => !k.Contains(@"\\**")).ToList();
            foreach (var kv in _entries)
            {
                var parts = kv.Key.Split(@"\\", 2); // Key, value
                var casedParts = _casePreservation[kv.Key].Split(@"\\", 2);
                if (parts[1].StartsWith("**del."))
                {
                    target.DeleteValue(parts[0], parts[1].Split(".", 2)[1]);
                }
                else if (parts[1].StartsWith("**delvals"))
                {
                    target.ClearKey(parts[0]);
                }
                else if (parts[1] == "**deletevalues")
                {
                    foreach (var entry in kv.Value.AsString().Split(";"))
                    {
                        target.DeleteValue(parts[0], entry);
                    }
                }
                else if (parts[1].StartsWith("**deletekeys"))
                {
                    foreach (var entry in kv.Value.AsString().Split(";"))
                    {
                        target.ClearKey(parts[0] + @"\" + entry);
                    }
                }
                else if (!string.IsNullOrEmpty(parts[1]) && !parts[1].StartsWith("**"))
                {
                    target.SetValue(casedParts[0], casedParts[1], kv.Value.AsArbitrary(), kv.Value.Kind);
                    if (oldEntries.Contains(kv.Key))
                    {
                        _ = oldEntries.Remove(kv.Key); // It's not forgotten
                    }
                }
            }
            foreach (var e in oldEntries.Where(RegistryPolicyProxy.IsPolicyKey)) // Remove the forgotten entries from the Registry
            {
                var parts = e.Split(@"\\", 2);
                target.ForgetValue(parts[0], parts[1]);
            }
        }

        public void Apply(IPolicySource target) =>
            // Apply all the values to the policy source
            ApplyDifference(null, target);

        public void ClearKey(string key)
        {
            foreach (var value in GetValueNames(key, false))
            {
                ForgetValue(key, value);
            }

            var ped = PolEntryData.FromString(" ");
            _entries.Add(GetDictKey(key, "**delvals."), ped);
        }

        public void ForgetKeyClearance(string key)
        {
            var keyDeleter = GetDictKey(key, "**delvals");
            foreach (var kv in _entries.Where(e => e.Key.StartsWith(keyDeleter)).ToList()) // "**delvals" and "**delvals." are both valid
            {
                _ = _entries.Remove(kv.Key);
            }
        }

        public List<string> GetKeyNames(string key)
        {
            var subkeyNames = new List<string>();
            var prefix = string.IsNullOrEmpty(key) ? string.Empty : key + @"\"; // Let an empty key name mean the root
            foreach (var entry in _entries.Keys.Where(e => e.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (entry.StartsWith(prefix + @"\", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue; // Values are delimited by an extra slash
                }

                var properCased = _casePreservation[entry].Split(@"\\", 2)[0];
                if (prefix.Length >= properCased.Length)
                {
                    continue; // Do not return the requested key itself
                }

                var localKeyName = properCased.Substring(prefix.Length).Split(@"\", 2)[0];
                if (!subkeyNames.Contains(localKeyName, StringComparer.InvariantCultureIgnoreCase))
                {
                    subkeyNames.Add(localKeyName);
                }
            }
            return subkeyNames;
        }

        public RegistryValueKind GetValueKind(string key, string value) => _entries[GetDictKey(key, value)].Kind;

        //public PolFile Duplicate()
        //{
        //    using var ms = new MemoryStream();
        //    using (var writer = new BinaryWriter(ms, System.Text.Encoding.Unicode, true))
        //    {
        //        Save(writer);
        //    }
        //    ms.Position = 0L;
        //    using var reader = new BinaryReader(ms, System.Text.Encoding.Unicode);
        //    return Load(reader);
        //}

        public static byte[] ObjectToBytes(object data, RegistryValueKind kind) => PolEntryData.FromArbitrary(data, kind).Data;

        public static object BytesToObject(byte[] data, RegistryValueKind kind) => new PolEntryData { Data = data, Kind = kind }.AsArbitrary();
    }
}