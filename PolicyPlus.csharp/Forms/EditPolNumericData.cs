using System;
using System.Windows.Forms;

namespace PolicyPlus.csharp.UI
{
    public partial class EditPolNumericData
    {
        public EditPolNumericData()
        {
            InitializeComponent();
        }

        public DialogResult PresentDialog(string valueName, ulong initialData, bool isQword)
        {
            TextName.Text = valueName;
            NumData.Maximum = isQword ? ulong.MaxValue : uint.MaxValue;
            NumData.Value = initialData;
            NumData.Select();
            return ShowDialog();
        }

        private void CheckHexadecimal_CheckedChanged(object sender, EventArgs e) => NumData.Hexadecimal = CheckHexadecimal.Checked;

        private void EditPolNumericData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }

    // The normal NumericUpDown has a b*g - it's unable to handle numbers greater than 0x7FFFFFF
    // when in hex mode This subclass fixes that b*g Adapted from https://social.msdn.microsoft.com/Forums/windows/en-US/6eea9c6c-a43c-4ef1-a7a3-de95e17e77a8/numericupdown-hexadecimal-bug?forum=winforms
}