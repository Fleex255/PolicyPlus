using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PolicyPlus
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class FindResults : Form
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
            SearchProgress = new ProgressBar();
            ResultsListview = new ListView();
            ResultsListview.SizeChanged += new EventHandler(ResultsListview_SizeChanged);
            ResultsListview.DoubleClick += new EventHandler(GoClicked);
            ChTitle = new ColumnHeader();
            ChCategory = new ColumnHeader();
            ProgressLabel = new Label();
            CloseButton = new Button();
            GoButton = new Button();
            GoButton.Click += new EventHandler(GoClicked);
            StopButton = new Button();
            StopButton.Click += new EventHandler(StopButton_Click);
            SuspendLayout();
            // 
            // SearchProgress
            // 
            SearchProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SearchProgress.Location = new Point(12, 25);
            SearchProgress.Name = "SearchProgress";
            SearchProgress.Size = new Size(295, 23);
            SearchProgress.Style = ProgressBarStyle.Continuous;
            SearchProgress.TabIndex = 0;
            // 
            // ResultsListview
            // 
            ResultsListview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            ResultsListview.Columns.AddRange(new ColumnHeader[] { ChTitle, ChCategory });
            ResultsListview.FullRowSelect = true;
            ResultsListview.HideSelection = false;
            ResultsListview.Location = new Point(12, 54);
            ResultsListview.MultiSelect = false;
            ResultsListview.Name = "ResultsListview";
            ResultsListview.ShowItemToolTips = true;
            ResultsListview.Size = new Size(351, 105);
            ResultsListview.TabIndex = 1;
            ResultsListview.UseCompatibleStateImageBehavior = false;
            ResultsListview.View = View.Details;
            // 
            // ChTitle
            // 
            ChTitle.Text = "Title";
            ChTitle.Width = 222;
            // 
            // ChCategory
            // 
            ChCategory.Text = "Category";
            ChCategory.Width = 99;
            // 
            // ProgressLabel
            // 
            ProgressLabel.AutoSize = true;
            ProgressLabel.Location = new Point(12, 9);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(109, 13);
            ProgressLabel.TabIndex = 2;
            ProgressLabel.Text = "Results: 0 (searching)";
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.DialogResult = DialogResult.Cancel;
            CloseButton.Location = new Point(288, 165);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 23);
            CloseButton.TabIndex = 4;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            // 
            // GoButton
            // 
            GoButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            GoButton.Location = new Point(207, 165);
            GoButton.Name = "GoButton";
            GoButton.Size = new Size(75, 23);
            GoButton.TabIndex = 3;
            GoButton.Text = "Go";
            GoButton.UseVisualStyleBackColor = true;
            // 
            // StopButton
            // 
            StopButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StopButton.Location = new Point(313, 25);
            StopButton.Name = "StopButton";
            StopButton.Size = new Size(50, 23);
            StopButton.TabIndex = 0;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            // 
            // FindResults
            // 
            AcceptButton = GoButton;
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(375, 200);
            Controls.Add(StopButton);
            Controls.Add(GoButton);
            Controls.Add(CloseButton);
            Controls.Add(ProgressLabel);
            Controls.Add(ResultsListview);
            Controls.Add(SearchProgress);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FindResults";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Search Results";
            Shown += new EventHandler(FindResults_Shown);
            Closing += new System.ComponentModel.CancelEventHandler(FindResults_Closing);
            Load += new EventHandler(FindResults_Load);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ProgressBar SearchProgress;
        internal ListView ResultsListview;
        internal ColumnHeader ChTitle;
        internal ColumnHeader ChCategory;
        internal Label ProgressLabel;
        internal Button CloseButton;
        internal Button GoButton;
        internal Button StopButton;
    }
}