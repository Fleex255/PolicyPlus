using System.Collections.Generic;

namespace PolicyPlus.Models
{
    // Compiled data, more object-oriented and display-worthy than raw data from ADMX files
    public class PolicyPlusCategory
    {
        public string UniqueId;
        public PolicyPlusCategory Parent;
        public List<PolicyPlusCategory> Children = new();
        public string DisplayName;
        public string DisplayExplanation;
        public List<PolicyPlusPolicy> Policies = new();
        public AdmxCategory RawCategory;
    }
}