using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FindByText : Form
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
            StringTextbox = new TextBox();
            TitleCheckbox = new CheckBox();
            DescriptionCheckbox = new CheckBox();
            CommentCheckbox = new CheckBox();
            SearchButton = new Button();
            SearchButton.Click += new EventHandler(SearchButton_Click);
            SuspendLayout();
            // 
            // StringTextbox
            // 
            StringTextbox.Location = new Point(12, 12);
            StringTextbox.Name = "StringTextbox";
            StringTextbox.Size = new Size(352, 20);
            StringTextbox.TabIndex = 0;
            // 
            // TitleCheckbox
            // 
            TitleCheckbox.AutoSize = true;
            TitleCheckbox.Checked = true;
            TitleCheckbox.CheckState = CheckState.Checked;
            TitleCheckbox.Location = new Point(12, 38);
            TitleCheckbox.Name = "TitleCheckbox";
            TitleCheckbox.Size = new Size(54, 17);
            TitleCheckbox.TabIndex = 1;
            TitleCheckbox.Text = "In title";
            TitleCheckbox.UseVisualStyleBackColor = true;
            // 
            // DescriptionCheckbox
            // 
            DescriptionCheckbox.AutoSize = true;
            DescriptionCheckbox.Checked = true;
            DescriptionCheckbox.CheckState = CheckState.Checked;
            DescriptionCheckbox.Location = new Point(72, 38);
            DescriptionCheckbox.Name = "DescriptionCheckbox";
            DescriptionCheckbox.Size = new Size(89, 17);
            DescriptionCheckbox.TabIndex = 2;
            DescriptionCheckbox.Text = "In description";
            DescriptionCheckbox.UseVisualStyleBackColor = true;
            // 
            // CommentCheckbox
            // 
            CommentCheckbox.AutoSize = true;
            CommentCheckbox.Checked = true;
            CommentCheckbox.CheckState = CheckState.Checked;
            CommentCheckbox.Location = new Point(167, 38);
            CommentCheckbox.Name = "CommentCheckbox";
            CommentCheckbox.Size = new Size(81, 17);
            CommentCheckbox.TabIndex = 3;
            CommentCheckbox.Text = "In comment";
            CommentCheckbox.UseVisualStyleBackColor = true;
            // 
            // SearchButton
            // 
            SearchButton.Location = new Point(289, 61);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 23);
            SearchButton.TabIndex = 4;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = true;
            // 
            // FindByText
            // 
            AcceptButton = SearchButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 96);
            Controls.Add(SearchButton);
            Controls.Add(CommentCheckbox);
            Controls.Add(DescriptionCheckbox);
            Controls.Add(TitleCheckbox);
            Controls.Add(StringTextbox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FindByText";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Find by Text";
            KeyUp += new KeyEventHandler(FindByText_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox StringTextbox;
        internal CheckBox TitleCheckbox;
        internal CheckBox DescriptionCheckbox;
        internal CheckBox CommentCheckbox;
        internal Button SearchButton;
    }
}