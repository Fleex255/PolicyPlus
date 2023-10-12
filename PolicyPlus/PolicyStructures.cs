using System.Collections.Generic;

namespace PolicyPlus
{
    // These structures hold information on the behavior of policies and policy elements
    public class PolicyRegistryList
    {
        public PolicyRegistryValue OnValue;
        public PolicyRegistrySingleList OnValueList;
        public PolicyRegistryValue OffValue;
        public PolicyRegistrySingleList OffValueList;
    }

    public class PolicyRegistrySingleList
    {
        public string DefaultRegistryKey;
        public List<PolicyRegistryListEntry> AffectedValues;
    }

    public class PolicyRegistryValue // <value>
    {
        public PolicyRegistryValueType RegistryType;
        public string StringValue;
        public uint NumberValue;
    }

    public class PolicyRegistryListEntry // <item>
    {
        public string RegistryValue;
        public string RegistryKey;
        public PolicyRegistryValue Value;
    }

    public enum PolicyRegistryValueType
    {
        Delete,
        Numeric,
        Text
    }

    public abstract class PolicyElement
    {
        public string ID;
        public string ClientExtension;
        public string RegistryKey;
        public string RegistryValue;
        public string ElementType;
    }
    public class DecimalPolicyElement : PolicyElement // <decimal>
    {
        public bool Required;
        public uint Minimum;
        public uint Maximum = uint.MaxValue;
        public bool StoreAsText;
        public bool NoOverwrite;
    }
    public class BooleanPolicyElement : PolicyElement // <boolean>
    {
        public PolicyRegistryList AffectedRegistry;
    }
    public class TextPolicyElement : PolicyElement // <text>
    {
        public bool Required;
        public int MaxLength;
        public bool RegExpandSz;
        public bool NoOverwrite;
    }
    public class ListPolicyElement : PolicyElement // <list>
    {
        public bool HasPrefix;
        public bool NoPurgeOthers;
        public bool RegExpandSz;
        public bool UserProvidesNames;
    }
    public class EnumPolicyElement : PolicyElement // <enum>
    {
        public bool Required;
        public List<EnumPolicyElementItem> Items;
    }

    public class EnumPolicyElementItem // <item>
    {
        public string DisplayCode;
        public PolicyRegistryValue Value;
        public PolicyRegistrySingleList ValueList; // <valueList>
    }
    public class MultiTextPolicyElement : PolicyElement // <multiText>
    {
        // This is undocumented, so it's unknown whether there can be other options for it
    }
}