using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ImportReg : Form
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
            TextReg = new TextBox();
            TextRoot = new TextBox();
            ButtonBrowse = new Button();
            ButtonBrowse.Click += new EventHandler(ButtonBrowse_Click);
            ButtonImport = new Button();
            ButtonImport.Click += new EventHandler(ButtonImport_Click);
            Label1 = new Label();
            Label2 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(46, 13);
            Label1.TabIndex = 0;
            Label1.Text = "REG file";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 41);
            Label2.Name = "Label2";
            Label2.Size = new Size(33, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Prefix";
            // 
            // TextReg
            // 
            TextReg.Location = new Point(64, 12);
            TextReg.Name = "TextReg";
            TextReg.Size = new Size(195, 20);
            TextReg.TabIndex = 1;
            // 
            // TextRoot
            // 
            TextRoot.Location = new Point(64, 38);
            TextRoot.Name = "TextRoot";
            TextRoot.Size = new Size(276, 20);
            TextRoot.TabIndex = 3;
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Location = new Point(265, 10);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new Size(75, 23);
            ButtonBrowse.TabIndex = 2;
            ButtonBrowse.Text = "Browse";
            ButtonBrowse.UseVisualStyleBackColor = true;
            // 
            // ButtonImport
            // 
            ButtonImport.Location = new Point(265, 64);
            ButtonImport.Name = "ButtonImport";
            ButtonImport.Size = new Size(75, 23);
            ButtonImport.TabIndex = 4;
            ButtonImport.Text = "Import";
            ButtonImport.UseVisualStyleBackColor = true;
            // 
            // ImportReg
            // 
            AcceptButton = ButtonImport;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(352, 99);
            Controls.Add(ButtonImport);
            Controls.Add(ButtonBrowse);
            Controls.Add(Label2);
            Controls.Add(TextRoot);
            Controls.Add(TextReg);
            Controls.Add(Label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImportReg";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import REG";
            KeyUp += new KeyEventHandler(ImportReg_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextReg;
        internal TextBox TextRoot;
        internal Button ButtonBrowse;
        internal Button ButtonImport;
    }
}