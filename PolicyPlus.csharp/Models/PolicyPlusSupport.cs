using System.Collections.Generic;

namespace PolicyPlus.csharp.Models
{
    public class PolicyPlusSupport
    {
        public string UniqueId;
        public string DisplayName;
        public List<PolicyPlusSupportEntry> Elements = new();
        public AdmxSupportDefinition RawSupport;
    }
}