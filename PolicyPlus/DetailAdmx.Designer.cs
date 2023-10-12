using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class DetailAdmx : Form
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
            Label Label2;
            Label Label3;
            Label Label4;
            Label Label5;
            Label Label6;
            Label Label7;
            TextPath = new TextBox();
            TextNamespace = new TextBox();
            TextSupersededAdm = new TextBox();
            LsvPolicies = new ListView();
            LsvPolicies.DoubleClick += new EventHandler(LsvPolicies_DoubleClick);
            ChPolicyId = new ColumnHeader();
            ChName = new ColumnHeader();
            LsvCategories = new ListView();
            LsvCategories.DoubleClick += new EventHandler(LsvCategories_DoubleClick);
            ChCategoryId = new ColumnHeader();
            ChCategoryName = new ColumnHeader();
            LsvProducts = new ListView();
            LsvProducts.DoubleClick += new EventHandler(LsvProducts_DoubleClick);
            ChProductId = new ColumnHeader();
            ChProductName = new ColumnHeader();
            LsvSupportDefinitions = new ListView();
            LsvSupportDefinitions.DoubleClick += new EventHandler(LsvSupportDefinitions_DoubleClick);
            ChSupportId = new ColumnHeader();
            ChSupportName = new ColumnHeader();
            ButtonClose = new Button();
            Label1 = new Label();
            Label2 = new Label();
            Label3 = new Label();
            Label4 = new Label();
            Label5 = new Label();
            Label6 = new Label();
            Label7 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(29, 13);
            Label1.TabIndex = 1;
            Label1.Text = "Path";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 41);
            Label2.Name = "Label2";
            Label2.Size = new Size(64, 13);
            Label2.TabIndex = 3;
            Label2.Text = "Namespace";
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(12, 67);
            Label3.Name = "Label3";
            Label3.Size = new Size(91, 13);
            Label3.TabIndex = 5;
            Label3.Text = "Superseded ADM";
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(12, 93);
            Label4.Name = "Label4";
            Label4.Size = new Size(43, 13);
            Label4.TabIndex = 7;
            Label4.Text = "Policies";
            // 
            // Label5
            // 
            Label5.AutoSize = true;
            Label5.Location = new Point(12, 196);
            Label5.Name = "Label5";
            Label5.Size = new Size(57, 13);
            Label5.TabIndex = 9;
            Label5.Text = "Categories";
            // 
            // Label6
            // 
            Label6.AutoSize = true;
            Label6.Location = new Point(12, 299);
            Label6.Name = "Label6";
            Label6.Size = new Size(49, 13);
            Label6.TabIndex = 11;
            Label6.Text = "Products";
            // 
            // Label7
            // 
            Label7.AutoSize = true;
            Label7.Location = new Point(12, 402);
            Label7.Name = "Label7";
            Label7.Size = new Size(69, 13);
            Label7.TabIndex = 13;
            Label7.Text = "Support rules";
            // 
            // TextPath
            // 
            TextPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TextPath.Location = new Point(109, 12);
            TextPath.Name = "TextPath";
            TextPath.ReadOnly = true;
            TextPath.Size = new Size(332, 20);
            TextPath.TabIndex = 0;
            // 
            // TextNamespace
            // 
            TextNamespace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TextNamespace.Location = new Point(109, 38);
            TextNamespace.Name = "TextNamespace";
            TextNamespace.ReadOnly = true;
            TextNamespace.Size = new Size(332, 20);
            TextNamespace.TabIndex = 2;
            // 
            // TextSupersededAdm
            // 
            TextSupersededAdm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TextSupersededAdm.Location = new Point(109, 64);
            TextSupersededAdm.Name = "TextSupersededAdm";
            TextSupersededAdm.ReadOnly = true;
            TextSupersededAdm.Size = new Size(332, 20);
            TextSupersededAdm.TabIndex = 4;
            // 
            // LsvPolicies
            // 
            LsvPolicies.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LsvPolicies.Columns.AddRange(new ColumnHeader[] { ChPolicyId, ChName });
            LsvPolicies.FullRowSelect = true;
            LsvPolicies.Location = new Point(109, 90);
            LsvPolicies.MultiSelect = false;
            LsvPolicies.Name = "LsvPolicies";
            LsvPolicies.ShowItemToolTips = true;
            LsvPolicies.Size = new Size(332, 97);
            LsvPolicies.TabIndex = 6;
            LsvPolicies.UseCompatibleStateImageBehavior = false;
            LsvPolicies.View = View.Details;
            // 
            // ChPolicyId
            // 
            ChPolicyId.Text = "Local ID";
            ChPolicyId.Width = 73;
            // 
            // ChName
            // 
            ChName.Text = "Name";
            ChName.Width = 166;
            // 
            // LsvCategories
            // 
            LsvCategories.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LsvCategories.Columns.AddRange(new ColumnHeader[] { ChCategoryId, ChCategoryName });
            LsvCategories.FullRowSelect = true;
            LsvCategories.Location = new Point(109, 193);
            LsvCategories.MultiSelect = false;
            LsvCategories.Name = "LsvCategories";
            LsvCategories.ShowItemToolTips = true;
            LsvCategories.Size = new Size(332, 97);
            LsvCategories.TabIndex = 8;
            LsvCategories.UseCompatibleStateImageBehavior = false;
            LsvCategories.View = View.Details;
            // 
            // ChCategoryId
            // 
            ChCategoryId.Text = "Local ID";
            ChCategoryId.Width = 73;
            // 
            // ChCategoryName
            // 
            ChCategoryName.Text = "Name";
            ChCategoryName.Width = 166;
            // 
            // LsvProducts
            // 
            LsvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LsvProducts.Columns.AddRange(new ColumnHeader[] { ChProductId, ChProductName });
            LsvProducts.FullRowSelect = true;
            LsvProducts.Location = new Point(109, 296);
            LsvProducts.MultiSelect = false;
            LsvProducts.Name = "LsvProducts";
            LsvProducts.ShowItemToolTips = true;
            LsvProducts.Size = new Size(332, 97);
            LsvProducts.TabIndex = 10;
            LsvProducts.UseCompatibleStateImageBehavior = false;
            LsvProducts.View = View.Details;
            // 
            // ChProductId
            // 
            ChProductId.Text = "Local ID";
            ChProductId.Width = 73;
            // 
            // ChProductName
            // 
            ChProductName.Text = "Name";
            ChProductName.Width = 166;
            // 
            // LsvSupportDefinitions
            // 
            LsvSupportDefinitions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LsvSupportDefinitions.Columns.AddRange(new ColumnHeader[] { ChSupportId, ChSupportName });
            LsvSupportDefinitions.FullRowSelect = true;
            LsvSupportDefinitions.Location = new Point(109, 399);
            LsvSupportDefinitions.MultiSelect = false;
            LsvSupportDefinitions.Name = "LsvSupportDefinitions";
            LsvSupportDefinitions.ShowItemToolTips = true;
            LsvSupportDefinitions.Size = new Size(332, 97);
            LsvSupportDefinitions.TabIndex = 12;
            LsvSupportDefinitions.UseCompatibleStateImageBehavior = false;
            LsvSupportDefinitions.View = View.Details;
            // 
            // ChSupportId
            // 
            ChSupportId.Text = "Local ID";
            ChSupportId.Width = 73;
            // 
            // ChSupportName
            // 
            ChSupportName.Text = "Name";
            ChSupportName.Width = 166;
            // 
            // ButtonClose
            // 
            ButtonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonClose.DialogResult = DialogResult.OK;
            ButtonClose.Location = new Point(366, 502);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 14;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // DetailAdmx
            // 
            AcceptButton = ButtonClose;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonClose;
            ClientSize = new Size(453, 537);
            Controls.Add(ButtonClose);
            Controls.Add(Label7);
            Controls.Add(LsvSupportDefinitions);
            Controls.Add(Label6);
            Controls.Add(LsvProducts);
            Controls.Add(Label5);
            Controls.Add(LsvCategories);
            Controls.Add(Label4);
            Controls.Add(LsvPolicies);
            Controls.Add(Label3);
            Controls.Add(TextSupersededAdm);
            Controls.Add(Label2);
            Controls.Add(TextNamespace);
            Controls.Add(Label1);
            Controls.Add(TextPath);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DetailAdmx";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ADMX Details";
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextPath;
        internal TextBox TextNamespace;
        internal TextBox TextSupersededAdm;
        internal ListView LsvPolicies;
        internal ColumnHeader ChPolicyId;
        internal ColumnHeader ChName;
        internal ListView LsvCategories;
        internal ColumnHeader ChCategoryId;
        internal ColumnHeader ChCategoryName;
        internal ListView LsvProducts;
        internal ColumnHeader ChProductId;
        internal ColumnHeader ChProductName;
        internal ListView LsvSupportDefinitions;
        internal ColumnHeader ChSupportId;
        internal ColumnHeader ChSupportName;
        internal Button ButtonClose;
    }
}