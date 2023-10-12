using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class LoadedSupportDefinitions
    {
        private IEnumerable<PolicyPlusSupport> Definitions;

        public LoadedSupportDefinitions()
        {
            InitializeComponent();
        }
        public void PresentDialog(AdmxBundle Workspace)
        {
            TextFilter.Text = "";
            Definitions = Workspace.SupportDefinitions.Values;
            UpdateListing();
            ChName.Width = LsvSupport.ClientRectangle.Width - ChDefinedIn.Width - SystemInformation.VerticalScrollBarWidth;
            ShowDialog();
        }
        public void UpdateListing()
        {
            // Add all the (matching) support definitions to the list view
            LsvSupport.Items.Clear();
            foreach (var support in Definitions.OrderBy(s => s.DisplayName.Trim())) // Some default support definitions have leading spaces
            {
                if (!support.DisplayName.ToLowerInvariant().Contains(TextFilter.Text.ToLowerInvariant()))
                    continue;
                var lsvi = LsvSupport.Items.Add(support.DisplayName.Trim());
                lsvi.SubItems.Add(System.IO.Path.GetFileName(support.RawSupport.DefinedIn.SourceFile));
                lsvi.Tag = support;
            }
        }
        private void LsvSupport_DoubleClick(object sender, EventArgs e)
        {
            My.MyProject.Forms.DetailSupport.PresentDialog((PolicyPlusSupport)LsvSupport.SelectedItems[0].Tag);
        }
        private void TextFilter_TextChanged(object sender, EventArgs e)
        {
            // Only repopulate if the form isn't still setting up
            if (Visible)
                UpdateListing();
        }
        private void LsvSupport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & LsvSupport.SelectedItems.Count > 0)
                LsvSupport_DoubleClick(sender, e);
        }
    }
}