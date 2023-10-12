using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ImportSpol : Form
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
            ButtonOpenFile = new Button();
            ButtonOpenFile.Click += new EventHandler(ButtonOpenFile_Click);
            Label1 = new Label();
            ButtonApply = new Button();
            ButtonApply.Click += new EventHandler(ButtonApply_Click);
            TextSpol = new TextBox();
            TextSpol.KeyDown += new KeyEventHandler(TextSpol_KeyDown);
            ButtonVerify = new Button();
            ButtonVerify.Click += new EventHandler(ButtonVerify_Click);
            ButtonReset = new Button();
            ButtonReset.Click += new EventHandler(ButtonReset_Click);
            SuspendLayout();
            // 
            // ButtonOpenFile
            // 
            ButtonOpenFile.Location = new Point(157, 12);
            ButtonOpenFile.Name = "ButtonOpenFile";
            ButtonOpenFile.Size = new Size(75, 23);
            ButtonOpenFile.TabIndex = 0;
            ButtonOpenFile.Text = "Open File";
            ButtonOpenFile.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 17);
            Label1.Name = "Label1";
            Label1.Size = new Size(139, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Semantic Policy (SPOL) text";
            // 
            // ButtonApply
            // 
            ButtonApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonApply.Location = new Point(303, 219);
            ButtonApply.Name = "ButtonApply";
            ButtonApply.Size = new Size(75, 23);
            ButtonApply.TabIndex = 4;
            ButtonApply.Text = "Apply";
            ButtonApply.UseVisualStyleBackColor = true;
            // 
            // TextSpol
            // 
            TextSpol.AcceptsReturn = true;
            TextSpol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            TextSpol.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextSpol.Location = new Point(12, 41);
            TextSpol.Multiline = true;
            TextSpol.Name = "TextSpol";
            TextSpol.ScrollBars = ScrollBars.Both;
            TextSpol.Size = new Size(366, 172);
            TextSpol.TabIndex = 2;
            TextSpol.Text = "Policy Plus Semantic Policy" + '\r' + '\n' + '\r' + '\n';
            TextSpol.WordWrap = false;
            // 
            // ButtonVerify
            // 
            ButtonVerify.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonVerify.Location = new Point(222, 219);
            ButtonVerify.Name = "ButtonVerify";
            ButtonVerify.Size = new Size(75, 23);
            ButtonVerify.TabIndex = 3;
            ButtonVerify.Text = "Verify";
            ButtonVerify.UseVisualStyleBackColor = true;
            // 
            // ButtonReset
            // 
            ButtonReset.Location = new Point(238, 12);
            ButtonReset.Name = "ButtonReset";
            ButtonReset.Size = new Size(75, 23);
            ButtonReset.TabIndex = 1;
            ButtonReset.Text = "Reset";
            ButtonReset.UseVisualStyleBackColor = true;
            // 
            // ImportSpol
            // 
            AcceptButton = ButtonApply;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 254);
            Controls.Add(ButtonReset);
            Controls.Add(ButtonVerify);
            Controls.Add(TextSpol);
            Controls.Add(ButtonApply);
            Controls.Add(Label1);
            Controls.Add(ButtonOpenFile);
            KeyPreview = true;
            MinimizeBox = false;
            MinimumSize = new Size(373, 266);
            Name = "ImportSpol";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import Semantic Policy";
            Shown += new EventHandler(ImportSpol_Shown);
            KeyUp += new KeyEventHandler(ImportSpol_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Button ButtonOpenFile;
        internal Label Label1;
        internal Button ButtonApply;
        internal TextBox TextSpol;
        internal Button ButtonVerify;
        internal Button ButtonReset;
    }
}