using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class LanguageOptions : Form
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
            ButtonOK = new Button();
            ButtonOK.Click += new EventHandler(ButtonOK_Click);
            TextAdmlLanguage = new TextBox();
            Label1 = new Label();
            Label2 = new Label();
            SuspendLayout();
            // 
            // ButtonOK
            // 
            ButtonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonOK.Location = new Point(213, 90);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 0;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 9);
            Label1.MaximumSize = new Size(276, 0);
            Label1.Name = "Label1";
            Label1.Size = new Size(275, 52);
            Label1.TabIndex = 1;
            Label1.Text = "Each ADMX policy definitions file may have multiple corresponding ADML language-s" + "pecific files. This setting controls which language's ADML file Policy Plus will" + " look for first.";
            // 
            // TextAdmlLanguage
            // 
            TextAdmlLanguage.Location = new Point(175, 64);
            TextAdmlLanguage.Name = "TextAdmlLanguage";
            TextAdmlLanguage.Size = new Size(113, 20);
            TextAdmlLanguage.TabIndex = 2;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 67);
            Label2.Name = "Label2";
            Label2.Size = new Size(157, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Preferred ADML language code";
            // 
            // LanguageOptions
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 125);
            Controls.Add(Label2);
            Controls.Add(TextAdmlLanguage);
            Controls.Add(Label1);
            Controls.Add(ButtonOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LanguageOptions";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Language Options";
            ResumeLayout(false);
            PerformLayout();

        }

        internal Button ButtonOK;
        internal TextBox TextAdmlLanguage;
    }
}