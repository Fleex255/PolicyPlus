using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class InspectSpolFragment : Form
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
            TextPolicyName = new TextBox();
            LabelPolicy = new Label();
            TextSpol = new TextBox();
            TextSpol.KeyDown += new KeyEventHandler(TextSpol_KeyDown);
            ButtonClose = new Button();
            ButtonCopy = new Button();
            ButtonCopy.Click += new EventHandler(ButtonCopy_Click);
            CheckHeader = new CheckBox();
            CheckHeader.CheckedChanged += new EventHandler(CheckHeader_CheckedChanged);
            SuspendLayout();
            // 
            // TextPolicyName
            // 
            TextPolicyName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TextPolicyName.Location = new Point(53, 12);
            TextPolicyName.Name = "TextPolicyName";
            TextPolicyName.ReadOnly = true;
            TextPolicyName.Size = new Size(266, 20);
            TextPolicyName.TabIndex = 0;
            // 
            // LabelPolicy
            // 
            LabelPolicy.AutoSize = true;
            LabelPolicy.Location = new Point(12, 15);
            LabelPolicy.Name = "LabelPolicy";
            LabelPolicy.Size = new Size(35, 13);
            LabelPolicy.TabIndex = 1;
            LabelPolicy.Text = "Policy";
            // 
            // TextSpol
            // 
            TextSpol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            TextSpol.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextSpol.Location = new Point(12, 38);
            TextSpol.Multiline = true;
            TextSpol.Name = "TextSpol";
            TextSpol.ReadOnly = true;
            TextSpol.ScrollBars = ScrollBars.Both;
            TextSpol.Size = new Size(307, 172);
            TextSpol.TabIndex = 1;
            TextSpol.WordWrap = false;
            // 
            // ButtonClose
            // 
            ButtonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonClose.DialogResult = DialogResult.OK;
            ButtonClose.Location = new Point(244, 216);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 4;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // ButtonCopy
            // 
            ButtonCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonCopy.Location = new Point(163, 216);
            ButtonCopy.Name = "ButtonCopy";
            ButtonCopy.Size = new Size(75, 23);
            ButtonCopy.TabIndex = 3;
            ButtonCopy.Text = "Copy";
            ButtonCopy.UseVisualStyleBackColor = true;
            // 
            // CheckHeader
            // 
            CheckHeader.AutoSize = true;
            CheckHeader.Location = new Point(12, 220);
            CheckHeader.Name = "CheckHeader";
            CheckHeader.Size = new Size(128, 17);
            CheckHeader.TabIndex = 2;
            CheckHeader.Text = "Include SPOL header";
            CheckHeader.UseVisualStyleBackColor = true;
            // 
            // InspectSpolFragment
            // 
            AcceptButton = ButtonClose;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonClose;
            ClientSize = new Size(331, 251);
            Controls.Add(CheckHeader);
            Controls.Add(ButtonCopy);
            Controls.Add(ButtonClose);
            Controls.Add(TextSpol);
            Controls.Add(LabelPolicy);
            Controls.Add(TextPolicyName);
            MinimizeBox = false;
            MinimumSize = new Size(347, 290);
            Name = "InspectSpolFragment";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Semantic Policy Fragment";
            Shown += new EventHandler(InspectSpolFragment_Shown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextPolicyName;
        internal Label LabelPolicy;
        internal TextBox TextSpol;
        internal Button ButtonClose;
        internal Button ButtonCopy;
        internal CheckBox CheckHeader;
    }
}