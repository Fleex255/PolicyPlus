namespace PolicyPlus.csharp.UI.Elements
{
    public class NumericBoxPresentationElement : PresentationElement // <decimalTextBox>
    {
        public uint DefaultValue; // defaultValue
        public bool HasSpinner = true; // spin
        public uint SpinnerIncrement; // spinStep
        public string Label; // Inner text
    }
}