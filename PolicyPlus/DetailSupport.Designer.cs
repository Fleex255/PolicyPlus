using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DetailSupport : Form
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
            Label DisplayCodeLabel;
            Label LogicLabel;
            Label ProductsLabel;
            NameTextbox = new TextBox();
            IdTextbox = new TextBox();
            DefinedTextbox = new TextBox();
            DisplayCodeTextbox = new TextBox();
            LogicTextbox = new TextBox();
            EntriesListview = new ListView();
            EntriesListview.ClientSizeChanged += new EventHandler(EntriesListview_ClientSizeChanged);
            EntriesListview.DoubleClick += new EventHandler(EntriesListview_DoubleClick);
            ChName = new ColumnHeader();
            ChMinVer = new ColumnHeader();
            ChMaxVer = new ColumnHeader();
            CloseButton = new Button();
            NameLabel = new Label();
            IdLabel = new Label();
            DefinedLabel = new Label();
            DisplayCodeLabel = new Label();
            LogicLabel = new Label();
            ProductsLabel = new Label();
            SuspendLayout();
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(12, 15);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(35, 13);
            NameLabel.TabIndex = 5;
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
            // DisplayCodeLabel
            // 
            DisplayCodeLabel.AutoSize = true;
            DisplayCodeLabel.Location = new Point(12, 93);
            DisplayCodeLabel.Name = "DisplayCodeLabel";
            DisplayCodeLabel.Size = new Size(68, 13);
            DisplayCodeLabel.TabIndex = 8;
            DisplayCodeLabel.Text = "Display code";
            // 
            // LogicLabel
            // 
            LogicLabel.AutoSize = true;
            LogicLabel.Location = new Point(12, 119);
            LogicLabel.Name = "LogicLabel";
            LogicLabel.Size = new Size(64, 13);
            LogicLabel.TabIndex = 9;
            LogicLabel.Text = "Composition";
            // 
            // ProductsLabel
            // 
            ProductsLabel.AutoSize = true;
            ProductsLabel.Location = new Point(12, 145);
            ProductsLabel.Name = "ProductsLabel";
            ProductsLabel.Size = new Size(49, 13);
            ProductsLabel.TabIndex = 11;
            ProductsLabel.Text = "Products";
            // 
            // NameTextbox
            // 
            NameTextbox.Location = new Point(86, 12);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.ReadOnly = true;
            NameTextbox.Size = new Size(268, 20);
            NameTextbox.TabIndex = 0;
            // 
            // IdTextbox
            // 
            IdTextbox.Location = new Point(86, 38);
            IdTextbox.Name = "IdTextbox";
            IdTextbox.ReadOnly = true;
            IdTextbox.Size = new Size(268, 20);
            IdTextbox.TabIndex = 1;
            // 
            // DefinedTextbox
            // 
            DefinedTextbox.Location = new Point(86, 64);
            DefinedTextbox.Name = "DefinedTextbox";
            DefinedTextbox.ReadOnly = true;
            DefinedTextbox.Size = new Size(268, 20);
            DefinedTextbox.TabIndex = 2;
            // 
            // DisplayCodeTextbox
            // 
            DisplayCodeTextbox.Location = new Point(86, 90);
            DisplayCodeTextbox.Name = "DisplayCodeTextbox";
            DisplayCodeTextbox.ReadOnly = true;
            DisplayCodeTextbox.Size = new Size(268, 20);
            DisplayCodeTextbox.TabIndex = 3;
            // 
            // LogicTextbox
            // 
            LogicTextbox.Location = new Point(86, 116);
            LogicTextbox.Name = "LogicTextbox";
            LogicTextbox.ReadOnly = true;
            LogicTextbox.Size = new Size(268, 20);
            LogicTextbox.TabIndex = 4;
            // 
            // EntriesListview
            // 
            EntriesListview.Columns.AddRange(new ColumnHeader[] { ChName, ChMinVer, ChMaxVer });
            EntriesListview.FullRowSelect = true;
            EntriesListview.HideSelection = false;
            EntriesListview.Location = new Point(86, 142);
            EntriesListview.MultiSelect = false;
            EntriesListview.Name = "EntriesListview";
            EntriesListview.ShowItemToolTips = true;
            EntriesListview.Size = new Size(268, 87);
            EntriesListview.TabIndex = 12;
            EntriesListview.UseCompatibleStateImageBehavior = false;
            EntriesListview.View = View.Details;
            // 
            // ChName
            // 
            ChName.Text = "Name";
            ChName.Width = 158;
            // 
            // ChMinVer
            // 
            ChMinVer.Text = "Min";
            ChMinVer.Width = 40;
            // 
            // ChMaxVer
            // 
            ChMaxVer.Text = "Max";
            ChMaxVer.Width = 40;
            // 
            // CloseButton
            // 
            CloseButton.DialogResult = DialogResult.OK;
            CloseButton.Location = new Point(279, 235);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 13;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // DetailSupport
            // 
            AcceptButton = CloseButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(366, 270);
            Controls.Add(CloseButton);
            Controls.Add(EntriesListview);
            Controls.Add(ProductsLabel);
            Controls.Add(LogicLabel);
            Controls.Add(DisplayCodeLabel);
            Controls.Add(DefinedLabel);
            Controls.Add(IdLabel);
            Controls.Add(NameLabel);
            Controls.Add(LogicTextbox);
            Controls.Add(DisplayCodeTextbox);
            Controls.Add(DefinedTextbox);
            Controls.Add(IdTextbox);
            Controls.Add(NameTextbox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetailSupport";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Support Details";
            Shown += new EventHandler(EntriesListview_ClientSizeChanged);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox NameTextbox;
        internal TextBox IdTextbox;
        internal TextBox DefinedTextbox;
        internal TextBox DisplayCodeTextbox;
        internal TextBox LogicTextbox;
        internal ListView EntriesListview;
        internal ColumnHeader ChName;
        internal ColumnHeader ChMinVer;
        internal ColumnHeader ChMaxVer;
        internal Button CloseButton;
    }
}