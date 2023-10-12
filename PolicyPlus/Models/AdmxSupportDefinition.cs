using System.Collections.Generic;

namespace PolicyPlus.Models
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