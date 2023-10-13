using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class InspectSpolFragment
    {
        private string _spolFragment;

        public InspectSpolFragment()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(PolicyPlusPolicy policy, IPolicySource compSource, IPolicySource userSource, Dictionary<string, string> compComments, Dictionary<string, string> userComments)
        {
            // Show the SPOL text for all the policy's sections
            TextPolicyName.Text = policy.DisplayName;
            var sb = new System.Text.StringBuilder();
            _ = AddSection(AdmxPolicySection.Machine);
            _ = AddSection(AdmxPolicySection.User);
            _spolFragment = sb.ToString();
            UpdateTextbox();
            return ShowDialog(this);

            bool AddSection(AdmxPolicySection section)
            {
                // Create SPOL info for one section of the policy
                if ((policy.RawPolicy.Section & section) == 0)
                {
                    return false;
                }

                var polSource = section == AdmxPolicySection.Machine ? compSource : userSource;
                var commentsMap = section == AdmxPolicySection.Machine ? compComments : userComments;
                var spolState = new SpolPolicyState
                {
                    UniqueId = policy.UniqueId,
                    Section = section
                };
                if (commentsMap is not null && commentsMap.TryGetValue(policy.UniqueId, out var value))
                {
                    spolState.Comment = value;
                }

                spolState.BasicState = PolicyProcessing.GetPolicyState(polSource, policy);
                if (spolState.BasicState == PolicyState.Enabled)
                {
                    spolState.ExtraOptions = PolicyProcessing.GetPolicyOptionStates(polSource, policy);
                }

                _ = sb.AppendLine(SpolFile.GetFragment(spolState));
                return true;
            }
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            TextSpol.SelectAll();
            TextSpol.Copy();
        }

        private void InspectSpolFragment_Shown(object sender, EventArgs e)
        {
            _ = TextSpol.Focus();
            TextSpol.SelectAll();
        }

        private void CheckHeader_CheckedChanged(object sender, EventArgs e) => UpdateTextbox();

        private void UpdateTextbox()
        {
            if (CheckHeader.Checked)
            {
                TextSpol.Text = "Policy Plus Semantic Policy" + Environment.NewLine + Environment.NewLine + _spolFragment;
            }
            else
            {
                TextSpol.Text = _spolFragment;
            }
        }

        private void TextSpol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                TextSpol.SelectAll();
            }
        }
    }
}