using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class LoadedAdmx : Form
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
            LsvAdmx = new ListView();
            LsvAdmx.DoubleClick += new EventHandler(LsvAdmx_DoubleClick);
            LsvAdmx.KeyDown += new KeyEventHandler(LsvAdmx_KeyDown);
            ChFileTitle = new ColumnHeader();
            ChFolder = new ColumnHeader();
            ChNamespace = new ColumnHeader();
            ButtonClose = new Button();
            SuspendLayout();
            // 
            // LsvAdmx
            // 
            LsvAdmx.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            LsvAdmx.Columns.AddRange(new ColumnHeader[] { ChFileTitle, ChFolder, ChNamespace });
            LsvAdmx.FullRowSelect = true;
            LsvAdmx.HideSelection = false;
            LsvAdmx.Location = new Point(12, 12);
            LsvAdmx.MultiSelect = false;
            LsvAdmx.Name = "LsvAdmx";
            LsvAdmx.ShowItemToolTips = true;
            LsvAdmx.Size = new Size(487, 233);
            LsvAdmx.TabIndex = 0;
            LsvAdmx.UseCompatibleStateImageBehavior = false;
            LsvAdmx.View = View.Details;
            // 
            // ChFileTitle
            // 
            ChFileTitle.Text = "File";
            ChFileTitle.Width = 88;
            // 
            // ChFolder
            // 
            ChFolder.Text = "Folder";
            ChFolder.Width = 203;
            // 
            // ChNamespace
            // 
            ChNamespace.Text = "Namespace";
            ChNamespace.Width = 172;
            // 
            // ButtonClose
            // 
            ButtonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ButtonClose.DialogResult = DialogResult.OK;
            ButtonClose.Location = new Point(424, 251);
            ButtonClose.Name = "ButtonClose";
            ButtonClose.Size = new Size(75, 23);
            ButtonClose.TabIndex = 1;
            ButtonClose.Text = "Close";
            ButtonClose.UseVisualStyleBackColor = true;
            // 
            // LoadedAdmx
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonClose;
            ClientSize = new Size(511, 286);
            Controls.Add(ButtonClose);
            Controls.Add(LsvAdmx);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoadedAdmx";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Loaded ADMX Files";
            SizeChanged += new EventHandler(LoadedAdmx_SizeChanged);
            ResumeLayout(false);

        }

        internal ListView LsvAdmx;
        internal ColumnHeader ChFileTitle;
        internal ColumnHeader ChFolder;
        internal ColumnHeader ChNamespace;
        internal Button ButtonClose;
    }
}