using System;
using System.Linq;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class FindByRegistry
    {
        public Func<PolicyPlusPolicy, bool> Searcher;

        public FindByRegistry()
        {
            InitializeComponent();
        }

        private void FindByRegistry_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var keyName = KeyTextbox.Text.ToLowerInvariant();
            var valName = ValueTextbox.Text.ToLowerInvariant();
            if (string.IsNullOrEmpty(keyName) && string.IsNullOrEmpty(valName))
            {
                _ = MessageBox.Show("Please enter search terms.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (new[] { @"HKLM\", @"HKCU\", @"HKEY_LOCAL_MACHINE\", @"HKEY_CURRENT_USER\" }.Any(bad => keyName.StartsWith(bad, StringComparison.InvariantCultureIgnoreCase)))
            {
                _ = MessageBox.Show("Policies' root keys are determined only by their section. Remove the root key from the search terms and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Searcher = new Func<PolicyPlusPolicy, bool>((policy) =>
                {
                    foreach (var rkvp in PolicyProcessing.GetReferencedRegistryValues(policy))
                    {
                        if (!string.IsNullOrEmpty(valName))
                        {
                            if (!string.Equals(rkvp.Value.ToLowerInvariant(), valName, StringComparison.Ordinal))
                            {
                                continue;
                            }
                        }
                        if (!string.IsNullOrEmpty(keyName))
                        {
                            if (keyName.Contains('*') || keyName.Contains('?')) // Wildcard path
                            {
                                if (!string.Equals(rkvp.Key.ToLowerInvariant(), keyName, StringComparison.Ordinal))
                                {
                                    continue;
                                }
                            }
                            else if (keyName.Contains('\\')) // Path root
                            {
                                if (!rkvp.Key.StartsWith(keyName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    continue;
                                }
                            }
                            else if (!rkvp.Key.Split(@"\").Any(part => part.Equals(keyName, StringComparison.InvariantCultureIgnoreCase))) // One path component
                            {
                                continue;
                            }
                        }
                        return true;
                    }
                    return false;
                });
            DialogResult = DialogResult.OK;
        }
    }
}