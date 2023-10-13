using System.Collections.Generic;

namespace PolicyPlus.csharp.Elements
{
    public class ComboBoxPresentationElement : PresentationElement // <comboBox>
    {
        public bool NoSort; // noSort
        public string Label; // <label>
        public string DefaultText; // <default>
        public List<string> Suggestions = new(); // <suggestion>s
    }
}