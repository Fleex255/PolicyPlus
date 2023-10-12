using System.Collections.Generic;
using Microsoft.Win32;

namespace PolicyPlus.Models
{
    public interface IPolicySource
    {
        bool ContainsValue(string key, string value);

        object GetValue(string key, string value);

        bool WillDeleteValue(string key, string value);

        List<string> GetValueNames(string key);

        void SetValue(string key, string value, object data, RegistryValueKind dataType);

        void ForgetValue(string key, string value); // Stop keeping track of a value

        void DeleteValue(string key, string value); // Mark a value as queued for deletion

        void ClearKey(string key); // Destroy all values in a key

        void ForgetKeyClearance(string key); // Unmark a key as cleared
    }
}