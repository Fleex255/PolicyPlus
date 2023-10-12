using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class OpenAdmxFolder : Form
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
            Label LabelFromWhere;
            OptLocalFolder = new RadioButton();
            OptLocalFolder.CheckedChanged += new EventHandler(Options_CheckedChanged);
            OptSysvol = new RadioButton();
            OptSysvol.CheckedChanged += new EventHandler(Options_CheckedChanged);
            OptCustomFolder = new RadioButton();
            OptCustomFolder.CheckedChanged += new EventHandler(Options_CheckedChanged);
            TextFolder = new TextBox();
            ButtonOK = new Button();
            ButtonOK.Click += new EventHandler(ButtonOK_Click);
            ButtonBrowse = new Button();
            ButtonBrowse.Click += new EventHandler(ButtonBrowse_Click);
            ClearWorkspaceCheckbox = new CheckBox();
            LabelFromWhere = new Label();
            SuspendLayout();
            // 
            // LabelFromWhere
            // 
            LabelFromWhere.AutoSize = true;
            LabelFromWhere.Location = new Point(12, 9);
            LabelFromWhere.Name = "LabelFromWhere";
            LabelFromWhere.Size = new Size(228, 13);
            LabelFromWhere.TabIndex = 0;
            LabelFromWhere.Text = "Where would you like to load ADMX files from?";
            // 
            // OptLocalFolder
            // 
            OptLocalFolder.AutoSize = true;
            OptLocalFolder.Location = new Point(15, 25);
            OptLocalFolder.Name = "OptLocalFolder";
            OptLocalFolder.Size = new Size(196, 17);
            OptLocalFolder.TabIndex = 1;
            OptLocalFolder.TabStop = true;
            OptLocalFolder.Text = "This system's PolicyDefinitions folder";
            OptLocalFolder.UseVisualStyleBackColor = true;
            // 
            // OptSysvol
            // 
            OptSysvol.AutoSize = true;
            OptSysvol.Location = new Point(15, 48);
            OptSysvol.Name = "OptSysvol";
            OptSysvol.Size = new Size(133, 17);
            OptSysvol.TabIndex = 2;
            OptSysvol.TabStop = true;
            OptSysvol.Text = "The domain's SYSVOL";
            OptSysvol.UseVisualStyleBackColor = true;
            // 
            // OptCustomFolder
            // 
            OptCustomFolder.AutoSize = true;
            OptCustomFolder.Location = new Point(15, 71);
            OptCustomFolder.Name = "OptCustomFolder";
            OptCustomFolder.Size = new Size(77, 17);
            OptCustomFolder.TabIndex = 3;
            OptCustomFolder.TabStop = true;
            OptCustomFolder.Text = "This folder:";
            OptCustomFolder.UseVisualStyleBackColor = true;
            // 
            // TextFolder
            // 
            TextFolder.Location = new Point(98, 70);
            TextFolder.Name = "TextFolder";
            TextFolder.Size = new Size(265, 20);
            TextFolder.TabIndex = 4;
            // 
            // ButtonOK
            // 
            ButtonOK.Location = new Point(354, 96);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 7;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Location = new Point(369, 68);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new Size(60, 23);
            ButtonBrowse.TabIndex = 5;
            ButtonBrowse.Text = "Browse";
            ButtonBrowse.UseVisualStyleBackColor = true;
            // 
            // ClearWorkspaceCheckbox
            // 
            ClearWorkspaceCheckbox.AutoSize = true;
            ClearWorkspaceCheckbox.Checked = true;
            ClearWorkspaceCheckbox.CheckState = CheckState.Checked;
            ClearWorkspaceCheckbox.Location = new Point(15, 100);
            ClearWorkspaceCheckbox.Name = "ClearWorkspaceCheckbox";
            ClearWorkspaceCheckbox.Size = new Size(239, 17);
            ClearWorkspaceCheckbox.TabIndex = 6;
            ClearWorkspaceCheckbox.Text = "Clear the workspace before adding this folder";
            ClearWorkspaceCheckbox.UseVisualStyleBackColor = true;
            // 
            // OpenAdmxFolder
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(441, 131);
            Controls.Add(ClearWorkspaceCheckbox);
            Controls.Add(ButtonBrowse);
            Controls.Add(ButtonOK);
            Controls.Add(TextFolder);
            Controls.Add(OptCustomFolder);
            Controls.Add(OptSysvol);
            Controls.Add(OptLocalFolder);
            Controls.Add(LabelFromWhere);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenAdmxFolder";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Open ADMX Folder";
            Shown += new EventHandler(OpenAdmxFolder_Shown);
            KeyUp += new KeyEventHandler(OpenAdmxFolder_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal RadioButton OptLocalFolder;
        internal RadioButton OptSysvol;
        internal RadioButton OptCustomFolder;
        internal TextBox TextFolder;
        internal Button ButtonOK;
        internal Button ButtonBrowse;
        internal CheckBox ClearWorkspaceCheckbox;
    }
}