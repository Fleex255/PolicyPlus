using System.Collections.Generic;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.Helpers
{
    public class FilterConfiguration
    {
        public bool? ManagedPolicy;
        public FilterPolicyState? PolicyState;
        public bool? Commented;
        public List<PolicyPlusProduct> AllowedProducts;
        public bool AlwaysMatchAny;
        public bool MatchBlankSupport;
    }
}