namespace PolicyPlus.Models
{
    public class PolicyPlusSupportEntry
    {
        public PolicyPlusProduct Product;
        public PolicyPlusSupport SupportDefinition; // Only used if this entry actually points to another support definition
        public AdmxSupportEntry RawSupportEntry;
    }
}