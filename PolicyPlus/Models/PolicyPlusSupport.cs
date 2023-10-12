using System.Collections.Generic;

namespace PolicyPlus.Models
{
    public class PolicyPlusSupport
    {
        public string UniqueId;
        public string DisplayName;
        public List<PolicyPlusSupportEntry> Elements = new();
        public AdmxSupportDefinition RawSupport;
    }
}