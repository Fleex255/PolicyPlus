using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class LoadedSupportDefinitions
    {
        private IEnumerable<PolicyPlusSupport> _definitions;

        public LoadedSupportDefinitions()
        {
            InitializeComponent();
        }

        public void PresentDialog(AdmxBundle workspace)
        {
            TextFilter.Text = "";
            _definitions = workspace.SupportDefinitions.Values;
            UpdateListing();
            ChName.Width = LsvSupport.ClientRectangle.Width - ChDefinedIn.Width - SystemInformation.VerticalScrollBarWidth;
            _ = ShowDialog(this);
        }

        public void UpdateListing()
        {
            // Add all the (matching) support definitions to the list view
            LsvSupport.Items.Clear();
            foreach (var support in _definitions.OrderBy(s => s.DisplayName.Trim())) // Some default support definitions have leading spaces
            {
                if (!support.DisplayName.Contains(TextFilter.Text, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var lsvi = LsvSupport.Items.Add(support.DisplayName.Trim());
                _ = lsvi.SubItems.Add(Path.GetFileName(support.RawSupport.DefinedIn.SourceFile));
                lsvi.Tag = support;
            }
        }

        private void LsvSupport_DoubleClick(object sender, EventArgs e) => new DetailSupport().PresentDialog((PolicyPlusSupport)LsvSupport.SelectedItems[0].Tag);

        private void TextFilter_TextChanged(object sender, EventArgs e)
        {
            // Only repopulate if the form isn't still setting up
            if (Visible)
            {
                UpdateListing();
            }
        }

        private void LsvSupport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && LsvSupport.SelectedItems.Count > 0)
            {
                LsvSupport_DoubleClick(sender, e);
            }
        }
    }
}