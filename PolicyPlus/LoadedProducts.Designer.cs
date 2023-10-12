using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class LoadedProducts : Form
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
            Label Label1;
            ColumnHeader ColumnHeader1;
            ColumnHeader ColumnHeader2;
            ColumnHeader ColumnHeader3;
            ColumnHeader ColumnHeader4;
            ColumnHeader ColumnHeader5;
            ColumnHeader ColumnHeader6;
            ColumnHeader ColumnHeader7;
            LsvTopLevelProducts = new ListView();
            LsvTopLevelProducts.SelectedIndexChanged += new EventHandler(UpdateMajorList);
            LsvTopLevelProducts.DoubleClick += new EventHandler(OpenProductDetails);
            LsvTopLevelProducts.KeyDown += new KeyEventHandler(ListKeyPressed);
            LabelMajorVersion = new Label();
            LsvMajorVersions = new ListView();
            LsvMajorVersions.SelectedIndexChanged += new EventHandler(UpdateMinorList);
            LsvMajorVersions.DoubleClick += new EventHandler(OpenProductDetails);
            LsvMajorVersions.KeyDown += new KeyEventHandler(ListKeyPressed);
            LabelMinorVersion = new Label();
            LsvMinorVersions = new ListView();
            LsvMinorVersions.DoubleClick += new EventHandler(OpenProductDetails);
            LsvMinorVersions.KeyDown += new KeyEventHandler(ListKeyPressed);
            ButtonClose = new Button();
            Label1 = new Label();
            ColumnHeader1 = new ColumnHeader();
            ColumnHeader2 = new ColumnHeader();
            ColumnHeader3 = new ColumnHeader();
            ColumnHeader4 = new ColumnHeader();
            ColumnHeader5 = new ColumnHeader();
            ColumnHeader6 = new ColumnHeader();
            ColumnHeader7 = new ColumnHeader();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 9);
            Label1.Name = "Label1";
            Label1.Size = new Size(95, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Top-level products";
            // 
            // ColumnHeader1
            // 
            ColumnHeader1.Text = "Name";
            ColumnHeader1.Width = 308;
            // 
            // ColumnHeader2
            // 
            ColumnHeader2.Text = "Children";
            ColumnHeader2.Width = 53;
            // 
            // ColumnHeader3
            // 
            ColumnHeader3.Text = "Name";
            ColumnHeader3.Width = 252;
            // 
            // ColumnHeader4
            // 
            ColumnHeader4.Text = "Version";
            ColumnHeader4.Width = 53;
            // 
            // ColumnHeader5
            // 
            ColumnHeader5.Text = "Name";
            ColumnHeader5.Width = 308;
            // 
            // ColumnHeader6
            // 
            ColumnHeader6.Text = "Version";
            ColumnHeader6.Width = 53;
            // 
            // ColumnHeader7
            // 
            ColumnHeader7.Text = "Children";
            // 
            // LsvTopLevelProducts
            // 
            LsvTopLevelProducts.Columns.AddRange(new ColumnHeader[] { ColumnHeader1, ColumnHeader2 });
            LsvTopLevelProducts.FullRowSelect = true;
            LsvTopLevelProducts.HideSelection = false;
            LsvTopLevelProducts.Location = new Point(12, 25);
            LsvTopLevelProducts.MultiSelect = false;
            LsvTopLevelProducts.Name = "LsvTopLevelProducts";
            LsvTopLevelProducts.ShowItemToolTips = true;
            LsvTopLevelProducts.Size = new Size(385, 97);
            LsvTopLevelProducts.TabIndex = 0;
            LsvTopLevelProducts.UseCompatibleStateImageBehavior = false;
            LsvTopLevelProducts.View = View.Details;
            // 
            // LabelMajorVersion
            // 
            LabelMajorVersion.AutoSize = true;
            LabelMajorVersion.Location = new Point(12, 125);
            LabelMajorVersion.Name = "LabelMajorVersion";
            LabelMajorVersion.Size = new Size(75, 13);
            LabelMajorVersion.TabIndex = 3;
            LabelMajorVersion.Text = "Major versions";
            // 
            // LsvMajorVersions
            // 
            LsvMajorVersions.Columns.AddRange(new ColumnHeader[] { ColumnHeader3, ColumnHeader4, ColumnHeader7 });
            LsvMajorVersions.FullRowSelect = true;
            LsvMajorVersions.HideSelection = false;
            LsvMajorVersions.Location = new Point(12, 141);
            LsvMajorVersions.MultiSelect = false;
            LsvMajorVersions.Name = "LsvMajorVersions";
            LsvMajorVersions.ShowItemToolTips = true;
            LsvMajorVersions.Size = new Size(385, 97);
            LsvMajorVersions.TabIndex = 1;
            LsvMajorVersions.UseCompatibleStateImageBehavior = false;
            LsvMajorVersions.View = View.Details;
            // 
            // LabelMinorVersion
            // 
            LabelMinorVersion.AutoSize = true;
            LabelMinorVersion.Location = new Point(12, 241);
            LabelMinorVersion.Name = "LabelMinorVersion";
            LabelMinorVersion.Size = new Size(75, 13);
            LabelMinorVersion.TabIndex = 5;
            LabelMinorVersion.Text = "Minor versions";
            // 
            // LsvMinorVersions
            // 
            LsvMinorVersions.Columns.AddRange(new ColumnHeader[] { ColumnHeader5, ColumnHeader6 });
            LsvMinorVersions.FullRowSelect = true;
            LsvMinorVersions.HideSelection = false;
            LsvMinorVersions.Location = new Point(12, 257);
            LsvMinorVersions.MultiSelect = false;
            LsvMinorVersions.Name = "LsvMinorVersions";
            LsvMinorVersions.ShowItemToolTips = true;
            LsvMinorVersions.Size = new Size(385, 97);
            LsvMinorVersions.TabIndex = 2;
            LsvMinorVersions.UseCompatibleStateImageBehavior = false;
            LsvMinorVersions.View = View.Details;
            // 
            // ButtonClose
            // 
            ButtonClose.DialogResult = DialogResult.OK;
            ButtonClose.Location = new Point(322, 360);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 3;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // LoadedProducts
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonClose;
            ClientSize = new Size(409, 395);
            Controls.Add(ButtonClose);
            Controls.Add(LabelMinorVersion);
            Controls.Add(LsvMinorVersions);
            Controls.Add(LabelMajorVersion);
            Controls.Add(LsvMajorVersions);
            Controls.Add(Label1);
            Controls.Add(LsvTopLevelProducts);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoadedProducts";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "All Products";
            ResumeLayout(false);
            PerformLayout();

        }

        internal ListView LsvTopLevelProducts;
        internal Label LabelMajorVersion;
        internal ListView LsvMajorVersions;
        internal Label LabelMinorVersion;
        internal ListView LsvMinorVersions;
        internal Button ButtonClose;
    }
}