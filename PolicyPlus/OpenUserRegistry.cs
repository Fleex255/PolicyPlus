using System;
using System.Windows.Forms;

namespace PolicyPlus
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
            bool canMountHives = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            foreach (var folder in System.IO.Directory.EnumerateDirectories(@"C:\Users"))
            {
                var dirInfo = new System.IO.DirectoryInfo(folder);
                if ((int)(dirInfo.Attributes & System.IO.FileAttributes.ReparsePoint) > 0)
                    continue;
                string ntuserPath = folder + @"\ntuser.dat";
                string access = "";
                try
                {
                    using (var fNtuser = new System.IO.FileStream(ntuserPath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                    {
                        access = canMountHives ? "Yes" : "No (unprivileged)";
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    access = "No";
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    access = "";
                }
                catch (Exception ex)
                {
                    access = "No (in use)";
                }
                if (!string.IsNullOrEmpty(access))
                {
                    var lvi = SubfoldersListview.Items.Add(System.IO.Path.GetFileName(folder));
                    lvi.SubItems.Add(access);
                }
            }
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (SubfoldersListview.SelectedItems.Count == 0)
                return;
            SelectedFilePath = System.IO.Path.Combine(@"C:\Users", SubfoldersListview.SelectedItems[0].Text, "ntuser.dat");
            DialogResult = DialogResult.OK;
        }
        private void OpenUserRegistry_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}