using System.Collections.Generic;
using PolicyPlus.csharp.Elements;

namespace PolicyPlus.csharp.Models.Sources.Admx
{
    public class AdmxPolicy
    {
        public string Id;
        public AdmxPolicySection Section;
        public string CategoryId;
        public string DisplayCode;
        public string ExplainCode;
        public string SupportedCode;
        public string PresentationId;
        public string ClientExtension;
        public string RegistryKey;
        public string RegistryValue;
        public PolicyRegistryList AffectedValues;
        public List<PolicyElement> Elements;
        public AdmxFile DefinedIn;
    }
}