using System;

namespace PolicyPlus.Models
{
    public class RegistryKeyValuePair : IEquatable<RegistryKeyValuePair>
    {
        public string Key;
        public string Value;

        public bool EqualsRkvp(RegistryKeyValuePair other) => other.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase) && other.Value.Equals(Value, StringComparison.InvariantCultureIgnoreCase);

        bool IEquatable<RegistryKeyValuePair>.Equals(RegistryKeyValuePair other) => EqualsRkvp(other);

        public override bool Equals(object obj)
        {
            if (obj is not RegistryKeyValuePair pair)
            {
                return false;
            }

            return EqualsRkvp(pair);
        }

        public override int GetHashCode() => Key.ToLowerInvariant().GetHashCode() ^ Value.ToLowerInvariant().GetHashCode();
    }
}