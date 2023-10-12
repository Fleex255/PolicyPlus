using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DetailCategory : Form
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
            Label DisplayCode;
            Label InfoCodeLabel;
            Label ParentLabel;
            NameTextbox = new TextBox();
            IdTextbox = new TextBox();
            DefinedTextbox = new TextBox();
            DisplayCodeTextbox = new TextBox();
            InfoCodeTextbox = new TextBox();
            ParentTextbox = new TextBox();
            ParentButton = new Button();
            ParentButton.Click += new EventHandler(ParentButton_Click);
            CloseButton = new Button();
            NameLabel = new Label();
            IdLabel = new Label();
            DefinedLabel = new Label();
            DisplayCode = new Label();
            InfoCodeLabel = new Label();
            ParentLabel = new Label();
            SuspendLayout();
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(12, 15);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(35, 13);
            NameLabel.TabIndex = 7;
            NameLabel.Text = "Name";
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(12, 41);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(55, 13);
            IdLabel.TabIndex = 8;
            IdLabel.Text = "Unique ID";
            // 
            // DefinedLabel
            // 
            DefinedLabel.AutoSize = true;
            DefinedLabel.Location = new Point(12, 67);
            DefinedLabel.Name = "DefinedLabel";
            DefinedLabel.Size = new Size(55, 13);
            DefinedLabel.TabIndex = 9;
            DefinedLabel.Text = "Defined in";
            // 
            // DisplayCode
            // 
            DisplayCode.AutoSize = true;
            DisplayCode.Location = new Point(12, 93);
            DisplayCode.Name = "DisplayCode";
            DisplayCode.Size = new Size(68, 13);
            DisplayCode.TabIndex = 10;
            DisplayCode.Text = "Display code";
            // 
            // InfoCodeLabel
            // 
            InfoCodeLabel.AutoSize = true;
            InfoCodeLabel.Location = new Point(12, 119);
            InfoCodeLabel.Name = "InfoCodeLabel";
            InfoCodeLabel.Size = new Size(52, 13);
            InfoCodeLabel.TabIndex = 11;
            InfoCodeLabel.Text = "Info code";
            // 
            // ParentLabel
            // 
            ParentLabel.AutoSize = true;
            ParentLabel.Location = new Point(12, 145);
            ParentLabel.Name = "ParentLabel";
            ParentLabel.Size = new Size(38, 13);
            ParentLabel.TabIndex = 12;
            ParentLabel.Text = "Parent";
            // 
            // NameTextbox
            // 
            NameTextbox.Location = new Point(86, 12);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.ReadOnly = true;
            NameTextbox.Size = new Size(225, 20);
            NameTextbox.TabIndex = 0;
            // 
            // IdTextbox
            // 
            IdTextbox.Location = new Point(86, 38);
            IdTextbox.Name = "IdTextbox";
            IdTextbox.ReadOnly = true;
            IdTextbox.Size = new Size(225, 20);
            IdTextbox.TabIndex = 1;
            // 
            // DefinedTextbox
            // 
            DefinedTextbox.Location = new Point(86, 64);
            DefinedTextbox.Name = "DefinedTextbox";
            DefinedTextbox.ReadOnly = true;
            DefinedTextbox.Size = new Size(225, 20);
            DefinedTextbox.TabIndex = 2;
            // 
            // DisplayCodeTextbox
            // 
            DisplayCodeTextbox.Location = new Point(86, 90);
            DisplayCodeTextbox.Name = "DisplayCodeTextbox";
            DisplayCodeTextbox.ReadOnly = true;
            DisplayCodeTextbox.Size = new Size(225, 20);
            DisplayCodeTextbox.TabIndex = 3;
            // 
            // InfoCodeTextbox
            // 
            InfoCodeTextbox.Location = new Point(86, 116);
            InfoCodeTextbox.Name = "InfoCodeTextbox";
            InfoCodeTextbox.ReadOnly = true;
            InfoCodeTextbox.Size = new Size(225, 20);
            InfoCodeTextbox.TabIndex = 4;
            // 
            // ParentTextbox
            // 
            ParentTextbox.Location = new Point(86, 142);
            ParentTextbox.Name = "ParentTextbox";
            ParentTextbox.ReadOnly = true;
            ParentTextbox.Size = new Size(144, 20);
            ParentTextbox.TabIndex = 5;
            // 
            // ParentButton
            // 
            ParentButton.Location = new Point(236, 140);
            ParentButton.Name = "ParentButton";
            ParentButton.Size = new Size(75, 23);
            ParentButton.TabIndex = 6;
            ParentButton.Text = "Details";
            ParentButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            CloseButton.DialogResult = DialogResult.OK;
            CloseButton.Location = new Point(236, 169);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 13;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // DetailCategory
            // 
            AcceptButton = CloseButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(323, 204);
            Controls.Add(CloseButton);
            Controls.Add(ParentLabel);
            Controls.Add(InfoCodeLabel);
            Controls.Add(DisplayCode);
            Controls.Add(DefinedLabel);
            Controls.Add(IdLabel);
            Controls.Add(NameLabel);
            Controls.Add(ParentButton);
            Controls.Add(ParentTextbox);
            Controls.Add(InfoCodeTextbox);
            Controls.Add(DisplayCodeTextbox);
            Controls.Add(DefinedTextbox);
            Controls.Add(IdTextbox);
            Controls.Add(NameTextbox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetailCategory";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Category Details";
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox NameTextbox;
        internal TextBox IdTextbox;
        internal TextBox DefinedTextbox;
        internal TextBox DisplayCodeTextbox;
        internal TextBox InfoCodeTextbox;
        internal TextBox ParentTextbox;
        internal Button ParentButton;
        internal Button CloseButton;
    }
}