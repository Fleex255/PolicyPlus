using System;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class EditPolKey
    {
        public EditPolKey()
        {
            InitializeComponent();
        }
        private void EditPolKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
        private void EditPolKey_Shown(object sender, EventArgs e)
        {
            TextName.Select();
            TextName.SelectAll();
        }
        public string PresentDialog(string InitialName)
        {
            TextName.Text = InitialName;
            if (ShowDialog() == DialogResult.OK)
            {
                return TextName.Text;
            }
            else
            {
                return "";
            }
        }
    }
}