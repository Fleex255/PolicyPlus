using System.Windows.Forms;
using Microsoft.Win32;

namespace PolicyPlus.csharp.UI
{
    public partial class EditPolValue
    {
        public RegistryValueKind SelectedKind;
        public string ChosenName;

        public EditPolValue()
        {
            InitializeComponent();
        }

        private void EditPolValueType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        public DialogResult PresentDialog()
        {
            TextName.Text = string.Empty;
            ComboKind.SelectedIndex = 0;
            var dlgRes = ShowDialog();
            if (dlgRes == DialogResult.OK)
            {
                SelectedKind = ComboKind.SelectedIndex switch
                {
                    0 => RegistryValueKind.String,
                    1 => RegistryValueKind.ExpandString,
                    2 => RegistryValueKind.MultiString,
                    3 => RegistryValueKind.DWord,
                    4 => RegistryValueKind.QWord,
                    _ => SelectedKind
                };
                ChosenName = TextName.Text;
            }
            return dlgRes;
        }
    }
}