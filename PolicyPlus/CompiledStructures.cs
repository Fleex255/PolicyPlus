using System.Collections.Generic;

namespace PolicyPlus
{
    // Compiled data, more object-oriented and display-worthy than raw data from ADMX files
    public class PolicyPlusCategory
    {
        public string UniqueID;
        public PolicyPlusCategory Parent;
        public List<PolicyPlusCategory> Children = new List<PolicyPlusCategory>();
        public string DisplayName;
        public string DisplayExplanation;
        public List<PolicyPlusPolicy> Policies = new List<PolicyPlusPolicy>();
        public AdmxCategory RawCategory;
    }

    public class PolicyPlusProduct
    {
        public string UniqueID;
        public PolicyPlusProduct Parent;
        public List<PolicyPlusProduct> Children = new List<PolicyPlusProduct>();
        public string DisplayName;
        public AdmxProduct RawProduct;
    }

    public class PolicyPlusSupport
    {
        public string UniqueID;
        public string DisplayName;
        public List<PolicyPlusSupportEntry> Elements = new List<PolicyPlusSupportEntry>();
        public AdmxSupportDefinition RawSupport;
    }

    public class PolicyPlusSupportEntry
    {
        public PolicyPlusProduct Product;
        public PolicyPlusSupport SupportDefinition; // Only used if this entry actually points to another support definition
        public AdmxSupportEntry RawSupportEntry;
    }

    public class PolicyPlusPolicy
    {
        public string UniqueID;
        public PolicyPlusCategory Category;
        public string DisplayName;
        public string DisplayExplanation;
        public PolicyPlusSupport SupportedOn;
        public Presentation Presentation;
        public AdmxPolicy RawPolicy;
    }
}