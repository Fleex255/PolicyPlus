using System;
using System.Windows.Forms;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class OpenPol
    {
        public PolicyLoader SelectedUser { get; set; }
        public PolicyLoader SelectedComputer { get; set; }

        public OpenPol()
        {
            InitializeComponent();
        }

        public void SetLastSources(PolicyLoaderSource computerType, string computerData, PolicyLoaderSource userType, string userData)
        {
            // Set up the UI to show the given configuration
            switch (computerType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        CompLocalOption.Checked = true;
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        CompRegistryOption.Checked = true;
                        CompRegTextbox.Text = computerData;
                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        CompFileOption.Checked = true;
                        CompPolFilenameTextbox.Text = computerData;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        CompNullOption.Checked = true;
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    break;
                case PolicyLoaderSource.NtUserDat:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(computerType), computerType, null);
            }
            switch (userType)
            {
                case PolicyLoaderSource.LocalGpo:
                    {
                        UserLocalOption.Checked = true;
                        break;
                    }
                case PolicyLoaderSource.LocalRegistry:
                    {
                        UserRegistryOption.Checked = true;
                        UserRegTextbox.Text = userData;
                        break;
                    }
                case PolicyLoaderSource.PolFile:
                    {
                        UserFileOption.Checked = true;
                        UserPolFilenameTextbox.Text = userData;
                        break;
                    }
                case PolicyLoaderSource.SidGpo:
                    {
                        UserPerUserGpoOption.Checked = true;
                        UserGpoSidTextbox.Text = userData;
                        break;
                    }
                case PolicyLoaderSource.NtUserDat:
                    {
                        UserPerUserRegOption.Checked = true;
                        UserHivePathTextbox.Text = userData;
                        break;
                    }
                case PolicyLoaderSource.Null:
                    {
                        UserNullOption.Checked = true;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        public void BrowseForPol(TextBox destTextbox)
        {
            // Browse for a POL file and put it in a text box
            using var sfd = new SaveFileDialog();
            sfd.OverwritePrompt = false;
            sfd.Filter = "Registry policy files|*.pol";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                destTextbox.Text = sfd.FileName;
            }
        }

        private void CompOptionsCheckedChanged(object sender, EventArgs e)
        {
            // When the user changes the computer selection
            CompRegTextbox.Enabled = CompRegistryOption.Checked;
            var polActive = CompFileOption.Checked;
            CompPolFilenameTextbox.Enabled = polActive;
            CompFileBrowseButton.Enabled = polActive;
        }

        private void UserOptionsCheckedChanged(object sender, EventArgs e)
        {
            // When the user changes the user selection
            UserRegTextbox.Enabled = UserRegistryOption.Checked;
            var file = UserFileOption.Checked;
            UserPolFilenameTextbox.Enabled = file;
            UserFileBrowseButton.Enabled = file;
            var perUserGpo = UserPerUserGpoOption.Checked;
            UserGpoSidTextbox.Enabled = perUserGpo;
            UserBrowseGpoButton.Enabled = perUserGpo;
            var perUserHive = UserPerUserRegOption.Checked;
            UserHivePathTextbox.Enabled = perUserHive;
            UserBrowseHiveButton.Enabled = perUserHive;
        }

        private void CompFileBrowseButton_Click(object sender, EventArgs e) => BrowseForPol(CompPolFilenameTextbox);

        private void UserFileBrowseButton_Click(object sender, EventArgs e) => BrowseForPol(UserPolFilenameTextbox);

        private void UserBrowseRegistryButton_Click(object sender, EventArgs e)
        {
            // Browse for a user Registry hive
            var dialog = new OpenUserRegistry();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                UserHivePathTextbox.Text = dialog.SelectedFilePath;
            }
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
                _ = MessageBox.Show($"The computer policy loader could not be created. {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                _ = MessageBox.Show($"The user policy loader could not be created. {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void UserBrowseGpoButton_Click(object sender, EventArgs e)
        {
            // Browse for a per-user GPO
            var dialog = new OpenUserGpo();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                UserGpoSidTextbox.Text = dialog.SelectedSid;
            }
        }

        private void OpenPol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}