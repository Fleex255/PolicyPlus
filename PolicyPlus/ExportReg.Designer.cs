using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ExportReg : Form
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
            Label Label3;
            Label Label4;
            TextReg = new TextBox();
            TextBranch = new TextBox();
            TextRoot = new TextBox();
            ButtonBrowse = new Button();
            ButtonBrowse.Click += new EventHandler(ButtonBrowse_Click);
            ButtonExport = new Button();
            ButtonExport.Click += new EventHandler(ButtonExport_Click);
            Label1 = new Label();
            Label2 = new Label();
            Label3 = new Label();
            Label4 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(77, 13);
            Label1.TabIndex = 0;
            Label1.Text = "Source branch";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 67);
            Label2.Name = "Label2";
            Label2.Size = new Size(73, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Registry prefix";
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(12, 41);
            Label3.Name = "Label3";
            Label3.Size = new Size(46, 13);
            Label3.TabIndex = 4;
            Label3.Text = "REG file";
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(275, 15);
            Label4.Name = "Label4";
            Label4.Size = new Size(96, 13);
            Label4.TabIndex = 8;
            Label4.Text = "(blank to export all)";
            // 
            // TextReg
            // 
            TextReg.Location = new Point(95, 38);
            TextReg.Name = "TextReg";
            TextReg.Size = new Size(195, 20);
            TextReg.TabIndex = 2;
            // 
            // TextBranch
            // 
            TextBranch.Location = new Point(95, 12);
            TextBranch.Name = "TextBranch";
            TextBranch.Size = new Size(174, 20);
            TextBranch.TabIndex = 1;
            // 
            // TextRoot
            // 
            TextRoot.Location = new Point(95, 64);
            TextRoot.Name = "TextRoot";
            TextRoot.Size = new Size(276, 20);
            TextRoot.TabIndex = 4;
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Location = new Point(296, 36);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new Size(75, 23);
            ButtonBrowse.TabIndex = 3;
            ButtonBrowse.Text = "Browse";
            ButtonBrowse.UseVisualStyleBackColor = true;
            // 
            // ButtonExport
            // 
            ButtonExport.Location = new Point(296, 90);
            ButtonExport.Name = "ButtonExport";
            ButtonExport.Size = new Size(75, 23);
            ButtonExport.TabIndex = 5;
            ButtonExport.Text = "Export";
            ButtonExport.UseVisualStyleBackColor = true;
            // 
            // ExportReg
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(383, 125);
            Controls.Add(Label4);
            Controls.Add(ButtonExport);
            Controls.Add(ButtonBrowse);
            Controls.Add(TextRoot);
            Controls.Add(Label3);
            Controls.Add(Label2);
            Controls.Add(TextBranch);
            Controls.Add(TextReg);
            Controls.Add(Label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ExportReg";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Export REG";
            KeyUp += new KeyEventHandler(ExportReg_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextReg;
        internal TextBox TextBranch;
        internal TextBox TextRoot;
        internal Button ButtonBrowse;
        internal Button ButtonExport;
    }
}