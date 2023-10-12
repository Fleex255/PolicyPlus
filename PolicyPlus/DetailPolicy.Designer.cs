using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DetailPolicy : Form
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
            Label NameLabel;
            Label IdLabel;
            Label DefinedLabel;
            Label DisplayLabel;
            Label InfoLabel;
            Label PresentLabel;
            Label SectionLabel;
            Label SupportLabel;
            Label CategoryLabel;
            NameTextbox = new TextBox();
            IdTextbox = new TextBox();
            DefinedTextbox = new TextBox();
            DisplayCodeTextbox = new TextBox();
            InfoCodeTextbox = new TextBox();
            PresentCodeTextbox = new TextBox();
            SectionTextbox = new TextBox();
            SupportTextbox = new TextBox();
            CategoryTextbox = new TextBox();
            CategoryButton = new Button();
            CategoryButton.Click += new EventHandler(CategoryButton_Click);
            SupportButton = new Button();
            SupportButton.Click += new EventHandler(SupportButton_Click);
            CloseButton = new Button();
            NameLabel = new Label();
            IdLabel = new Label();
            DefinedLabel = new Label();
            DisplayLabel = new Label();
            InfoLabel = new Label();
            PresentLabel = new Label();
            SectionLabel = new Label();
            SupportLabel = new Label();
            CategoryLabel = new Label();
            SuspendLayout();
            // 
            // NameTextbox
            // 
            NameTextbox.Location = new Point(111, 12);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.ReadOnly = true;
            NameTextbox.Size = new Size(258, 20);
            NameTextbox.TabIndex = 0;
            // 
            // IdTextbox
            // 
            IdTextbox.Location = new Point(111, 38);
            IdTextbox.Name = "IdTextbox";
            IdTextbox.ReadOnly = true;
            IdTextbox.Size = new Size(258, 20);
            IdTextbox.TabIndex = 1;
            // 
            // DefinedTextbox
            // 
            DefinedTextbox.Location = new Point(111, 64);
            DefinedTextbox.Name = "DefinedTextbox";
            DefinedTextbox.ReadOnly = true;
            DefinedTextbox.Size = new Size(258, 20);
            DefinedTextbox.TabIndex = 2;
            // 
            // DisplayCodeTextbox
            // 
            DisplayCodeTextbox.Location = new Point(111, 90);
            DisplayCodeTextbox.Name = "DisplayCodeTextbox";
            DisplayCodeTextbox.ReadOnly = true;
            DisplayCodeTextbox.Size = new Size(258, 20);
            DisplayCodeTextbox.TabIndex = 3;
            // 
            // InfoCodeTextbox
            // 
            InfoCodeTextbox.Location = new Point(111, 116);
            InfoCodeTextbox.Name = "InfoCodeTextbox";
            InfoCodeTextbox.ReadOnly = true;
            InfoCodeTextbox.Size = new Size(258, 20);
            InfoCodeTextbox.TabIndex = 4;
            // 
            // PresentCodeTextbox
            // 
            PresentCodeTextbox.Location = new Point(111, 142);
            PresentCodeTextbox.Name = "PresentCodeTextbox";
            PresentCodeTextbox.ReadOnly = true;
            PresentCodeTextbox.Size = new Size(258, 20);
            PresentCodeTextbox.TabIndex = 5;
            // 
            // SectionTextbox
            // 
            SectionTextbox.Location = new Point(111, 168);
            SectionTextbox.Name = "SectionTextbox";
            SectionTextbox.ReadOnly = true;
            SectionTextbox.Size = new Size(258, 20);
            SectionTextbox.TabIndex = 6;
            // 
            // SupportTextbox
            // 
            SupportTextbox.Location = new Point(111, 194);
            SupportTextbox.Name = "SupportTextbox";
            SupportTextbox.ReadOnly = true;
            SupportTextbox.Size = new Size(177, 20);
            SupportTextbox.TabIndex = 7;
            // 
            // CategoryTextbox
            // 
            CategoryTextbox.Location = new Point(111, 220);
            CategoryTextbox.Name = "CategoryTextbox";
            CategoryTextbox.ReadOnly = true;
            CategoryTextbox.Size = new Size(177, 20);
            CategoryTextbox.TabIndex = 8;
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(12, 15);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(35, 13);
            NameLabel.TabIndex = 9;
            NameLabel.Text = "Name";
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(12, 41);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(55, 13);
            IdLabel.TabIndex = 10;
            IdLabel.Text = "Unique ID";
            // 
            // DefinedLabel
            // 
            DefinedLabel.AutoSize = true;
            DefinedLabel.Location = new Point(12, 67);
            DefinedLabel.Name = "DefinedLabel";
            DefinedLabel.Size = new Size(55, 13);
            DefinedLabel.TabIndex = 11;
            DefinedLabel.Text = "Defined in";
            // 
            // DisplayLabel
            // 
            DisplayLabel.AutoSize = true;
            DisplayLabel.Location = new Point(12, 93);
            DisplayLabel.Name = "DisplayLabel";
            DisplayLabel.Size = new Size(68, 13);
            DisplayLabel.TabIndex = 12;
            DisplayLabel.Text = "Display code";
            // 
            // InfoLabel
            // 
            InfoLabel.AutoSize = true;
            InfoLabel.Location = new Point(12, 119);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new Size(52, 13);
            InfoLabel.TabIndex = 13;
            InfoLabel.Text = "Info code";
            // 
            // PresentLabel
            // 
            PresentLabel.AutoSize = true;
            PresentLabel.Location = new Point(12, 145);
            PresentLabel.Name = "PresentLabel";
            PresentLabel.Size = new Size(93, 13);
            PresentLabel.TabIndex = 14;
            PresentLabel.Text = "Presentation code";
            // 
            // SectionLabel
            // 
            SectionLabel.AutoSize = true;
            SectionLabel.Location = new Point(12, 171);
            SectionLabel.Name = "SectionLabel";
            SectionLabel.Size = new Size(43, 13);
            SectionLabel.TabIndex = 15;
            SectionLabel.Text = "Section";
            // 
            // SupportLabel
            // 
            SupportLabel.AutoSize = true;
            SupportLabel.Location = new Point(12, 197);
            SupportLabel.Name = "SupportLabel";
            SupportLabel.Size = new Size(71, 13);
            SupportLabel.TabIndex = 16;
            SupportLabel.Text = "Supported on";
            // 
            // CategoryLabel
            // 
            CategoryLabel.AutoSize = true;
            CategoryLabel.Location = new Point(12, 223);
            CategoryLabel.Name = "CategoryLabel";
            CategoryLabel.Size = new Size(49, 13);
            CategoryLabel.TabIndex = 17;
            CategoryLabel.Text = "Category";
            // 
            // CategoryButton
            // 
            CategoryButton.Location = new Point(294, 218);
            CategoryButton.Name = "CategoryButton";
            CategoryButton.Size = new Size(75, 23);
            CategoryButton.TabIndex = 18;
            CategoryButton.Text = "Details";
            CategoryButton.UseVisualStyleBackColor = true;
            // 
            // SupportButton
            // 
            SupportButton.Location = new Point(294, 192);
            SupportButton.Name = "SupportButton";
            SupportButton.Size = new Size(75, 23);
            SupportButton.TabIndex = 17;
            SupportButton.Text = "Details";
            SupportButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            CloseButton.DialogResult = DialogResult.OK;
            CloseButton.Location = new Point(294, 247);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 19;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // DetailPolicy
            // 
            AcceptButton = CloseButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(381, 282);
            Controls.Add(CloseButton);
            Controls.Add(SupportButton);
            Controls.Add(CategoryButton);
            Controls.Add(CategoryLabel);
            Controls.Add(SupportLabel);
            Controls.Add(SectionLabel);
            Controls.Add(PresentLabel);
            Controls.Add(InfoLabel);
            Controls.Add(DisplayLabel);
            Controls.Add(DefinedLabel);
            Controls.Add(IdLabel);
            Controls.Add(NameLabel);
            Controls.Add(CategoryTextbox);
            Controls.Add(SupportTextbox);
            Controls.Add(SectionTextbox);
            Controls.Add(PresentCodeTextbox);
            Controls.Add(InfoCodeTextbox);
            Controls.Add(DisplayCodeTextbox);
            Controls.Add(DefinedTextbox);
            Controls.Add(IdTextbox);
            Controls.Add(NameTextbox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetailPolicy";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Policy Details";
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox NameTextbox;
        internal TextBox IdTextbox;
        internal TextBox DefinedTextbox;
        internal TextBox DisplayCodeTextbox;
        internal TextBox InfoCodeTextbox;
        internal TextBox PresentCodeTextbox;
        internal TextBox SectionTextbox;
        internal TextBox SupportTextbox;
        internal TextBox CategoryTextbox;
        internal Button CategoryButton;
        internal Button SupportButton;
        internal Button CloseButton;
    }
}