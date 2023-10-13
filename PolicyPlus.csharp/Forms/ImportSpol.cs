using System;
using System.IO;
using System.Windows.Forms;
using PolicyPlus.csharp.Models.Sources;

namespace PolicyPlus.csharp.UI
{
    public partial class ImportSpol
    {
        public SpolFile? Spol;

        public ImportSpol()
        {
            InitializeComponent();
        }

        private void ButtonOpenFile_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Semantic Policy files|*.spol|All files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TextSpol.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void ButtonVerify_Click(object sender, EventArgs e)
        {
            try
            {
                var spol = SpolFile.FromText(TextSpol.Text);
                _ = MessageBox.Show("Validation successful, " + spol.Policies.Count + " policy settings found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("SPOL validation failed: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonApply_Click(object sender, EventArgs e)
        {
            try
            {
                Spol = SpolFile.FromText(TextSpol.Text); // Tell the main form that the SPOL is ready to be committed
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"The SPOL text is invalid: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ImportSpol_Shown(object sender, EventArgs e) => Spol = null;

        private void ImportSpol_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && !(TextSpol.Focused && TextSpol.SelectionLength > 0))
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void TextSpol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                TextSpol.SelectAll();
            }
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the text box?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                TextSpol.Text = "Policy Plus Semantic Policy" + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}