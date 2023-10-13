﻿using System.Collections.Generic;

namespace PolicyPlus.csharp.Models.Sources.Admx
{
    public class AdmxSupportDefinition
    {
        public string Id;
        public string DisplayCode;
        public AdmxSupportLogicType Logic;
        public List<AdmxSupportEntry> Entries;
        public AdmxFile DefinedIn;
    }
}