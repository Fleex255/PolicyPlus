using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public partial class InspectSpolFragment
    {
        private string SpolFragment;

        public InspectSpolFragment()
        {
            InitializeComponent();
        }
        public DialogResult PresentDialog(PolicyPlusPolicy Policy, AdmxBundle AdmxWorkspace, IPolicySource CompSource, IPolicySource UserSource, Dictionary<string, string> CompComments, Dictionary<string, string> UserComments)
        {
            // Show the SPOL text for all the policy's sections
            TextPolicyName.Text = Policy.DisplayName;
            var sb = new System.Text.StringBuilder();
            bool addSection(AdmxPolicySection Section)
            {
                // Create SPOL info for one section of the policy
                if ((Policy.RawPolicy.Section & Section) == 0)
                    return false;
                var polSource = Section == AdmxPolicySection.Machine ? CompSource : UserSource;
                var commentsMap = Section == AdmxPolicySection.Machine ? CompComments : UserComments;
                var spolState = new SpolPolicyState() { UniqueID = Policy.UniqueID };
                spolState.Section = Section;
                if (commentsMap is not null && commentsMap.ContainsKey(Policy.UniqueID))
                    spolState.Comment = commentsMap[Policy.UniqueID];
                spolState.BasicState = PolicyProcessing.GetPolicyState(polSource, Policy);
                if (spolState.BasicState == PolicyState.Enabled)
                    spolState.ExtraOptions = PolicyProcessing.GetPolicyOptionStates(polSource, Policy);
                sb.AppendLine(SpolFile.GetFragment(spolState));
                return true;
            };
            addSection(AdmxPolicySection.Machine);
            addSection(AdmxPolicySection.User);
            SpolFragment = sb.ToString();
            UpdateTextbox();
            return ShowDialog();
        }
        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            TextSpol.SelectAll();
            TextSpol.Copy();
        }
        private void InspectSpolFragment_Shown(object sender, EventArgs e)
        {
            TextSpol.Focus();
            TextSpol.SelectAll();
        }
        private void CheckHeader_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTextbox();
        }
        private void UpdateTextbox()
        {
            if (CheckHeader.Checked)
            {
                TextSpol.Text = "Policy Plus Semantic Policy" + Constants.vbCrLf + Constants.vbCrLf + SpolFragment;
            }
            else
            {
                TextSpol.Text = SpolFragment;
            }
        }
        private void TextSpol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A & e.Control)
                TextSpol.SelectAll();
        }
    }
}