using System.Collections.Generic;
using PolicyPlus.Models;

namespace PolicyPlus.UI.Elements
{
    public class EnumPolicyElement : PolicyElement // <enum>
    {
        public bool Required;
        public List<EnumPolicyElementItem> Items;
    }
}