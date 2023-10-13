namespace PolicyPlus.csharp.Elements
{
    public class TextPolicyElement : PolicyElement // <text>
    {
        public bool Required;
        public int MaxLength;
        public bool RegExpandSz;
        public bool NoOverwrite;
    }
}