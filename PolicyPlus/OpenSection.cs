using System;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class OpenSection
    {
        public AdmxPolicySection SelectedSection;

        public OpenSection()
        {
            InitializeComponent();
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            // Report the selected section
            if (OptUser.Checked | OptComputer.Checked)
            {
                SelectedSection = OptUser.Checked ? AdmxPolicySection.User : AdmxPolicySection.Machine;
                DialogResult = DialogResult.OK;
            }
        }
        private void OpenSection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
        private void OpenSection_Shown(object sender, EventArgs e)
        {
            OptUser.Checked = false;
            OptComputer.Checked = false;
        }
        public DialogResult PresentDialog(bool UserEnabled, bool CompEnabled)
        {
            OptUser.Enabled = UserEnabled;
            OptComputer.Enabled = CompEnabled;
            return ShowDialog();
        }
    }
}