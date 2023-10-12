using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EditPolNumericData : Form
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
            TextName = new TextBox();
            CheckHexadecimal = new CheckBox();
            CheckHexadecimal.CheckedChanged += new EventHandler(CheckHexadecimal_CheckedChanged);
            NumData = new WideRangeNumericUpDown();
            ButtonOK = new Button();
            Label1 = new Label();
            Label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)NumData).BeginInit();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(63, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Value name";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 41);
            Label2.Name = "Label2";
            Label2.Size = new Size(44, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Number";
            // 
            // TextName
            // 
            TextName.Location = new Point(81, 12);
            TextName.Name = "TextName";
            TextName.ReadOnly = true;
            TextName.Size = new Size(230, 20);
            TextName.TabIndex = 0;
            // 
            // CheckHexadecimal
            // 
            CheckHexadecimal.AutoSize = true;
            CheckHexadecimal.Location = new Point(224, 40);
            CheckHexadecimal.Name = "CheckHexadecimal";
            CheckHexadecimal.Size = new Size(87, 17);
            CheckHexadecimal.TabIndex = 2;
            CheckHexadecimal.Text = "Hexadecimal";
            CheckHexadecimal.UseVisualStyleBackColor = true;
            // 
            // NumData
            // 
            NumData.Location = new Point(81, 37);
            NumData.Name = "NumData";
            NumData.Size = new Size(137, 20);
            NumData.TabIndex = 1;
            // 
            // ButtonOK
            // 
            ButtonOK.DialogResult = DialogResult.OK;
            ButtonOK.Location = new Point(236, 63);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 3;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // EditPolNumericData
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(323, 98);
            Controls.Add(ButtonOK);
            Controls.Add(NumData);
            Controls.Add(CheckHexadecimal);
            Controls.Add(Label2);
            Controls.Add(Label1);
            Controls.Add(TextName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditPolNumericData";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Number";
            ((System.ComponentModel.ISupportInitialize)NumData).EndInit();
            KeyDown += new KeyEventHandler(EditPolNumericData_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextName;
        internal CheckBox CheckHexadecimal;
        internal Button ButtonOK;
        internal WideRangeNumericUpDown NumData;
    }
}