using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace PolicyPlus.UI
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
                Interaction.MsgBox("Please enter a valid language code.", MsgBoxStyle.Exclamation);
                return;
            }
            if ((selection ?? "") == (_originalLanguage ?? ""))
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