using System;
using System.IO;
using System.Windows.Forms;

namespace PolicyPlus.csharp.UI
{
    public partial class OpenUserRegistry
    {
        public string SelectedFilePath;

        public OpenUserRegistry()
        {
            InitializeComponent();
        }

        private void OpenUserRegistry_Shown(object sender, EventArgs e)
        {
            // Set up the UI
            SubfoldersListview.Columns[1].Width = SubfoldersListview.ClientSize.Width - SubfoldersListview.Columns[0].Width - SystemInformation.VerticalScrollBarWidth;
            SubfoldersListview.Items.Clear();
            // Scan for user hives and whether they can be accessed now
            var canMountHives = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            foreach (var folder in Directory.EnumerateDirectories(@"C:\Users"))
            {
                var dirInfo = new System.IO.DirectoryInfo(folder);
                if ((int)(dirInfo.Attributes & FileAttributes.ReparsePoint) > 0)
                {
                    continue;
                }

                var ntuserPath = folder + @"\ntuser.dat";
                var access = string.Empty;
                try
                {
                    using var fNtuser = new System.IO.FileStream(ntuserPath, FileMode.Open, FileAccess.ReadWrite);
                    access = canMountHives ? "Yes" : "No (unprivileged)";
                }
                catch (UnauthorizedAccessException)
                {
                    access = "No";
                }
                catch (System.IO.FileNotFoundException)
                {
                    access = string.Empty;
                }
                catch (Exception)
                {
                    access = "No (in use)";
                }
                if (!string.IsNullOrEmpty(access))
                {
                    var lvi = SubfoldersListview.Items.Add(Path.GetFileName(folder));
                    _ = lvi.SubItems.Add(access);
                }
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (SubfoldersListview.SelectedItems.Count == 0)
            {
                return;
            }

            SelectedFilePath = Path.Combine(@"C:\Users", SubfoldersListview.SelectedItems[0].Text, "ntuser.dat");
            DialogResult = DialogResult.OK;
        }

        private void OpenUserRegistry_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}