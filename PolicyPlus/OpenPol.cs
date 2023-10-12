using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public partial class OpenPol
    {
        public PolicyLoader SelectedUser, SelectedComputer;

        public OpenPol()
        {
            InitializeComponent();
        }
        public void SetLastSources(PolicyLoaderSource ComputerType, string ComputerData, PolicyLoaderSource UserType, string UserData)
        {
            // Set up the UI to show the given configuration
            switch (ComputerType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        CompLocalOption.Checked = true;
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        CompRegistryOption.Checked = true;
                        CompRegTextbox.Text = ComputerData;
                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        CompFileOption.Checked = true;
                        CompPolFilenameTextbox.Text = ComputerData;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        CompNullOption.Checked = true;
                        break;
                    }
            }
            switch (UserType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        UserLocalOption.Checked = true;
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        UserRegistryOption.Checked = true;
                        UserRegTextbox.Text = UserData;
                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        UserFileOption.Checked = true;
                        UserPolFilenameTextbox.Text = UserData;
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        UserPerUserGpoOption.Checked = true;
                        UserGpoSidTextbox.Text = UserData;
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        UserPerUserRegOption.Checked = true;
                        UserHivePathTextbox.Text = UserData;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        UserNullOption.Checked = true;
                        break;
                    }
            }
        }
        public void BrowseForPol(TextBox DestTextbox)
        {
            // Browse for a POL file and put it in a text box
            using (var sfd = new SaveFileDialog())
            {
                sfd.OverwritePrompt = false;
                sfd.Filter = "Registry policy files|*.pol";
                if (sfd.ShowDialog() == DialogResult.OK)
                    DestTextbox.Text = sfd.FileName;
            }
        }
        private void CompOptionsCheckedChanged(object sender, EventArgs e)
        {
            // When the user changes the computer selection
            bool regMount = CompRegistryOption.Checked;
            CompRegTextbox.Enabled = regMount;
            bool polActive = CompFileOption.Checked;
            CompPolFilenameTextbox.Enabled = polActive;
            CompFileBrowseButton.Enabled = polActive;
        }
        private void UserOptionsCheckedChanged(object sender, EventArgs e)
        {
            // When the user changes the user selection
            bool regMount = UserRegistryOption.Checked;
            UserRegTextbox.Enabled = regMount;
            bool file = UserFileOption.Checked;
            UserPolFilenameTextbox.Enabled = file;
            UserFileBrowseButton.Enabled = file;
            bool perUserGpo = UserPerUserGpoOption.Checked;
            UserGpoSidTextbox.Enabled = perUserGpo;
            UserBrowseGpoButton.Enabled = perUserGpo;
            bool perUserHive = UserPerUserRegOption.Checked;
            UserHivePathTextbox.Enabled = perUserHive;
            UserBrowseHiveButton.Enabled = perUserHive;
        }
        private void CompFileBrowseButton_Click(object sender, EventArgs e)
        {
            BrowseForPol(CompPolFilenameTextbox);
        }
        private void UserFileBrowseButton_Click(object sender, EventArgs e)
        {
            BrowseForPol(UserPolFilenameTextbox);
        }
        private void UserBrowseRegistryButton_Click(object sender, EventArgs e)
        {
            // Browse for a user Registry hive
            if (My.MyProject.Forms.OpenUserRegistry.ShowDialog() == DialogResult.OK)
                UserHivePathTextbox.Text = My.MyProject.Forms.OpenUserRegistry.SelectedFilePath;
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            // Create the policy loaders
            try
            {
                if (CompLocalOption.Checked)
                {
                    SelectedComputer = new PolicyLoader(PolicyLoaderSource.LocalGpo, "", false);
                }
                else if (CompRegistryOption.Checked)
                {
                    SelectedComputer = new PolicyLoader(PolicyLoaderSource.LocalRegistry, CompRegTextbox.Text, false);
                }
                else if (CompFileOption.Checked)
                {
                    SelectedComputer = new PolicyLoader(PolicyLoaderSource.PolFile, CompPolFilenameTextbox.Text, false);
                }
                else
                {
                    SelectedComputer = new PolicyLoader(PolicyLoaderSource.Null, "", false);
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("The computer policy loader could not be created. " + ex.Message, MsgBoxStyle.Exclamation);
                return;
            }
            try
            {
                if (UserLocalOption.Checked)
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.LocalGpo, "", true);
                }
                else if (UserRegistryOption.Checked)
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.LocalRegistry, UserRegTextbox.Text, true);
                }
                else if (UserFileOption.Checked)
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.PolFile, UserPolFilenameTextbox.Text, true);
                }
                else if (UserPerUserGpoOption.Checked)
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.SidGpo, UserGpoSidTextbox.Text, true);
                }
                else if (UserPerUserRegOption.Checked)
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.NtUserDat, UserHivePathTextbox.Text, true);
                }
                else
                {
                    SelectedUser = new PolicyLoader(PolicyLoaderSource.Null, "", true);
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("The user policy loader could not be created. " + ex.Message, MsgBoxStyle.Exclamation);
                return;
            }
            DialogResult = DialogResult.OK;
        }
        private void UserBrowseGpoButton_Click(object sender, EventArgs e)
        {
            // Browse for a per-user GPO
            if (My.MyProject.Forms.OpenUserGpo.ShowDialog() == DialogResult.OK)
                UserGpoSidTextbox.Text = My.MyProject.Forms.OpenUserGpo.SelectedSid;
        }
        private void OpenPol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}