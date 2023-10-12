using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FindById : Form
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
            StatusImage = new PictureBox();
            IdTextbox = new TextBox();
            IdTextbox.TextChanged += new EventHandler(IdTextbox_TextChanged);
            GoButton = new Button();
            GoButton.Click += new EventHandler(GoButton_Click);
            ((System.ComponentModel.ISupportInitialize)StatusImage).BeginInit();
            SuspendLayout();
            // 
            // StatusImage
            // 
            StatusImage.Location = new Point(12, 14);
            StatusImage.Name = "StatusImage";
            StatusImage.Size = new Size(16, 16);
            StatusImage.TabIndex = 0;
            StatusImage.TabStop = false;
            // 
            // IdTextbox
            // 
            IdTextbox.Location = new Point(34, 12);
            IdTextbox.Name = "IdTextbox";
            IdTextbox.Size = new Size(277, 20);
            IdTextbox.TabIndex = 1;
            IdTextbox.Text = " ";
            // 
            // GoButton
            // 
            GoButton.Location = new Point(236, 38);
            GoButton.Name = "GoButton";
            GoButton.Size = new Size(75, 23);
            GoButton.TabIndex = 2;
            GoButton.Text = "Go";
            GoButton.UseVisualStyleBackColor = true;
            // 
            // FindById
            // 
            AcceptButton = GoButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(323, 73);
            Controls.Add(GoButton);
            Controls.Add(IdTextbox);
            Controls.Add(StatusImage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FindById";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Find by ID";
            ((System.ComponentModel.ISupportInitialize)StatusImage).EndInit();
            Load += new EventHandler(FindById_Load);
            Shown += new EventHandler(FindById_Shown);
            KeyUp += new KeyEventHandler(FindById_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal PictureBox StatusImage;
        internal TextBox IdTextbox;
        internal Button GoButton;
    }
}