using System;
using System.Windows.Forms;

using PolicyPlus.csharp.Models;
using PolicyPlus.csharp.Models.Sources;

namespace PolicyPlus.csharp.UI
{
    public partial class ExportReg
    {
        private PolFile _source;

        public ExportReg()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string branch, PolFile pol, bool isUser)
        {
            _source = pol;
            TextBranch.Text = branch;
            TextRoot.Text = isUser ? @"HKEY_CURRENT_USER\" : @"HKEY_LOCAL_MACHINE\";
            TextReg.Text = "";
            return ShowDialog(this);
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "Registry scripts|*.reg";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                TextReg.Text = sfd.FileName;
            }
        }

        private void ExportReg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextReg.Text))
            {
                _ = MessageBox.Show("Please specify a filename and path for the exported REG.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var reg = new RegFile();
            reg.SetPrefix(TextRoot.Text);
            reg.SetSourceBranch(TextBranch.Text);
            try
            {
                _source.Apply(reg);
                reg.Save(TextReg.Text);
                _ = MessageBox.Show("REG exported successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Failed to export REG!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}