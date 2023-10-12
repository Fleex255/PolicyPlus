using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EditPolDelete : Form
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
            TextKey = new TextBox();
            OptPurge = new RadioButton();
            OptPurge.CheckedChanged += new EventHandler(ChoiceChanged);
            OptClearFirst = new RadioButton();
            OptClearFirst.CheckedChanged += new EventHandler(ChoiceChanged);
            OptDeleteOne = new RadioButton();
            OptDeleteOne.CheckedChanged += new EventHandler(ChoiceChanged);
            TextValueName = new TextBox();
            ButtonOK = new Button();
            ButtonOK.Click += new EventHandler(ButtonOK_Click);
            Label1 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(72, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Container key";
            // 
            // TextKey
            // 
            TextKey.Location = new Point(90, 12);
            TextKey.Name = "TextKey";
            TextKey.ReadOnly = true;
            TextKey.Size = new Size(179, 20);
            TextKey.TabIndex = 0;
            // 
            // OptPurge
            // 
            OptPurge.AutoSize = true;
            OptPurge.Location = new Point(15, 38);
            OptPurge.Name = "OptPurge";
            OptPurge.Size = new Size(189, 17);
            OptPurge.TabIndex = 2;
            OptPurge.TabStop = true;
            OptPurge.Text = "Delete all the values under the key";
            OptPurge.UseVisualStyleBackColor = true;
            // 
            // OptClearFirst
            // 
            OptClearFirst.AutoSize = true;
            OptClearFirst.Location = new Point(15, 61);
            OptClearFirst.Name = "OptClearFirst";
            OptClearFirst.Size = new Size(189, 17);
            OptClearFirst.TabIndex = 3;
            OptClearFirst.TabStop = true;
            OptClearFirst.Text = "Clear the key before adding values";
            OptClearFirst.UseVisualStyleBackColor = true;
            // 
            // OptDeleteOne
            // 
            OptDeleteOne.AutoSize = true;
            OptDeleteOne.Location = new Point(15, 84);
            OptDeleteOne.Name = "OptDeleteOne";
            OptDeleteOne.Size = new Size(107, 17);
            OptDeleteOne.TabIndex = 4;
            OptDeleteOne.TabStop = true;
            OptDeleteOne.Text = "Delete this value:";
            OptDeleteOne.UseVisualStyleBackColor = true;
            // 
            // TextValueName
            // 
            TextValueName.Enabled = false;
            TextValueName.Location = new Point(128, 83);
            TextValueName.Name = "TextValueName";
            TextValueName.Size = new Size(141, 20);
            TextValueName.TabIndex = 5;
            // 
            // ButtonOK
            // 
            ButtonOK.Location = new Point(194, 109);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 6;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // EditPolDelete
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(281, 144);
            Controls.Add(ButtonOK);
            Controls.Add(TextValueName);
            Controls.Add(OptDeleteOne);
            Controls.Add(OptClearFirst);
            Controls.Add(OptPurge);
            Controls.Add(Label1);
            Controls.Add(TextKey);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditPolDelete";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Delete Value(s)";
            KeyDown += new KeyEventHandler(EditPolDelete_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextKey;
        internal RadioButton OptPurge;
        internal RadioButton OptClearFirst;
        internal RadioButton OptDeleteOne;
        internal TextBox TextValueName;
        internal Button ButtonOK;
    }
}