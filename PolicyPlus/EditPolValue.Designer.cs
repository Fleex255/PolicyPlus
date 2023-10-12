using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EditPolValue : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            Label Label1;
            Label Label2;
            ComboKind = new ComboBox();
            ButtonOK = new Button();
            TextName = new TextBox();
            Label1 = new Label();
            Label2 = new Label();
            SuspendLayout();
            // 
            // ComboKind
            // 
            ComboKind.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboKind.FormattingEnabled = true;
            ComboKind.Items.AddRange(new object[] { "String", "Expandable string", "List of strings", "32-bit DWord", "64-bit QWord" });
            ComboKind.Location = new Point(115, 38);
            ComboKind.Name = "ComboKind";
            ComboKind.Size = new Size(162, 21);
            ComboKind.TabIndex = 2;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 41);
            Label1.Name = "Label1";
            Label1.Size = new Size(97, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Registry value type";
            // 
            // ButtonOK
            // 
            ButtonOK.DialogResult = DialogResult.OK;
            ButtonOK.Location = new Point(202, 65);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 3;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // TextName
            // 
            TextName.Location = new Point(115, 12);
            TextName.Name = "TextName";
            TextName.Size = new Size(162, 20);
            TextName.TabIndex = 1;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 15);
            Label2.Name = "Label2";
            Label2.Size = new Size(63, 13);
            Label2.TabIndex = 4;
            Label2.Text = "Value name";
            // 
            // EditPolValue
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(289, 100);
            Controls.Add(Label2);
            Controls.Add(TextName);
            Controls.Add(ButtonOK);
            Controls.Add(Label1);
            Controls.Add(ComboKind);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditPolValue";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Value";
            KeyDown += new KeyEventHandler(EditPolValueType_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ComboBox ComboKind;
        internal Button ButtonOK;
        internal TextBox TextName;
    }
}