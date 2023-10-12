using System.Collections.Generic;
using PolicyPlus.Models;

namespace PolicyPlus.Helpers
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