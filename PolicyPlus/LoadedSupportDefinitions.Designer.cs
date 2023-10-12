using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class LoadedSupportDefinitions : Form
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
            LsvSupport = new ListView();
            LsvSupport.DoubleClick += new EventHandler(LsvSupport_DoubleClick);
            LsvSupport.KeyDown += new KeyEventHandler(LsvSupport_KeyDown);
            ChName = new ColumnHeader();
            ChDefinedIn = new ColumnHeader();
            ButtonClose = new Button();
            TextFilter = new TextBox();
            TextFilter.TextChanged += new EventHandler(TextFilter_TextChanged);
            Label1 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(79, 13);
            Label1.TabIndex = 3;
            Label1.Text = "Substring (filter)";
            // 
            // LsvSupport
            // 
            LsvSupport.Columns.AddRange(new ColumnHeader[] { ChName, ChDefinedIn });
            LsvSupport.FullRowSelect = true;
            LsvSupport.Location = new Point(12, 38);
            LsvSupport.MultiSelect = false;
            LsvSupport.Name = "LsvSupport";
            LsvSupport.ShowItemToolTips = true;
            LsvSupport.Size = new Size(435, 190);
            LsvSupport.TabIndex = 2;
            LsvSupport.UseCompatibleStateImageBehavior = false;
            LsvSupport.View = View.Details;
            // 
            // ChName
            // 
            ChName.Text = "Name";
            ChName.Width = 317;
            // 
            // ChDefinedIn
            // 
            ChDefinedIn.Text = "ADMX File";
            ChDefinedIn.Width = 97;
            // 
            // ButtonClose
            // 
            ButtonClose.DialogResult = DialogResult.OK;
            ButtonClose.Location = new Point(372, 234);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 3;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // TextFilter
            // 
            TextFilter.Location = new Point(97, 12);
            TextFilter.Name = "TextFilter";
            TextFilter.Size = new Size(350, 20);
            TextFilter.TabIndex = 1;
            // 
            // LoadedSupportDefinitions
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonClose;
            ClientSize = new Size(459, 269);
            Controls.Add(Label1);
            Controls.Add(TextFilter);
            Controls.Add(ButtonClose);
            Controls.Add(LsvSupport);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoadedSupportDefinitions";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "All Support Definitions";
            ResumeLayout(false);
            PerformLayout();

        }

        internal ListView LsvSupport;
        internal ColumnHeader ChName;
        internal ColumnHeader ChDefinedIn;
        internal Button ButtonClose;
        internal TextBox TextFilter;
    }
}