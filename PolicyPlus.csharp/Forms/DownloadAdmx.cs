using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolicyPlus.csharp.Helpers;
using PolicyPlus.csharp.Models;

namespace PolicyPlus.csharp.UI
{
    public partial class DownloadAdmx
    {
        private const string MicrosoftMsiDownloadLink = "https://download.microsoft.com/download/4/e/0/4e0fe8d7-ba82-4354-9cc2-18ac02cfd6b5/Administrative%20Templates%20(.admx)%20for%20Windows%2010%20November%202021%20Update.msi";
        private const string PolicyDefinitionsMsiSubdirectory = @"\Microsoft Group Policy\Windows 10 November 2021 Update (21H2)\PolicyDefinitions";
        private bool _downloading;
        public string NewPolicySourceFolder;

        public DownloadAdmx()
        {
            InitializeComponent();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                TextDestFolder.Text = fbd.SelectedPath;
            }
        }

        private void DownloadAdmx_Closing(object sender, CancelEventArgs e)
        {
            if (_downloading)
            {
                e.Cancel = true;
            }
        }

        private void DownloadAdmx_Shown(object sender, EventArgs e)
        {
            TextDestFolder.Text = Environment.ExpandEnvironmentVariables(@"%windir%\PolicyDefinitions");
            NewPolicySourceFolder = string.Empty;
            SetIsBusy(false);
        }

        public void SetIsBusy(bool busy)
        {
            _downloading = busy;
            LabelProgress.Visible = busy;
            ButtonClose.Enabled = !busy;
            ButtonStart.Enabled = !busy;
            TextDestFolder.Enabled = !busy;
            ButtonBrowse.Enabled = !busy;
            ProgressSpinner.MarqueeAnimationSpeed = busy ? 100 : 0;
            ProgressSpinner.Style = busy ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            ProgressSpinner.Value = 0;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            LabelProgress.Text = string.Empty;
            SetIsBusy(true);
            var destination = TextDestFolder.Text;
            var isAdmin = false;
            using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
            {
                isAdmin = new System.Security.Principal.WindowsPrincipal(identity).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }

            _ = Task.Factory.StartNew(() =>
                {
                    var failPhase = "create a scratch space";
                    try
                    {
                        var tempPath = Environment.ExpandEnvironmentVariables(@"%localappdata%\PolicyPlusAdmxDownload\");
                        _ = Directory.CreateDirectory(tempPath);
                        failPhase = "download the package";
                        SetProgress("Downloading MSI from Microsoft...");
                        _ = Download.DownloadAndSave(MicrosoftMsiDownloadLink, tempPath, "W10Admx.msi");
                        failPhase = "extract the package";
                        SetProgress("Unpacking MSI...");
                        var unpackPath = tempPath + "MsiUnpack";

                        var downloadPath = tempPath + "W10Admx.msi";
                        var proc = Process.Start("msiexec", "/a \"" + downloadPath + "\" /quiet /qn TARGETDIR=\"" + unpackPath + "\"");
                        proc.WaitForExit();
                        if (proc.ExitCode != 0)
                        {
                            throw new Exception(); // msiexec failed
                        }

                        File.Delete(downloadPath);
                        if (Directory.Exists(destination) && isAdmin)
                        {
                            failPhase = "take control of the destination";
                            SetProgress("Securing destination...");
                            Privilege.EnablePrivilege("SeTakeOwnershipPrivilege");
                            Privilege.EnablePrivilege("SeRestorePrivilege");
                            TakeOwnership(destination);
                        }
                        failPhase = "move the ADMX files";
                        SetProgress("Moving files to destination...");
                        var unpackedDefsPath = unpackPath + PolicyDefinitionsMsiSubdirectory;
                        var langSubfolder = System.Globalization.CultureInfo.CurrentCulture.Name;
                        MoveFilesInDir(unpackedDefsPath, destination, false, isAdmin);
                        var sourceAdmlPath = unpackedDefsPath + @"\" + langSubfolder;
                        if (Directory.Exists(sourceAdmlPath))
                        {
                            MoveFilesInDir(sourceAdmlPath, destination + @"\" + langSubfolder, true, isAdmin);
                        }

                        if (langSubfolder != "en-US")
                        {
                            // Also copy the English language files as a fallback
                            MoveFilesInDir(unpackedDefsPath + @"\en-US", destination + @"\en-US", true, isAdmin);
                        }
                        failPhase = "remove temporary files";
                        SetProgress("Cleaning up...");
                        Directory.Delete(tempPath, true);
                        SetProgress("Done.");
                        Invoke(new Action(() =>
                          {
                              SetIsBusy(false);

                              if (MessageBox.Show("Please enter search terms.", "Search", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                              {
                                  NewPolicySourceFolder = destination;
                              }
                              DialogResult = DialogResult.OK;
                          }));
                    }
                    catch (Exception)
                    {
                        Invoke(new Action(() =>
                        {
                            SetIsBusy(false);
                            _ = MessageBox.Show($"Failed to {failPhase}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }));
                    }
                });
        }

        private static void MoveFilesInDir(string source, string dest, bool inheritAcl, bool isAdmin)
        {
            var creatingNew = !Directory.Exists(dest);
            _ = Directory.CreateDirectory(dest);
            if (isAdmin)
            {
                switch (creatingNew)
                {
                    case true when inheritAcl:
                        {
                            var dirAcl = new System.Security.AccessControl.DirectorySecurity();
                            dirAcl.SetAccessRuleProtection(false, true);
                            new DirectoryInfo(dest).SetAccessControl(dirAcl);
                            break;
                        }
                    case false:
                        TakeOwnership(dest);
                        break;
                }
            }
            foreach (var file in Directory.EnumerateFiles(source))
            {
                var plainFilename = Path.GetFileName(file);
                var newName = Path.Combine(dest, plainFilename);
                if (File.Exists(newName))
                {
                    File.Delete(newName);
                }

                File.Move(file, newName);
                if (!isAdmin)
                {
                    continue;
                }

                var fileAcl = new System.Security.AccessControl.FileSecurity();
                fileAcl.SetAccessRuleProtection(false, true);
                new FileInfo(newName).SetAccessControl(fileAcl);
            }
        }

        private static void TakeOwnership(string folder)
        {
            var dacl = new DirectoryInfo(folder).GetAccessControl();
            var adminSid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, null);
            dacl.SetOwner(adminSid);

            new DirectoryInfo(folder).SetAccessControl(dacl);
            dacl = new DirectoryInfo(folder).GetAccessControl();

            var allowRule = new System.Security.AccessControl.FileSystemAccessRule(adminSid, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow);
            dacl.AddAccessRule(allowRule);

            new DirectoryInfo(folder).SetAccessControl(dacl);
        }

        private void SetProgress(string progress) => Invoke(new Action(() => LabelProgress.Text = progress));
    }
}