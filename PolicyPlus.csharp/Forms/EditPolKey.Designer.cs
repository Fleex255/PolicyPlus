﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus.csharp.UI
{

    public partial class EditPolKey : System.Windows.Forms.Form
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
#pragma warning disable CS0649 // Field 'EditPolKey.components' is never assigned to, and will always have its default value null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Field 'EditPolKey.components' is never assigned to, and will always have its default value null

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            Label Label1;
            TextName = new TextBox();
            ButtonOK = new Button();
            Label1 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(35, 13);
            Label1.TabIndex = 0;
            Label1.Text = "Name";
            // 
            // TextName
            // 
            TextName.Location = new Point(53, 12);
            TextName.Name = "TextName";
            TextName.Size = new Size(223, 20);
            TextName.TabIndex = 1;
            // 
            // ButtonOK
            // 
            ButtonOK.DialogResult = DialogResult.OK;
            ButtonOK.Location = new Point(201, 38);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new Size(75, 23);
            ButtonOK.TabIndex = 2;
            ButtonOK.Text = "OK";
            ButtonOK.UseVisualStyleBackColor = true;
            // 
            // EditPolKey
            // 
            AcceptButton = ButtonOK;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(288, 73);
            Controls.Add(ButtonOK);
            Controls.Add(TextName);
            Controls.Add(Label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditPolKey";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Key";
            KeyDown += new KeyEventHandler(EditPolKey_KeyDown);
            Shown += new EventHandler(EditPolKey_Shown);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox TextName;
        internal Button ButtonOK;
    }
}