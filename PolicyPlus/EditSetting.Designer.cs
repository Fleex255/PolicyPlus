using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EditSetting : Form
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
            Label SectionLabel;
            CommentLabel = new Label();
            SupportedLabel = new Label();
            SettingNameLabel = new Label();
            CommentTextbox = new TextBox();
            SupportedTextbox = new TextBox();
            NotConfiguredOption = new RadioButton();
            NotConfiguredOption.CheckedChanged += new EventHandler(StateRadiosChanged);
            EnabledOption = new RadioButton();
            EnabledOption.CheckedChanged += new EventHandler(StateRadiosChanged);
            DisabledOption = new RadioButton();
            DisabledOption.CheckedChanged += new EventHandler(StateRadiosChanged);
            ExtraOptionsPanel = new Panel();
            ExtraOptionsTable = new TableLayoutPanel();
            CloseButton = new Button();
            CloseButton.Click += new EventHandler(CancelButton_Click);
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            HelpTextbox = new TextBox();
            SectionDropdown = new ComboBox();
            SectionDropdown.SelectedIndexChanged += new EventHandler(SectionDropdown_SelectedIndexChanged);
            ApplyButton = new Button();
            ApplyButton.Click += new EventHandler(ApplyButton_Click);
            SectionLabel = new Label();
            ExtraOptionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // CommentLabel
            // 
            CommentLabel.AutoSize = true;
            CommentLabel.Location = new Point(260, 28);
            CommentLabel.Name = "CommentLabel";
            CommentLabel.Size = new Size(51, 13);
            CommentLabel.TabIndex = 2;
            CommentLabel.Text = "Comment";
            // 
            // SupportedLabel
            // 
            SupportedLabel.AutoSize = true;
            SupportedLabel.Location = new Point(240, 103);
            SupportedLabel.Name = "SupportedLabel";
            SupportedLabel.Size = new Size(71, 13);
            SupportedLabel.TabIndex = 4;
            SupportedLabel.Text = "Supported on";
            // 
            // SectionLabel
            // 
            SectionLabel.AutoSize = true;
            SectionLabel.Location = new Point(12, 28);
            SectionLabel.Name = "SectionLabel";
            SectionLabel.Size = new Size(54, 13);
            SectionLabel.TabIndex = 12;
            SectionLabel.Text = "Editing for";
            // 
            // SettingNameLabel
            // 
            SettingNameLabel.AutoEllipsis = true;
            SettingNameLabel.Location = new Point(12, 9);
            SettingNameLabel.Name = "SettingNameLabel";
            SettingNameLabel.Size = new Size(614, 13);
            SettingNameLabel.TabIndex = 0;
            SettingNameLabel.Text = "Policy name";
            // 
            // CommentTextbox
            // 
            CommentTextbox.AcceptsReturn = true;
            CommentTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CommentTextbox.Location = new Point(317, 25);
            CommentTextbox.Multiline = true;
            CommentTextbox.Name = "CommentTextbox";
            CommentTextbox.Size = new Size(309, 69);
            CommentTextbox.TabIndex = 100;
            // 
            // SupportedTextbox
            // 
            SupportedTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SupportedTextbox.Location = new Point(317, 100);
            SupportedTextbox.Multiline = true;
            SupportedTextbox.Name = "SupportedTextbox";
            SupportedTextbox.ReadOnly = true;
            SupportedTextbox.Size = new Size(309, 44);
            SupportedTextbox.TabIndex = 101;
            // 
            // NotConfiguredOption
            // 
            NotConfiguredOption.AutoSize = true;
            NotConfiguredOption.Location = new Point(12, 52);
            NotConfiguredOption.Name = "NotConfiguredOption";
            NotConfiguredOption.Size = new Size(96, 17);
            NotConfiguredOption.TabIndex = 1;
            NotConfiguredOption.TabStop = true;
            NotConfiguredOption.Text = "Not Configured";
            NotConfiguredOption.UseVisualStyleBackColor = true;
            // 
            // EnabledOption
            // 
            EnabledOption.AutoSize = true;
            EnabledOption.Location = new Point(12, 75);
            EnabledOption.Name = "EnabledOption";
            EnabledOption.Size = new Size(64, 17);
            EnabledOption.TabIndex = 2;
            EnabledOption.TabStop = true;
            EnabledOption.Text = "Enabled";
            EnabledOption.UseVisualStyleBackColor = true;
            // 
            // DisabledOption
            // 
            DisabledOption.AutoSize = true;
            DisabledOption.Location = new Point(12, 98);
            DisabledOption.Name = "DisabledOption";
            DisabledOption.Size = new Size(66, 17);
            DisabledOption.TabIndex = 3;
            DisabledOption.TabStop = true;
            DisabledOption.Text = "Disabled";
            DisabledOption.UseVisualStyleBackColor = true;
            // 
            // ExtraOptionsPanel
            // 
            ExtraOptionsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            ExtraOptionsPanel.BackColor = Color.White;
            ExtraOptionsPanel.BorderStyle = BorderStyle.FixedSingle;
            ExtraOptionsPanel.Controls.Add(ExtraOptionsTable);
            ExtraOptionsPanel.Location = new Point(12, 150);
            ExtraOptionsPanel.Name = "ExtraOptionsPanel";
            ExtraOptionsPanel.Size = new Size(299, 244);
            ExtraOptionsPanel.TabIndex = 8;
            // 
            // ExtraOptionsTable
            // 
            ExtraOptionsTable.AutoSize = true;
            ExtraOptionsTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ExtraOptionsTable.ColumnCount = 1;
            ExtraOptionsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 298.0f));
            ExtraOptionsTable.Location = new Point(0, 0);
            ExtraOptionsTable.Margin = new Padding(0);
            ExtraOptionsTable.MaximumSize = new Size(297, 0);
            ExtraOptionsTable.MinimumSize = new Size(297, 0);
            ExtraOptionsTable.Name = "ExtraOptionsTable";
            ExtraOptionsTable.RowCount = 1;
            ExtraOptionsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20.0f));
            ExtraOptionsTable.Size = new Size(297, 20);
            ExtraOptionsTable.TabIndex = 0;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.DialogResult = DialogResult.Cancel;
            CloseButton.Location = new Point(470, 400);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 104;
            CloseButton.Text = "Cancel";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkButton.Location = new Point(389, 400);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 103;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // HelpTextbox
            // 
            HelpTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            HelpTextbox.Location = new Point(317, 150);
            HelpTextbox.Multiline = true;
            HelpTextbox.Name = "HelpTextbox";
            HelpTextbox.ReadOnly = true;
            HelpTextbox.ScrollBars = ScrollBars.Both;
            HelpTextbox.Size = new Size(309, 244);
            HelpTextbox.TabIndex = 102;
            // 
            // SectionDropdown
            // 
            SectionDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            SectionDropdown.FormattingEnabled = true;
            SectionDropdown.Items.AddRange(new object[] { "User", "Computer" });
            SectionDropdown.Location = new Point(72, 25);
            SectionDropdown.Name = "SectionDropdown";
            SectionDropdown.Size = new Size(112, 21);
            SectionDropdown.TabIndex = 4;
            // 
            // ApplyButton
            // 
            ApplyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ApplyButton.Location = new Point(551, 400);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(75, 23);
            ApplyButton.TabIndex = 105;
            ApplyButton.Text = "Apply";
            ApplyButton.UseVisualStyleBackColor = true;
            // 
            // EditSetting
            // 
            AcceptButton = OkButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(638, 435);
            Controls.Add(ApplyButton);
            Controls.Add(SectionLabel);
            Controls.Add(SectionDropdown);
            Controls.Add(HelpTextbox);
            Controls.Add(OkButton);
            Controls.Add(CloseButton);
            Controls.Add(ExtraOptionsPanel);
            Controls.Add(DisabledOption);
            Controls.Add(EnabledOption);
            Controls.Add(NotConfiguredOption);
            Controls.Add(SupportedLabel);
            Controls.Add(SupportedTextbox);
            Controls.Add(CommentLabel);
            Controls.Add(CommentTextbox);
            Controls.Add(SettingNameLabel);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(654, 474);
            Name = "EditSetting";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Policy Setting";
            ExtraOptionsPanel.ResumeLayout(false);
            ExtraOptionsPanel.PerformLayout();
            Shown += new EventHandler(EditSetting_Shown);
            Resize += new EventHandler(EditSetting_Resize);
            FormClosed += new FormClosedEventHandler(EditSetting_FormClosed);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Label SettingNameLabel;
        internal TextBox CommentTextbox;
        internal TextBox SupportedTextbox;
        internal RadioButton NotConfiguredOption;
        internal RadioButton EnabledOption;
        internal RadioButton DisabledOption;
        internal Panel ExtraOptionsPanel;
        internal TableLayoutPanel ExtraOptionsTable;
        internal Button CloseButton;
        internal Button OkButton;
        internal TextBox HelpTextbox;
        internal ComboBox SectionDropdown;
        internal Button ApplyButton;
        internal Label CommentLabel;
        internal Label SupportedLabel;
    }
}