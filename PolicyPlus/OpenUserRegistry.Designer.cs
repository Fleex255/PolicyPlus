using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class OpenUserRegistry : Form
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
            SubfoldersListview = new ListView();
            ChUsername = new ColumnHeader();
            ChAccess = new ColumnHeader();
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            SuspendLayout();
            // 
            // SubfoldersListview
            // 
            SubfoldersListview.Columns.AddRange(new ColumnHeader[] { ChUsername, ChAccess });
            SubfoldersListview.FullRowSelect = true;
            SubfoldersListview.Location = new Point(12, 12);
            SubfoldersListview.MultiSelect = false;
            SubfoldersListview.Name = "SubfoldersListview";
            SubfoldersListview.Size = new Size(314, 111);
            SubfoldersListview.TabIndex = 0;
            SubfoldersListview.UseCompatibleStateImageBehavior = false;
            SubfoldersListview.View = View.Details;
            // 
            // ChUsername
            // 
            ChUsername.Text = "Folder Name";
            ChUsername.Width = 196;
            // 
            // ChAccess
            // 
            ChAccess.Text = "Accessible";
            ChAccess.Width = 95;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(251, 129);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 1;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // OpenUserRegistry
            // 
            AcceptButton = OkButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(338, 164);
            Controls.Add(OkButton);
            Controls.Add(SubfoldersListview);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenUserRegistry";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Open User Hive";
            Shown += new EventHandler(OpenUserRegistry_Shown);
            KeyUp += new KeyEventHandler(OpenUserRegistry_KeyUp);
            ResumeLayout(false);

        }

        internal ListView SubfoldersListview;
        internal ColumnHeader ChUsername;
        internal ColumnHeader ChAccess;
        internal Button OkButton;
    }
}