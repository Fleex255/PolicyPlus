using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class EditPol : Form
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
            LsvPol = new ListView();
            LsvPol.SelectedIndexChanged += new EventHandler(LsvPol_SelectedIndexChanged);
            ChItem = new ColumnHeader();
            ChValue = new ColumnHeader();
            ButtonSave = new Button();
            ButtonSave.Click += new EventHandler(ButtonSave_Click);
            ButtonAddKey = new Button();
            ButtonAddKey.Click += new EventHandler(ButtonAddKey_Click);
            ButtonAddValue = new Button();
            ButtonAddValue.Click += new EventHandler(ButtonAddValue_Click);
            ButtonDeleteValue = new Button();
            ButtonDeleteValue.Click += new EventHandler(ButtonDeleteValue_Click);
            ButtonForget = new Button();
            ButtonForget.Click += new EventHandler(ButtonForget_Click);
            ButtonEdit = new Button();
            ButtonEdit.Click += new EventHandler(ButtonEdit_Click);
            ButtonImport = new Button();
            ButtonImport.Click += new EventHandler(ButtonImport_Click);
            ButtonExport = new Button();
            ButtonExport.Click += new EventHandler(ButtonExport_Click);
            SuspendLayout();
            // 
            // LsvPol
            // 
            LsvPol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            LsvPol.Columns.AddRange(new ColumnHeader[] { ChItem, ChValue });
            LsvPol.FullRowSelect = true;
            LsvPol.HideSelection = false;
            LsvPol.Location = new Point(12, 41);
            LsvPol.MultiSelect = false;
            LsvPol.Name = "LsvPol";
            LsvPol.ShowItemToolTips = true;
            LsvPol.Size = new Size(555, 235);
            LsvPol.TabIndex = 0;
            LsvPol.UseCompatibleStateImageBehavior = false;
            LsvPol.View = View.Details;
            // 
            // ChItem
            // 
            ChItem.Text = "Name";
            ChItem.Width = 377;
            // 
            // ChValue
            // 
            ChValue.Text = "Value";
            ChValue.Width = 160;
            // 
            // ButtonSave
            // 
            ButtonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonSave.Location = new Point(492, 282);
            ButtonSave.Name = "ButtonSave";
            ButtonSave.Size = new Size(75, 23);
            ButtonSave.TabIndex = 11;
            ButtonSave.Text = "Done";
            ButtonSave.UseVisualStyleBackColor = true;
            // 
            // ButtonAddKey
            // 
            ButtonAddKey.Location = new Point(12, 12);
            ButtonAddKey.Name = "ButtonAddKey";
            ButtonAddKey.Size = new Size(75, 23);
            ButtonAddKey.TabIndex = 3;
            ButtonAddKey.Text = "Add Key";
            ButtonAddKey.UseVisualStyleBackColor = true;
            // 
            // ButtonAddValue
            // 
            ButtonAddValue.Location = new Point(93, 12);
            ButtonAddValue.Name = "ButtonAddValue";
            ButtonAddValue.Size = new Size(87, 23);
            ButtonAddValue.TabIndex = 4;
            ButtonAddValue.Text = "Add Value";
            ButtonAddValue.UseVisualStyleBackColor = true;
            // 
            // ButtonDeleteValue
            // 
            ButtonDeleteValue.Location = new Point(186, 12);
            ButtonDeleteValue.Name = "ButtonDeleteValue";
            ButtonDeleteValue.Size = new Size(100, 23);
            ButtonDeleteValue.TabIndex = 5;
            ButtonDeleteValue.Text = "Delete Value(s)";
            ButtonDeleteValue.UseVisualStyleBackColor = true;
            // 
            // ButtonForget
            // 
            ButtonForget.Location = new Point(292, 12);
            ButtonForget.Name = "ButtonForget";
            ButtonForget.Size = new Size(75, 23);
            ButtonForget.TabIndex = 6;
            ButtonForget.Text = "Forget";
            ButtonForget.UseVisualStyleBackColor = true;
            // 
            // ButtonEdit
            // 
            ButtonEdit.Location = new Point(373, 12);
            ButtonEdit.Name = "ButtonEdit";
            ButtonEdit.Size = new Size(75, 23);
            ButtonEdit.TabIndex = 7;
            ButtonEdit.Text = "Edit";
            ButtonEdit.UseVisualStyleBackColor = true;
            // 
            // ButtonImport
            // 
            ButtonImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ButtonImport.Location = new Point(12, 282);
            ButtonImport.Name = "ButtonImport";
            ButtonImport.Size = new Size(75, 23);
            ButtonImport.TabIndex = 9;
            ButtonImport.Text = "Import REG";
            ButtonImport.UseVisualStyleBackColor = true;
            // 
            // ButtonExport
            // 
            ButtonExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ButtonExport.Location = new Point(93, 282);
            ButtonExport.Name = "ButtonExport";
            ButtonExport.Size = new Size(75, 23);
            ButtonExport.TabIndex = 10;
            ButtonExport.Text = "Export REG";
            ButtonExport.UseVisualStyleBackColor = true;
            // 
            // EditPol
            // 
            AcceptButton = ButtonSave;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(579, 317);
            Controls.Add(ButtonExport);
            Controls.Add(ButtonImport);
            Controls.Add(ButtonEdit);
            Controls.Add(ButtonForget);
            Controls.Add(ButtonDeleteValue);
            Controls.Add(ButtonAddValue);
            Controls.Add(ButtonAddKey);
            Controls.Add(ButtonSave);
            Controls.Add(LsvPol);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditPol";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Raw POL";
            KeyDown += new KeyEventHandler(EditPol_KeyDown);
            ResumeLayout(false);

        }

        internal ListView LsvPol;
        internal Button ButtonSave;
        internal ColumnHeader ChItem;
        internal ColumnHeader ChValue;
        internal Button ButtonAddKey;
        internal Button ButtonAddValue;
        internal Button ButtonDeleteValue;
        internal Button ButtonForget;
        internal Button ButtonEdit;
        internal Button ButtonImport;
        internal Button ButtonExport;
    }
}