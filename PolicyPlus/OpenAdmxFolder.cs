using System;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public partial class OpenAdmxFolder
    {
        private string SysvolPolicyDefinitionsPath = "";
        public string SelectedFolder;
        public bool ClearWorkspace;

        public OpenAdmxFolder()
        {
            InitializeComponent();
        }
        private void Options_CheckedChanged(object sender, EventArgs e)
        {
            bool customSelected = OptCustomFolder.Checked;
            TextFolder.Enabled = customSelected;
            ButtonBrowse.Enabled = customSelected;
        }
        private void OpenAdmxFolder_Shown(object sender, EventArgs e)
        {
            OptCustomFolder.Checked = true;
            Domain compDomain = null;
            try
            {
                compDomain = Domain.GetComputerDomain();
            }
            catch (Exception ex)
            {
                // Not domain-joined, or no domain controller is available
            }
            if (compDomain is null)
            {
                SysvolPolicyDefinitionsPath = "";
            }
            else
            {
                string possiblePath = @"\\" + compDomain.Name + @"\SYSVOL\" + compDomain.Name + @"\Policies\PolicyDefinitions";
                if (System.IO.Directory.Exists(possiblePath))
                    SysvolPolicyDefinitionsPath = possiblePath;
                else
                    SysvolPolicyDefinitionsPath = "";
            }
            OptSysvol.Enabled = !string.IsNullOrEmpty(SysvolPolicyDefinitionsPath);
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (OptLocalFolder.Checked)
            {
                SelectedFolder = Environment.ExpandEnvironmentVariables(@"%windir%\PolicyDefinitions");
            }
            else if (OptSysvol.Checked)
            {
                SelectedFolder = SysvolPolicyDefinitionsPath;
            }
            else if (OptCustomFolder.Checked)
            {
                SelectedFolder = TextFolder.Text;
            }
            if (System.IO.Directory.Exists(SelectedFolder))
            {
                ClearWorkspace = ClearWorkspaceCheckbox.Checked;
                DialogResult = DialogResult.OK;
            }
            else
            {
                Interaction.MsgBox("The folder you specified does not exist.", MsgBoxStyle.Exclamation);
            }
        }
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;
                TextFolder.Text = fbd.SelectedPath;
            }
        }
        private void OpenAdmxFolder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}