using System.Collections.Generic;

namespace PolicyPlus.csharp.Elements
{
    public class EnumPolicyElement : PolicyElement // <enum>
    {
        public bool Required;
        public List<EnumPolicyElementItem> Items;
    }
}