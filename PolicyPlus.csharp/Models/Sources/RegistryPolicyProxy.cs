using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace PolicyPlus.csharp.Models.Sources
{
    public class RegistryPolicyProxy : IPolicySource // Pass operations through to the real Registry
    {
        public static RegistryPolicyProxy EncapsulateKey(RegistryKey key) => new() { EncapsulatedRegistry = key };

        public static RegistryPolicyProxy EncapsulateKey(RegistryHive key) => EncapsulateKey(RegistryKey.OpenBaseKey(key, RegistryView.Default));

        public void DeleteValue(string key, string value)
        {
            using var regKey = EncapsulatedRegistry.OpenSubKey(key, true);

            regKey?.DeleteValue(value, false);
        }

        public void ForgetValue(string key, string value) => DeleteValue(key, value); // The Registry has no concept of "will delete this when I see it"

        public void SetValue(string key, string value, object data, RegistryValueKind dataType)
        {
            data = data switch
            {
                uint => new ReinterpretableDword { Unsigned = Convert.ToUInt32(data) }.Signed,
                ulong => new ReinterpretableQword { Unsigned = (ulong)data }.Signed,
                _ => data
            };

            using var regKey = EncapsulatedRegistry.CreateSubKey(key);
            regKey?.SetValue(value, data, dataType);
        }

        public bool ContainsValue(string key, string value)
        {
            using var regKey = EncapsulatedRegistry.OpenSubKey(key);
            if (regKey is null)
            {
                return false;
            }

            return string.IsNullOrEmpty(value) || regKey.GetValueNames().Any(s => s.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public object? GetValue(string key, string value)
        {
            using var regKey = EncapsulatedRegistry.OpenSubKey(key, false);
            if (regKey is null)
            {
                return null;
            }

            var data = regKey.GetValue(value, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
            return data switch
            {
                int => new ReinterpretableDword { Signed = (int)data }.Unsigned,
                long => new ReinterpretableQword { Signed = (long)data }.Unsigned,
                _ => data
            };
        }

        public List<string> GetValueNames(string key)
        {
            using var regKey = EncapsulatedRegistry.OpenSubKey(key);
            return regKey is null ? new List<string>() : regKey.GetValueNames().ToList();
        }

        public bool WillDeleteValue(string key, string value) => false;

        public static bool IsPolicyKey(string keyPath) => PolicyKeys.Any(pk => keyPath.StartsWith(pk + @"\", StringComparison.InvariantCultureIgnoreCase) || keyPath.Equals(pk, StringComparison.InvariantCultureIgnoreCase));

        public void ClearKey(string key)
        {
            foreach (var value in GetValueNames(key))
            {
                ForgetValue(key, value);
            }
        }

        public void ForgetKeyClearance(string key)
        {
            // Does nothing
        }

        public RegistryKey EncapsulatedRegistry { get; private set; }

        public static IEnumerable<string> PolicyKeys =>
            // Values outside these branches are not tracked by PolFile.ApplyDifference
            new[] { @"software\policies", @"software\microsoft\windows\currentversion\policies", @"system\currentcontrolset\policies" };
    }
}