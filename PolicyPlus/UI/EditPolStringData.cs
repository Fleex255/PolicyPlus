using System.Windows.Forms;

namespace PolicyPlus.UI
{
    public partial class EditPolStringData
    {
        public EditPolStringData()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string valueName, string initialData)
        {
            TextName.Text = valueName;
            TextData.Text = initialData;
            TextData.Select();
            TextData.SelectAll();
            return ShowDialog();
        }

        private void EditPolStringData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}