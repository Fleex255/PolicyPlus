using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Windows.Forms;


namespace PolicyPlus.csharp.UI
{
    public partial class OpenAdmxFolder
    {
        private string _sysvolPolicyDefinitionsPath = string.Empty;
        public string SelectedFolder;
        public bool ClearWorkspace;

        public OpenAdmxFolder()
        {
            InitializeComponent();
        }

        private void Options_CheckedChanged(object sender, EventArgs e)
        {
            var customSelected = OptCustomFolder.Checked;
            TextFolder.Enabled = customSelected;
            ButtonBrowse.Enabled = customSelected;
        }

        private void OpenAdmxFolder_Shown(object sender, EventArgs e)
        {
            OptCustomFolder.Checked = true;
            Domain? compDomain = null;
            try
            {
                compDomain = Domain.GetComputerDomain();
            }
            catch
            {
                // Not domain-joined, or no domain controller is available
            }
            if (compDomain is null)
            {
                _sysvolPolicyDefinitionsPath = string.Empty;
            }
            else
            {
                var possiblePath = @"\\" + compDomain.Name + @"\SYSVOL\" + compDomain.Name + @"\Policies\PolicyDefinitions";
                _sysvolPolicyDefinitionsPath = Directory.Exists(possiblePath) ? possiblePath : string.Empty;
            }
            OptSysvol.Enabled = !string.IsNullOrEmpty(_sysvolPolicyDefinitionsPath);
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (OptLocalFolder.Checked)
            {
                SelectedFolder = Environment.ExpandEnvironmentVariables(@"%windir%\PolicyDefinitions");
            }
            else if (OptSysvol.Checked)
            {
                SelectedFolder = _sysvolPolicyDefinitionsPath;
            }
            else if (OptCustomFolder.Checked)
            {
                SelectedFolder = TextFolder.Text;
            }
            if (Directory.Exists(SelectedFolder))
            {
                ClearWorkspace = ClearWorkspaceCheckbox.Checked;
                DialogResult = DialogResult.OK;
            }
            else
            {
                _ = MessageBox.Show("The folder you specified does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TextFolder.Text = fbd.SelectedPath;
            }
        }

        private void OpenAdmxFolder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}