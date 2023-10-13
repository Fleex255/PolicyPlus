﻿using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI.Elements
{
    public class DecimalPolicyElement : PolicyElement // <decimal>
    {
        public bool Required;
        public uint Minimum;
        public uint Maximum = uint.MaxValue;
        public bool StoreAsText;
        public bool NoOverwrite;
    }
}