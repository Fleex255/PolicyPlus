using System.Collections.Generic;
using PolicyPlus.csharp.Models.Sources.Admx;

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