using System;
using System.Windows.Forms;


namespace PolicyPlus.csharp.UI
{
    public partial class LanguageOptions
    {
        private string _originalLanguage;
        public string NewLanguage;

        public LanguageOptions()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string currentLanguage)
        {
            _originalLanguage = currentLanguage;
            TextAdmlLanguage.Text = currentLanguage;
            return ShowDialog();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            var selection = TextAdmlLanguage.Text.Trim();
            if (selection.Split('-').Length != 2)
            {
                _ = MessageBox.Show("Please enter a valid language code.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if ((selection ?? string.Empty) == (_originalLanguage ?? string.Empty))
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                NewLanguage = selection;
                DialogResult = DialogResult.OK;
            }
        }
    }
}