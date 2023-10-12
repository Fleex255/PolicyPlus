using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DownloadAdmx : Form
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
            Label LabelWhatsThis;
            Label LabelDestFolder;
            TextDestFolder = new TextBox();
            ButtonBrowse = new Button();
            ButtonBrowse.Click += new EventHandler(ButtonBrowse_Click);
            ProgressSpinner = new ProgressBar();
            LabelProgress = new Label();
            ButtonStart = new Button();
            ButtonStart.Click += new EventHandler(ButtonStart_Click);
            ButtonClose = new Button();
            LabelWhatsThis = new Label();
            LabelDestFolder = new Label();
            SuspendLayout();
            // 
            // LabelWhatsThis
            // 
            LabelWhatsThis.AutoSize = true;
            LabelWhatsThis.Location = new Point(12, 9);
            LabelWhatsThis.Name = "LabelWhatsThis";
            LabelWhatsThis.Size = new Size(331, 13);
            LabelWhatsThis.TabIndex = 0;
            LabelWhatsThis.Text = "Download the full set of policy definitions (ADMX files) from Microsoft.";
            // 
            // LabelDestFolder
            // 
            LabelDestFolder.AutoSize = true;
            LabelDestFolder.Location = new Point(12, 30);
            LabelDestFolder.Name = "LabelDestFolder";
            LabelDestFolder.Size = new Size(89, 13);
            LabelDestFolder.TabIndex = 3;
            LabelDestFolder.Text = "Destination folder";
            // 
            // TextDestFolder
            // 
            TextDestFolder.Location = new Point(107, 27);
            TextDestFolder.Name = "TextDestFolder";
            TextDestFolder.Size = new Size(184, 20);
            TextDestFolder.TabIndex = 1;
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Location = new Point(297, 25);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new Size(75, 23);
            ButtonBrowse.TabIndex = 2;
            ButtonBrowse.Text = "Browse";
            ButtonBrowse.UseVisualStyleBackColor = true;
            // 
            // ProgressSpinner
            // 
            ProgressSpinner.Location = new Point(12, 53);
            ProgressSpinner.Name = "ProgressSpinner";
            ProgressSpinner.Size = new Size(360, 23);
            ProgressSpinner.Style = ProgressBarStyle.Marquee;
            ProgressSpinner.TabIndex = 4;
            // 
            // LabelProgress
            // 
            LabelProgress.AutoSize = true;
            LabelProgress.Location = new Point(12, 87);
            LabelProgress.Name = "LabelProgress";
            LabelProgress.Size = new Size(48, 13);
            LabelProgress.TabIndex = 5;
            LabelProgress.Text = "Progress";
            LabelProgress.Visible = false;
            // 
            // ButtonStart
            // 
            ButtonStart.Location = new Point(297, 82);
            ButtonStart.Name = "ButtonStart";
            ButtonStart.Size = new Size(75, 23);
            ButtonStart.TabIndex = 6;
            ButtonStart.Text = "Begin";
            ButtonStart.UseVisualStyleBackColor = true;
            // 
            // ButtonClose
            // 
            ButtonClose.DialogResult = DialogResult.Cancel;
            ButtonClose.Location = new Point(216, 82);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 7;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // DownloadAdmx
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 117);
            Controls.Add(ButtonClose);
            Controls.Add(ButtonStart);
            Controls.Add(LabelProgress);
            Controls.Add(ProgressSpinner);
            Controls.Add(LabelDestFolder);
            Controls.Add(ButtonBrowse);
            Controls.Add(TextDestFolder);
            Controls.Add(LabelWhatsThis);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DownloadAdmx";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Acquire ADMX Files";
            Closing += new System.ComponentModel.CancelEventHandler(DownloadAdmx_Closing);
            Shown += new EventHandler(DownloadAdmx_Shown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextDestFolder;
        internal Button ButtonBrowse;
        internal ProgressBar ProgressSpinner;
        internal Label LabelProgress;
        internal Button ButtonStart;
        internal Button ButtonClose;
    }
}