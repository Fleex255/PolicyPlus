using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DetailProduct : Form
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
            Label KindLabel;
            Label VersionLabel;
            Label ParentLabel;
            Label ChildrenLabel;
            Label DisplayCodeLabel;
            NameTextbox = new TextBox();
            IdTextbox = new TextBox();
            DefinedTextbox = new TextBox();
            DisplayCodeTextbox = new TextBox();
            KindTextbox = new TextBox();
            ParentButton = new Button();
            ParentButton.Click += new EventHandler(ParentButton_Click);
            ParentTextbox = new TextBox();
            ChildrenListview = new ListView();
            ChildrenListview.ClientSizeChanged += new EventHandler(ChildrenListview_ClientSizeChanged);
            ChildrenListview.DoubleClick += new EventHandler(ChildrenListview_DoubleClick);
            ChVersion = new ColumnHeader();
            ChName = new ColumnHeader();
            CloseButton = new Button();
            VersionTextbox = new TextBox();
            NameLabel = new Label();
            IdLabel = new Label();
            DefinedLabel = new Label();
            KindLabel = new Label();
            VersionLabel = new Label();
            ParentLabel = new Label();
            ChildrenLabel = new Label();
            DisplayCodeLabel = new Label();
            SuspendLayout();
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(12, 15);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(35, 13);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Name";
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(12, 41);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(55, 13);
            IdLabel.TabIndex = 6;
            IdLabel.Text = "Unique ID";
            // 
            // DefinedLabel
            // 
            DefinedLabel.AutoSize = true;
            DefinedLabel.Location = new Point(12, 67);
            DefinedLabel.Name = "DefinedLabel";
            DefinedLabel.Size = new Size(55, 13);
            DefinedLabel.TabIndex = 7;
            DefinedLabel.Text = "Defined in";
            // 
            // KindLabel
            // 
            KindLabel.AutoSize = true;
            KindLabel.Location = new Point(12, 119);
            KindLabel.Name = "KindLabel";
            KindLabel.Size = new Size(28, 13);
            KindLabel.TabIndex = 8;
            KindLabel.Text = "Kind";
            // 
            // VersionLabel
            // 
            VersionLabel.AutoSize = true;
            VersionLabel.Location = new Point(12, 145);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(80, 13);
            VersionLabel.TabIndex = 9;
            VersionLabel.Text = "Version number";
            // 
            // ParentLabel
            // 
            ParentLabel.AutoSize = true;
            ParentLabel.Location = new Point(12, 171);
            ParentLabel.Name = "ParentLabel";
            ParentLabel.Size = new Size(38, 13);
            ParentLabel.TabIndex = 12;
            ParentLabel.Text = "Parent";
            // 
            // ChildrenLabel
            // 
            ChildrenLabel.AutoSize = true;
            ChildrenLabel.ForeColor = SystemColors.ControlText;
            ChildrenLabel.Location = new Point(12, 197);
            ChildrenLabel.Name = "ChildrenLabel";
            ChildrenLabel.Size = new Size(67, 13);
            ChildrenLabel.TabIndex = 14;
            ChildrenLabel.Text = "Subproducts";
            // 
            // DisplayCodeLabel
            // 
            DisplayCodeLabel.AutoSize = true;
            DisplayCodeLabel.Location = new Point(12, 93);
            DisplayCodeLabel.Name = "DisplayCodeLabel";
            DisplayCodeLabel.Size = new Size(68, 13);
            DisplayCodeLabel.TabIndex = 16;
            DisplayCodeLabel.Text = "Display code";
            // 
            // NameTextbox
            // 
            NameTextbox.Location = new Point(98, 12);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.ReadOnly = true;
            NameTextbox.Size = new Size(256, 20);
            NameTextbox.TabIndex = 0;
            // 
            // IdTextbox
            // 
            IdTextbox.Location = new Point(98, 38);
            IdTextbox.Name = "IdTextbox";
            IdTextbox.ReadOnly = true;
            IdTextbox.Size = new Size(256, 20);
            IdTextbox.TabIndex = 2;
            // 
            // DefinedTextbox
            // 
            DefinedTextbox.Location = new Point(98, 64);
            DefinedTextbox.Name = "DefinedTextbox";
            DefinedTextbox.ReadOnly = true;
            DefinedTextbox.Size = new Size(256, 20);
            DefinedTextbox.TabIndex = 3;
            // 
            // DisplayCodeTextbox
            // 
            DisplayCodeTextbox.Location = new Point(98, 90);
            DisplayCodeTextbox.Name = "DisplayCodeTextbox";
            DisplayCodeTextbox.ReadOnly = true;
            DisplayCodeTextbox.Size = new Size(256, 20);
            DisplayCodeTextbox.TabIndex = 4;
            // 
            // KindTextbox
            // 
            KindTextbox.Location = new Point(98, 116);
            KindTextbox.Name = "KindTextbox";
            KindTextbox.ReadOnly = true;
            KindTextbox.Size = new Size(256, 20);
            KindTextbox.TabIndex = 5;
            // 
            // ParentButton
            // 
            ParentButton.Location = new Point(279, 166);
            ParentButton.Name = "ParentButton";
            ParentButton.Size = new Size(75, 23);
            ParentButton.TabIndex = 10;
            ParentButton.Text = "Details";
            ParentButton.UseVisualStyleBackColor = true;
            // 
            // ParentTextbox
            // 
            ParentTextbox.Location = new Point(98, 168);
            ParentTextbox.Name = "ParentTextbox";
            ParentTextbox.ReadOnly = true;
            ParentTextbox.Size = new Size(175, 20);
            ParentTextbox.TabIndex = 7;
            // 
            // ChildrenListview
            // 
            ChildrenListview.Columns.AddRange(new ColumnHeader[] { ChVersion, ChName });
            ChildrenListview.FullRowSelect = true;
            ChildrenListview.HideSelection = false;
            ChildrenListview.Location = new Point(98, 194);
            ChildrenListview.MultiSelect = false;
            ChildrenListview.Name = "ChildrenListview";
            ChildrenListview.Size = new Size(256, 110);
            ChildrenListview.TabIndex = 13;
            ChildrenListview.UseCompatibleStateImageBehavior = false;
            ChildrenListview.View = View.Details;
            // 
            // ChVersion
            // 
            ChVersion.Text = "Version";
            ChVersion.Width = 51;
            // 
            // ChName
            // 
            ChName.Text = "Name";
            ChName.Width = 176;
            // 
            // CloseButton
            // 
            CloseButton.DialogResult = DialogResult.OK;
            CloseButton.Location = new Point(279, 310);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 15;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // VersionTextbox
            // 
            VersionTextbox.Location = new Point(98, 142);
            VersionTextbox.Name = "VersionTextbox";
            VersionTextbox.ReadOnly = true;
            VersionTextbox.Size = new Size(256, 20);
            VersionTextbox.TabIndex = 6;
            // 
            // DetailProduct
            // 
            AcceptButton = CloseButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(366, 345);
            Controls.Add(VersionTextbox);
            Controls.Add(DisplayCodeLabel);
            Controls.Add(CloseButton);
            Controls.Add(ChildrenLabel);
            Controls.Add(ChildrenListview);
            Controls.Add(ParentLabel);
            Controls.Add(ParentTextbox);
            Controls.Add(ParentButton);
            Controls.Add(VersionLabel);
            Controls.Add(KindLabel);
            Controls.Add(DefinedLabel);
            Controls.Add(IdLabel);
            Controls.Add(KindTextbox);
            Controls.Add(DisplayCodeTextbox);
            Controls.Add(DefinedTextbox);
            Controls.Add(IdTextbox);
            Controls.Add(NameLabel);
            Controls.Add(NameTextbox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetailProduct";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Product Details";
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox NameTextbox;
        internal TextBox IdTextbox;
        internal TextBox DefinedTextbox;
        internal TextBox DisplayCodeTextbox;
        internal TextBox KindTextbox;
        internal Button ParentButton;
        internal TextBox ParentTextbox;
        internal ListView ChildrenListview;
        internal ColumnHeader ChVersion;
        internal ColumnHeader ChName;
        internal Button CloseButton;
        internal TextBox VersionTextbox;
    }
}