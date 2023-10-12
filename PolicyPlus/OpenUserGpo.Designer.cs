using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class OpenUserGpo : Form
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
            UsernameTextbox = new TextBox();
            SearchButton = new Button();
            SearchButton.Click += new EventHandler(SearchButton_Click);
            SidTextbox = new TextBox();
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            Label1 = new Label();
            Label2 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(12, 15);
            Label1.Name = "Label1";
            Label1.Size = new Size(95, 13);
            Label1.TabIndex = 0;
            Label1.Text = "Look up username";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(12, 41);
            Label2.Name = "Label2";
            Label2.Size = new Size(25, 13);
            Label2.TabIndex = 3;
            Label2.Text = "SID";
            // 
            // UsernameTextbox
            // 
            UsernameTextbox.Location = new Point(113, 12);
            UsernameTextbox.Name = "UsernameTextbox";
            UsernameTextbox.Size = new Size(153, 20);
            UsernameTextbox.TabIndex = 1;
            // 
            // SearchButton
            // 
            SearchButton.Location = new Point(272, 10);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(57, 23);
            SearchButton.TabIndex = 2;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = true;
            // 
            // SidTextbox
            // 
            SidTextbox.Location = new Point(43, 38);
            SidTextbox.Name = "SidTextbox";
            SidTextbox.Size = new Size(286, 20);
            SidTextbox.TabIndex = 3;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(254, 64);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 4;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // OpenUserGpo
            // 
            AcceptButton = OkButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(341, 99);
            Controls.Add(OkButton);
            Controls.Add(SidTextbox);
            Controls.Add(Label2);
            Controls.Add(SearchButton);
            Controls.Add(UsernameTextbox);
            Controls.Add(Label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OpenUserGpo";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select User SID";
            Shown += new EventHandler(OpenUserGpo_Shown);
            KeyUp += new KeyEventHandler(OpenUserGpo_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox UsernameTextbox;
        internal Button SearchButton;
        internal TextBox SidTextbox;
        internal Button OkButton;
    }
}