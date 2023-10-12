using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus
{
    public partial class ExportReg
    {
        private PolFile Source;

        public ExportReg()
        {
            InitializeComponent();
        }
        public DialogResult PresentDialog(string Branch, PolFile Pol, bool IsUser)
        {
            Source = Pol;
            TextBranch.Text = Branch;
            TextRoot.Text = IsUser ? @"HKEY_CURRENT_USER\" : @"HKEY_LOCAL_MACHINE\";
            TextReg.Text = "";
            return ShowDialog();
        }
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Registry scripts|*.reg";
                if (sfd.ShowDialog() == DialogResult.OK)
                    TextReg.Text = sfd.FileName;
            }
        }
        private void ExportReg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
        private void ButtonExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextReg.Text))
            {
                Interaction.MsgBox("Please specify a filename and path for the exported REG.", MsgBoxStyle.Exclamation);
                return;
            }
            var reg = new RegFile();
            reg.SetPrefix(TextRoot.Text);
            reg.SetSourceBranch(TextBranch.Text);
            try
            {
                Source.Apply(reg);
                reg.Save(TextReg.Text);
                Interaction.MsgBox("REG exported successfully.", MsgBoxStyle.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Failed to export REG!", MsgBoxStyle.Exclamation);
            }
        }
    }
}