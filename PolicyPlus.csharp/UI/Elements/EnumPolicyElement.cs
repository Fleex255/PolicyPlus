using System.Collections.Generic;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI.Elements
{
    public class EnumPolicyElement : PolicyElement // <enum>
    {
        public bool Required;
        public List<EnumPolicyElementItem> Items;
    }
}