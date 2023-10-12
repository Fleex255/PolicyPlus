using System;
using System.Windows.Forms;

namespace PolicyPlus
{
    public partial class EditPolNumericData
    {
        public EditPolNumericData()
        {
            InitializeComponent();
        }
        public DialogResult PresentDialog(string ValueName, ulong InitialData, bool IsQword)
        {
            TextName.Text = ValueName;
            NumData.Maximum = IsQword ? ulong.MaxValue : uint.MaxValue;
            NumData.Value = InitialData;
            NumData.Select();
            return ShowDialog();
        }
        private void CheckHexadecimal_CheckedChanged(object sender, EventArgs e)
        {
            NumData.Hexadecimal = CheckHexadecimal.Checked;
        }
        private void EditPolNumericData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
    // The normal NumericUpDown has a bug - it's unable to handle numbers greater than 0x7FFFFFF when in hex mode
    // This subclass fixes that bug
    // Adapted from https://social.msdn.microsoft.com/Forums/windows/en-US/6eea9c6c-a43c-4ef1-a7a3-de95e17e77a8/numericupdown-hexadecimal-bug?forum=winforms
    internal class WideRangeNumericUpDown : NumericUpDown
    {
        protected override void UpdateEditText()
        {
            if (Hexadecimal)
            {
                if (UserEdit)
                    HexParseEditText();
                if (!string.IsNullOrEmpty(Text))
                {
                    ChangingText = true;
                    Text = string.Format("{0:X}", (ulong)Math.Round(Value));
                }
            }
            else
            {
                base.UpdateEditText();
            }
        }
        protected override void ValidateEditText()
        {
            if (Hexadecimal)
            {
                HexParseEditText();
                UpdateEditText();
            }
            else
            {
                base.ValidateEditText();
            }
        }
        private void HexParseEditText()
        {
            try
            {
                if (!string.IsNullOrEmpty(Text))
                    Value = ulong.Parse(Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Value = Maximum;
            }
            catch (OverflowException ex)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    if (Text.StartsWith("-"))
                    {
                        Value = Minimum;
                    }
                    else
                    {
                        Value = Maximum;
                    }
                }
            }
            // Do nothing
            catch (Exception ex)
            {
            }
            finally
            {
                UserEdit = false;
            }
        }
    }
}