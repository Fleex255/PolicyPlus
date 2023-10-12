using System.Collections.Generic;

namespace PolicyPlus
{
    // Raw data loaded from ADMX files
    public class AdmxProduct
    {
        public string ID;
        public string DisplayCode;
        public AdmxProductType Type;
        public int Version;
        public AdmxProduct Parent;
        public AdmxFile DefinedIn;
    }

    public enum AdmxProductType
    {
        Product,
        MajorRevision,
        MinorRevision
    }

    public class AdmxSupportDefinition
    {
        public string ID;
        public string DisplayCode;
        public AdmxSupportLogicType Logic;
        public List<AdmxSupportEntry> Entries;
        public AdmxFile DefinedIn;
    }

    public enum AdmxSupportLogicType
    {
        Blank,
        AllOf,
        AnyOf
    }

    public class AdmxSupportEntry
    {
        public string ProductID;
        public bool IsRange;
        public int? MinVersion;
        public int? MaxVersion;
    }

    public class AdmxCategory
    {
        public string ID;
        public string DisplayCode;
        public string ExplainCode;
        public string ParentID;
        public AdmxFile DefinedIn;
    }

    public class AdmxPolicy
    {
        public string ID;
        public AdmxPolicySection Section;
        public string CategoryID;
        public string DisplayCode;
        public string ExplainCode;
        public string SupportedCode;
        public string PresentationID;
        public string ClientExtension;
        public string RegistryKey;
        public string RegistryValue;
        public PolicyRegistryList AffectedValues;
        public List<PolicyElement> Elements;
        public AdmxFile DefinedIn;
    }

    public enum AdmxPolicySection
    {
        Machine = 1,
        User = 2,
        Both = 3
    }
}