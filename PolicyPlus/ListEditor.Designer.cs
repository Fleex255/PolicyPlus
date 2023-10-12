using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ListEditor : Form
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
            EntriesDatagrid = new DataGridView();
            NameColumn = new DataGridViewTextBoxColumn();
            ValueColumn = new DataGridViewTextBoxColumn();
            ElementNameLabel = new Label();
            CloseButton = new Button();
            OkButton = new Button();
            OkButton.Click += new EventHandler(OkButton_Click);
            ((System.ComponentModel.ISupportInitialize)EntriesDatagrid).BeginInit();
            SuspendLayout();
            // 
            // EntriesDatagrid
            // 
            EntriesDatagrid.AllowUserToResizeRows = false;
            EntriesDatagrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            EntriesDatagrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            EntriesDatagrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            EntriesDatagrid.Columns.AddRange(new DataGridViewColumn[] { NameColumn, ValueColumn });
            EntriesDatagrid.Location = new Point(12, 25);
            EntriesDatagrid.Name = "EntriesDatagrid";
            EntriesDatagrid.Size = new Size(404, 170);
            EntriesDatagrid.TabIndex = 0;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Name";
            NameColumn.Name = "NameColumn";
            // 
            // ValueColumn
            // 
            ValueColumn.HeaderText = "Value";
            ValueColumn.Name = "ValueColumn";
            // 
            // ElementNameLabel
            // 
            ElementNameLabel.AutoEllipsis = true;
            ElementNameLabel.AutoSize = true;
            ElementNameLabel.Location = new Point(12, 9);
            ElementNameLabel.MaximumSize = new Size(400, 0);
            ElementNameLabel.Name = "ElementNameLabel";
            ElementNameLabel.Size = new Size(92, 13);
            ElementNameLabel.TabIndex = 1;
            ElementNameLabel.Text = "List element name";
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.DialogResult = DialogResult.Cancel;
            CloseButton.Location = new Point(341, 201);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 2;
            CloseButton.Text = "Cancel";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkButton.Location = new Point(260, 201);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 1;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            // 
            // ListEditor
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(428, 236);
            Controls.Add(OkButton);
            Controls.Add(CloseButton);
            Controls.Add(ElementNameLabel);
            Controls.Add(EntriesDatagrid);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(444, 275);
            Name = "ListEditor";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit List";
            ((System.ComponentModel.ISupportInitialize)EntriesDatagrid).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        internal DataGridView EntriesDatagrid;
        internal DataGridViewTextBoxColumn NameColumn;
        internal DataGridViewTextBoxColumn ValueColumn;
        internal Label ElementNameLabel;
        internal Button CloseButton;
        internal Button OkButton;
    }
}