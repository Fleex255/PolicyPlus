using System.Collections.Generic;

namespace PolicyPlus.Models
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