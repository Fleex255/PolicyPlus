using Microsoft.Win32;

namespace PolicyPlus.csharp.Models
{
    public class ConfigurationStorage
    {
        private readonly RegistryKey _configKey;

        public ConfigurationStorage(RegistryHive registryBase, string subkey)
        {
            try
            {
                _configKey = RegistryKey.OpenBaseKey(registryBase, RegistryView.Default).CreateSubKey(subkey);
            }
            catch
            {
                // The key couldn't be created
            }
        }

        public object? GetValue(string valueName, object defaultValue) => _configKey is not null ? _configKey.GetValue(valueName, defaultValue) : defaultValue;

        public void SetValue(string valueName, object data) => _configKey?.SetValue(valueName, data);

        public bool HasValue(string valueName) => _configKey?.GetValue(valueName) != null;
    }
}