using System;
using System.Windows.Forms;
using PolicyPlus.csharp.Models.Sources;

namespace PolicyPlus.csharp.UI
{
    public partial class ImportReg
    {
        private IPolicySource _policySource;

        public ImportReg()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(IPolicySource target)
        {
            TextReg.Text = "";
            TextRoot.Text = "";
            _policySource = target;
            return ShowDialog();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Registry scripts|*.reg";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            TextReg.Text = ofd.FileName;
            if (string.IsNullOrEmpty(TextRoot.Text))
            {
                try
                {
                    var reg = RegFile.Load(ofd.FileName, "");
                    TextRoot.Text = reg.GuessPrefix();
                    if (reg.HasDefaultValues())
                    {
                        _ = MessageBox.Show("This REG file contains data for default values, which cannot be applied to all policy sources.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception)
                {
                    _ = MessageBox.Show("An error occurred while trying to guess the prefix.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void ImportReg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextReg.Text))
            {
                _ = MessageBox.Show("Please specify a REG file to import.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(TextRoot.Text))
            {
                _ = MessageBox.Show("Please specify the prefix used to fully qualify paths in the REG file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                var reg = RegFile.Load(TextReg.Text, TextRoot.Text);
                reg.Apply(_policySource);
                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Failed to import the REG file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}