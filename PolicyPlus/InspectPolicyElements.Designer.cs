using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class InspectPolicyElements : Form
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
            Label PolicyNameLabel;
            PolicyNameTextbox = new TextBox();
            PolicyDetailsButton = new Button();
            PolicyDetailsButton.Click += new EventHandler(PolicyDetailsButton_Click);
            InfoTreeview = new TreeView();
            InfoTreeview.KeyDown += new KeyEventHandler(InfoTreeview_KeyDown);
            CloseButton = new Button();
            PolicyNameLabel = new Label();
            SuspendLayout();
            // 
            // PolicyNameTextbox
            // 
            PolicyNameTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PolicyNameTextbox.Location = new Point(53, 12);
            PolicyNameTextbox.Name = "PolicyNameTextbox";
            PolicyNameTextbox.ReadOnly = true;
            PolicyNameTextbox.Size = new Size(248, 20);
            PolicyNameTextbox.TabIndex = 0;
            // 
            // PolicyDetailsButton
            // 
            PolicyDetailsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PolicyDetailsButton.Location = new Point(307, 10);
            PolicyDetailsButton.Name = "PolicyDetailsButton";
            PolicyDetailsButton.Size = new Size(75, 23);
            PolicyDetailsButton.TabIndex = 1;
            PolicyDetailsButton.Text = "Details";
            PolicyDetailsButton.UseVisualStyleBackColor = true;
            // 
            // PolicyNameLabel
            // 
            PolicyNameLabel.AutoSize = true;
            PolicyNameLabel.Location = new Point(12, 15);
            PolicyNameLabel.Name = "PolicyNameLabel";
            PolicyNameLabel.Size = new Size(35, 13);
            PolicyNameLabel.TabIndex = 2;
            PolicyNameLabel.Text = "Policy";
            // 
            // InfoTreeview
            // 
            InfoTreeview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            InfoTreeview.HideSelection = false;
            InfoTreeview.Location = new Point(15, 38);
            InfoTreeview.Name = "InfoTreeview";
            InfoTreeview.ShowNodeToolTips = true;
            InfoTreeview.Size = new Size(367, 193);
            InfoTreeview.TabIndex = 3;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.DialogResult = DialogResult.Cancel;
            CloseButton.Location = new Point(307, 237);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 4;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // InspectPolicyElements
            // 
            AcceptButton = CloseButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(394, 272);
            Controls.Add(CloseButton);
            Controls.Add(InfoTreeview);
            Controls.Add(PolicyNameLabel);
            Controls.Add(PolicyDetailsButton);
            Controls.Add(PolicyNameTextbox);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(305, 219);
            Name = "InspectPolicyElements";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Element Inspector";
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox PolicyNameTextbox;
        internal Button PolicyDetailsButton;
        internal TreeView InfoTreeview;
        internal Button CloseButton;
    }
}