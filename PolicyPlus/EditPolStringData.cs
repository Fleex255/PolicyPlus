using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class EditPolStringData
    {
        public EditPolStringData()
        {
            InitializeComponent();
        }
        public DialogResult PresentDialog(string ValueName, string InitialData)
        {
            TextName.Text = ValueName;
            TextData.Text = InitialData;
            TextData.Select();
            TextData.SelectAll();
            return ShowDialog();
        }
        private void EditPolStringData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}