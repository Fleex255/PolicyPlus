using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class OpenSection : Form
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
            OptUser = new RadioButton();
            OptComputer = new RadioButton();
            ButtonOK = new Button();
            ButtonOK.Click += new EventHandler(ButtonOK_Click);
            ButtonCancel = new Button();
            SuspendLayout();
            // 
            // OptUser
            // 
            OptUser.AutoSize = true;
            OptUser.Location = new Point(12, 12);
            OptUser.Name = "OptUser";
            OptUser.Size = new Size(47, 17);
            OptUser.TabIndex = 0;
            OptUser.TabStop = true;
            OptUser.Text = "User";
            OptUser.UseVisualStyleBackColor = true;
            // 
            // OptComputer
            // 
            OptComputer.AutoSize = true;
            OptComputer.Location = new Point(12, 35);
            OptComputer.Name = "OptComputer";
            OptComputer.Size = new Size(70, 17);
            OptComputer.TabIndex = 1;
            OptComputer.TabStop = true;
            OptComputer.Text = "Computer";
            OptComputer.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            ButtonOK.Location = new Point(72, 58);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(78, 23);
            ButtonOK.TabIndex = 2;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // ButtonCancel
            // 
            ButtonCancel.DialogResult = DialogResult.Cancel;
            ButtonCancel.Location = new Point(12, 58);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new Size(54, 23);
            ButtonCancel.TabIndex = 3;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // OpenSection
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ButtonCancel;
            ClientSize = new Size(162, 91);
            Controls.Add(ButtonCancel);
            Controls.Add(ButtonOK);
            Controls.Add(OptComputer);
            Controls.Add(OptUser);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenSection";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Section";
            KeyDown += new KeyEventHandler(OpenSection_KeyDown);
            Shown += new EventHandler(OpenSection_Shown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal RadioButton OptUser;
        internal RadioButton OptComputer;
        internal Button ButtonOK;
        internal Button ButtonCancel;
    }
}