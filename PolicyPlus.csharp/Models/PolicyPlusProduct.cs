using System.Collections.Generic;
using PolicyPlus.csharp.Models.Sources.Admx;

namespace PolicyPlus.csharp.Models
{
    public class PolicyPlusProduct
    {
        public string UniqueId;
        public PolicyPlusProduct Parent;
        public List<PolicyPlusProduct> Children = new();
        public string DisplayName;
        public AdmxProduct RawProduct;
    }
}