namespace PolicyPlus.Models
{
    // Raw data loaded from ADMX files
    public class AdmxProduct
    {
        public string Id;
        public string DisplayCode;
        public AdmxProductType Type;
        public int Version;
        public AdmxProduct Parent;
        public AdmxFile DefinedIn;
    }
}