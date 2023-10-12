using System.Windows.Forms;

namespace PolicyPlus.UI.Elements
{
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