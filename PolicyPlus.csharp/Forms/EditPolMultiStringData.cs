using System.Windows.Forms;

namespace PolicyPlus.csharp.UI
{
    public partial class EditPolMultiStringData
    {
        public EditPolMultiStringData()
        {
            InitializeComponent();
        }

        private void EditPolMultiStringData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        public DialogResult PresentDialog(string valueName, string[] initialData)
        {
            TextName.Text = valueName;
            TextData.Lines = initialData;
            TextData.Select();
            return ShowDialog();
        }
    }
}