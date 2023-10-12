using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FilterOptions : Form
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
            Label PolicyTypeLabel;
            Label PolicyStateLabel;
            Label CommentedLabel;
            PolicyTypeCombobox = new ComboBox();
            PolicyStateCombobox = new ComboBox();
            CommentedCombobox = new ComboBox();
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            ResetButton = new Button();
            ResetButton.Click += new EventHandler(ResetButton_Click);
            RequirementsBox = new GroupBox();
            AllowedProductsTreeview = new DoubleClickIgnoringTreeView();
            AllowedProductsTreeview.EnabledChanged += new EventHandler(AllowedProductsTreeview_EnabledChanged);
            AllowedProductsTreeview.AfterCheck += new TreeViewEventHandler(AllowedProductsTreeview_AfterCheck);
            MatchBlankSupportCheckbox = new CheckBox();
            AlwaysMatchAnyCheckbox = new CheckBox();
            SupportedCheckbox = new CheckBox();
            SupportedCheckbox.CheckedChanged += new EventHandler(SupportedCheckbox_CheckedChanged);
            PolicyTypeLabel = new Label();
            PolicyStateLabel = new Label();
            CommentedLabel = new Label();
            RequirementsBox.SuspendLayout();
            SuspendLayout();
            // 
            // PolicyTypeLabel
            // 
            PolicyTypeLabel.AutoSize = true;
            PolicyTypeLabel.Location = new Point(12, 9);
            PolicyTypeLabel.Name = "PolicyTypeLabel";
            PolicyTypeLabel.Size = new Size(58, 13);
            PolicyTypeLabel.TabIndex = 1;
            PolicyTypeLabel.Text = "Policy type";
            // 
            // PolicyStateLabel
            // 
            PolicyStateLabel.AutoSize = true;
            PolicyStateLabel.Location = new Point(121, 9);
            PolicyStateLabel.Name = "PolicyStateLabel";
            PolicyStateLabel.Size = new Size(67, 13);
            PolicyStateLabel.TabIndex = 3;
            PolicyStateLabel.Text = "Current state";
            // 
            // CommentedLabel
            // 
            CommentedLabel.AutoSize = true;
            CommentedLabel.Location = new Point(230, 9);
            CommentedLabel.Name = "CommentedLabel";
            CommentedLabel.Size = new Size(63, 13);
            CommentedLabel.TabIndex = 5;
            CommentedLabel.Text = "Commented";
            // 
            // PolicyTypeCombobox
            // 
            PolicyTypeCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            PolicyTypeCombobox.FormattingEnabled = true;
            PolicyTypeCombobox.Items.AddRange(new object[] { "Any", "Policy", "Preference" });
            PolicyTypeCombobox.Location = new Point(12, 25);
            PolicyTypeCombobox.Name = "PolicyTypeCombobox";
            PolicyTypeCombobox.Size = new Size(103, 21);
            PolicyTypeCombobox.TabIndex = 0;
            // 
            // PolicyStateCombobox
            // 
            PolicyStateCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            PolicyStateCombobox.FormattingEnabled = true;
            PolicyStateCombobox.Items.AddRange(new object[] { "Any", "Not Configured", "Configured", "Enabled", "Disabled" });
            PolicyStateCombobox.Location = new Point(121, 25);
            PolicyStateCombobox.Name = "PolicyStateCombobox";
            PolicyStateCombobox.Size = new Size(103, 21);
            PolicyStateCombobox.TabIndex = 2;
            // 
            // CommentedCombobox
            // 
            CommentedCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            CommentedCombobox.FormattingEnabled = true;
            CommentedCombobox.Items.AddRange(new object[] { "Any", "Yes", "No" });
            CommentedCombobox.Location = new Point(230, 25);
            CommentedCombobox.Name = "CommentedCombobox";
            CommentedCombobox.Size = new Size(103, 21);
            CommentedCombobox.TabIndex = 4;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OkButton.Location = new Point(258, 311);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 6;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ResetButton.Location = new Point(12, 311);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(75, 23);
            ResetButton.TabIndex = 7;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            // 
            // RequirementsBox
            // 
            RequirementsBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            RequirementsBox.Controls.Add(AllowedProductsTreeview);
            RequirementsBox.Controls.Add(MatchBlankSupportCheckbox);
            RequirementsBox.Controls.Add(AlwaysMatchAnyCheckbox);
            RequirementsBox.Enabled = false;
            RequirementsBox.Location = new Point(12, 52);
            RequirementsBox.Name = "RequirementsBox";
            RequirementsBox.Size = new Size(321, 253);
            RequirementsBox.TabIndex = 8;
            RequirementsBox.TabStop = false;
            // 
            // AllowedProductsTreeview
            // 
            AllowedProductsTreeview.CheckBoxes = true;
            AllowedProductsTreeview.FullRowSelect = true;
            AllowedProductsTreeview.HideSelection = false;
            AllowedProductsTreeview.Location = new Point(6, 69);
            AllowedProductsTreeview.Name = "AllowedProductsTreeview";
            AllowedProductsTreeview.ShowNodeToolTips = true;
            AllowedProductsTreeview.Size = new Size(309, 178);
            AllowedProductsTreeview.TabIndex = 10;
            // 
            // MatchBlankSupportCheckbox
            // 
            MatchBlankSupportCheckbox.AutoSize = true;
            MatchBlankSupportCheckbox.Checked = true;
            MatchBlankSupportCheckbox.CheckState = CheckState.Checked;
            MatchBlankSupportCheckbox.Location = new Point(6, 46);
            MatchBlankSupportCheckbox.Name = "MatchBlankSupportCheckbox";
            MatchBlankSupportCheckbox.Size = new Size(282, 17);
            MatchBlankSupportCheckbox.TabIndex = 0;
            MatchBlankSupportCheckbox.Text = "Match policies with missing or blank support definitions";
            MatchBlankSupportCheckbox.UseVisualStyleBackColor = true;
            // 
            // AlwaysMatchAnyCheckbox
            // 
            AlwaysMatchAnyCheckbox.AutoSize = true;
            AlwaysMatchAnyCheckbox.Checked = true;
            AlwaysMatchAnyCheckbox.CheckState = CheckState.Checked;
            AlwaysMatchAnyCheckbox.Location = new Point(6, 23);
            AlwaysMatchAnyCheckbox.Name = "AlwaysMatchAnyCheckbox";
            AlwaysMatchAnyCheckbox.Size = new Size(303, 17);
            AlwaysMatchAnyCheckbox.TabIndex = 0;
            AlwaysMatchAnyCheckbox.Text = "Match a policy if at least one selected product is supported";
            AlwaysMatchAnyCheckbox.UseVisualStyleBackColor = true;
            // 
            // SupportedCheckbox
            // 
            SupportedCheckbox.AutoSize = true;
            SupportedCheckbox.Location = new Point(18, 52);
            SupportedCheckbox.Name = "SupportedCheckbox";
            SupportedCheckbox.Size = new Size(90, 17);
            SupportedCheckbox.TabIndex = 9;
            SupportedCheckbox.Text = "Supported on";
            SupportedCheckbox.UseVisualStyleBackColor = true;
            // 
            // FilterOptions
            // 
            AcceptButton = OkButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 346);
            Controls.Add(SupportedCheckbox);
            Controls.Add(RequirementsBox);
            Controls.Add(ResetButton);
            Controls.Add(OkButton);
            Controls.Add(CommentedLabel);
            Controls.Add(CommentedCombobox);
            Controls.Add(PolicyStateLabel);
            Controls.Add(PolicyStateCombobox);
            Controls.Add(PolicyTypeLabel);
            Controls.Add(PolicyTypeCombobox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FilterOptions";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Filter Options";
            RequirementsBox.ResumeLayout(false);
            RequirementsBox.PerformLayout();
            KeyDown += new KeyEventHandler(FilterOptions_KeyDown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ComboBox PolicyTypeCombobox;
        internal ComboBox PolicyStateCombobox;
        internal ComboBox CommentedCombobox;
        internal Button OkButton;
        internal Button ResetButton;
        internal GroupBox RequirementsBox;
        internal CheckBox SupportedCheckbox;
        internal TreeView AllowedProductsTreeview;
        internal CheckBox MatchBlankSupportCheckbox;
        internal CheckBox AlwaysMatchAnyCheckbox;
    }
}