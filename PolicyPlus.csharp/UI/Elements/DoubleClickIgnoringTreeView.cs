using System.Windows.Forms;

namespace PolicyPlus.csharp.UI.Elements
{
    // The TreeView control has a b*g: the displayed check state gets out of sync with the Checked
    // property when the checkbox is double-clicked Fix adapted from https://stackoverflow.com/a/3174824
    internal class DoubleClickIgnoringTreeView : TreeView
    {
        protected override void WndProc(ref Message m)
        {
            // Ignore WM_LBUTTONDBLCLK
            if (m.Msg != 0x203)
            {
                base.WndProc(ref m);
            }
        }
    }
}