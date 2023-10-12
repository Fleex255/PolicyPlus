using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace PolicyPlus
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
                DialogResult = DialogResult.Cancel;
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            string keyName = KeyTextbox.Text.ToLowerInvariant();
            string valName = ValueTextbox.Text.ToLowerInvariant();
            if (string.IsNullOrEmpty(keyName) & string.IsNullOrEmpty(valName))
            {
                Interaction.MsgBox("Please enter search terms.", MsgBoxStyle.Exclamation);
                return;
            }
            if (new[] { @"HKLM\", @"HKCU\", @"HKEY_LOCAL_MACHINE\", @"HKEY_CURRENT_USER\" }.Any(bad => keyName.StartsWith(bad, StringComparison.InvariantCultureIgnoreCase)))
            {
                Interaction.MsgBox("Policies' root keys are determined only by their section. Remove the root key from the search terms and try again.", MsgBoxStyle.Exclamation);
                return;
            }
            Searcher = new Func<PolicyPlusPolicy, bool>((Policy) =>
                {
                    var affected = PolicyProcessing.GetReferencedRegistryValues(Policy);
                    foreach (var rkvp in affected)
                    {
                        if (!string.IsNullOrEmpty(valName))
                        {
                            if (!LikeOperator.LikeString(rkvp.Value.ToLowerInvariant(), valName, CompareMethod.Binary))
                                continue;
                        }
                        if (!string.IsNullOrEmpty(keyName))
                        {
                            if (keyName.Contains("*") | keyName.Contains("?")) // Wildcard path
                            {
                                if (!LikeOperator.LikeString(rkvp.Key.ToLowerInvariant(), keyName, CompareMethod.Binary))
                                    continue;
                            }
                            else if (keyName.Contains(@"\")) // Path root
                            {
                                if (!rkvp.Key.StartsWith(keyName, StringComparison.InvariantCultureIgnoreCase))
                                    continue;
                            }
                            else if (!Strings.Split(rkvp.Key, @"\").Any(part => part.Equals(keyName, StringComparison.InvariantCultureIgnoreCase))) // One path component
                                continue;
                        }
                        return true;
                    }
                    return false;
                });
            DialogResult = DialogResult.OK;
        }
    }
}