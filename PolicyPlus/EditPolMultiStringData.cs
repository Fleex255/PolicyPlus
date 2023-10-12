using System.Windows.Forms;

namespace PolicyPlus
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
                DialogResult = DialogResult.Cancel;
        }
        public DialogResult PresentDialog(string ValueName, string[] InitialData)
        {
            TextName.Text = ValueName;
            TextData.Lines = InitialData;
            TextData.Select();
            return ShowDialog();
        }
    }
}