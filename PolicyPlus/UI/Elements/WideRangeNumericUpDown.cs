using System;
using System.Windows.Forms;

namespace PolicyPlus.UI.Elements
{
    internal class WideRangeNumericUpDown : NumericUpDown
    {
        protected override void UpdateEditText()
        {
            if (Hexadecimal)
            {
                if (UserEdit)
                {
                    HexParseEditText();
                }

                if (string.IsNullOrEmpty(Text))
                {
                    return;
                }

                ChangingText = true;
                Text = $"{(ulong)Math.Round(Value):X}";
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
                {
                    Value = ulong.Parse(Text, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Value = Maximum;
            }
            catch (OverflowException)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    Value = Text.StartsWith("-") ? Minimum : Maximum;
                }
            }
            // Do nothing
            catch
            {
                // ignored
            }
            finally
            {
                UserEdit = false;
            }
        }
    }
}