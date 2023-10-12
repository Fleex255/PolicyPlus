using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FindByRegistry : Form
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
            Label KeyPathLabel;
            Label ValueLabel;
            KeyTextbox = new TextBox();
            ValueTextbox = new TextBox();
            SearchButton = new Button();
            SearchButton.Click += new EventHandler(SearchButton_Click);
            KeyPathLabel = new Label();
            ValueLabel = new Label();
            SuspendLayout();
            // 
            // KeyPathLabel
            // 
            KeyPathLabel.AutoSize = true;
            KeyPathLabel.Location = new Point(12, 15);
            KeyPathLabel.Name = "KeyPathLabel";
            KeyPathLabel.Size = new Size(90, 13);
            KeyPathLabel.TabIndex = 0;
            KeyPathLabel.Text = "Key path or name";
            // 
            // ValueLabel
            // 
            ValueLabel.AutoSize = true;
            ValueLabel.Location = new Point(12, 41);
            ValueLabel.Name = "ValueLabel";
            ValueLabel.Size = new Size(63, 13);
            ValueLabel.TabIndex = 2;
            ValueLabel.Text = "Value name";
            // 
            // KeyTextbox
            // 
            KeyTextbox.Location = new Point(108, 12);
            KeyTextbox.Name = "KeyTextbox";
            KeyTextbox.Size = new Size(260, 20);
            KeyTextbox.TabIndex = 1;
            // 
            // ValueTextbox
            // 
            ValueTextbox.Location = new Point(108, 38);
            ValueTextbox.Name = "ValueTextbox";
            ValueTextbox.Size = new Size(260, 20);
            ValueTextbox.TabIndex = 2;
            // 
            // SearchButton
            // 
            SearchButton.Location = new Point(293, 64);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 23);
            SearchButton.TabIndex = 3;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = true;
            // 
            // FindByRegistry
            // 
            AcceptButton = SearchButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 99);
            Controls.Add(SearchButton);
            Controls.Add(ValueLabel);
            Controls.Add(ValueTextbox);
            Controls.Add(KeyTextbox);
            Controls.Add(KeyPathLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FindByRegistry";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Find by Registry";
            KeyUp += new KeyEventHandler(FindByRegistry_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox KeyTextbox;
        internal TextBox ValueTextbox;
        internal Button SearchButton;
    }
}