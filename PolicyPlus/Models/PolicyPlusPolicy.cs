using PolicyPlus.UI.Elements;

namespace PolicyPlus.Models
{
    public class PolicyPlusPolicy
    {
        public string UniqueId;
        public PolicyPlusCategory Category;
        public string DisplayName;
        public string DisplayExplanation;
        public PolicyPlusSupport SupportedOn;
        public Presentation Presentation;
        public AdmxPolicy RawPolicy;
    }
}