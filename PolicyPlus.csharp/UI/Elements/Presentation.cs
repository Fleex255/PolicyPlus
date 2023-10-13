using System.Collections.Generic;

namespace PolicyPlus.csharp.UI.Elements
{
    // These structures hold information on how the UI for policy elements appears
    public class Presentation
    {
        public string Name;
        public List<PresentationElement> Elements = new();
    }
}