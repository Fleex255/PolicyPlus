<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindResults
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SearchProgress = New System.Windows.Forms.ProgressBar()
        Me.ResultsListview = New System.Windows.Forms.ListView()
        Me.ChTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChCategory = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ProgressLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.GoButton = New System.Windows.Forms.Button()
        Me.StopButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SearchProgress
        '
        Me.SearchProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchProgress.Location = New System.Drawing.Point(12, 25)
        Me.SearchProgress.Name = "SearchProgress"
        Me.SearchProgress.Size = New System.Drawing.Size(295, 23)
        Me.SearchProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.SearchProgress.TabIndex = 0
        '
        'ResultsListview
        '
        Me.ResultsListview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResultsListview.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChTitle, Me.ChCategory})
        Me.ResultsListview.FullRowSelect = True
        Me.ResultsListview.HideSelection = False
        Me.ResultsListview.Location = New System.Drawing.Point(12, 54)
        Me.ResultsListview.MultiSelect = False
        Me.ResultsListview.Name = "ResultsListview"
        Me.ResultsListview.ShowItemToolTips = True
        Me.ResultsListview.Size = New System.Drawing.Size(351, 105)
        Me.ResultsListview.TabIndex = 1
        Me.ResultsListview.UseCompatibleStateImageBehavior = False
        Me.ResultsListview.View = System.Windows.Forms.View.Details
        '
        'ChTitle
        '
        Me.ChTitle.Text = "Title"
        Me.ChTitle.Width = 222
        '
        'ChCategory
        '
        Me.ChCategory.Text = "Category"
        Me.ChCategory.Width = 99
        '
        'ProgressLabel
        '
        Me.ProgressLabel.AutoSize = True
        Me.ProgressLabel.Location = New System.Drawing.Point(12, 9)
        Me.ProgressLabel.Name = "ProgressLabel"
        Me.ProgressLabel.Size = New System.Drawing.Size(109, 13)
        Me.ProgressLabel.TabIndex = 2
        Me.ProgressLabel.Text = "Results: 0 (searching)"
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(288, 165)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'GoButton
        '
        Me.GoButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GoButton.Location = New System.Drawing.Point(207, 165)
        Me.GoButton.Name = "GoButton"
        Me.GoButton.Size = New System.Drawing.Size(75, 23)
        Me.GoButton.TabIndex = 3
        Me.GoButton.Text = "Go"
        Me.GoButton.UseVisualStyleBackColor = True
        '
        'StopButton
        '
        Me.StopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StopButton.Location = New System.Drawing.Point(313, 25)
        Me.StopButton.Name = "StopButton"
        Me.StopButton.Size = New System.Drawing.Size(50, 23)
        Me.StopButton.TabIndex = 0
        Me.StopButton.Text = "Stop"
        Me.StopButton.UseVisualStyleBackColor = True
        '
        'FindResults
        '
        Me.AcceptButton = Me.GoButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(375, 200)
        Me.Controls.Add(Me.StopButton)
        Me.Controls.Add(Me.GoButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ProgressLabel)
        Me.Controls.Add(Me.ResultsListview)
        Me.Controls.Add(Me.SearchProgress)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindResults"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Search Results"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SearchProgress As ProgressBar
    Friend WithEvents ResultsListview As ListView
    Friend WithEvents ChTitle As ColumnHeader
    Friend WithEvents ChCategory As ColumnHeader
    Friend WithEvents ProgressLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents GoButton As Button
    Friend WithEvents StopButton As Button
End Class
