<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FilterOptions
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
        Dim PolicyTypeLabel As System.Windows.Forms.Label
        Dim PolicyStateLabel As System.Windows.Forms.Label
        Dim CommentedLabel As System.Windows.Forms.Label
        Me.PolicyTypeCombobox = New System.Windows.Forms.ComboBox()
        Me.PolicyStateCombobox = New System.Windows.Forms.ComboBox()
        Me.CommentedCombobox = New System.Windows.Forms.ComboBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.ResetButton = New System.Windows.Forms.Button()
        Me.RequirementsBox = New System.Windows.Forms.GroupBox()
        Me.AllowedProductsTreeview = New PolicyPlus.DoubleClickIgnoringTreeView()
        Me.MatchBlankSupportCheckbox = New System.Windows.Forms.CheckBox()
        Me.AlwaysMatchAnyCheckbox = New System.Windows.Forms.CheckBox()
        Me.SupportedCheckbox = New System.Windows.Forms.CheckBox()
        PolicyTypeLabel = New System.Windows.Forms.Label()
        PolicyStateLabel = New System.Windows.Forms.Label()
        CommentedLabel = New System.Windows.Forms.Label()
        Me.RequirementsBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'PolicyTypeLabel
        '
        PolicyTypeLabel.AutoSize = True
        PolicyTypeLabel.Location = New System.Drawing.Point(12, 9)
        PolicyTypeLabel.Name = "PolicyTypeLabel"
        PolicyTypeLabel.Size = New System.Drawing.Size(58, 13)
        PolicyTypeLabel.TabIndex = 1
        PolicyTypeLabel.Text = "Policy type"
        '
        'PolicyStateLabel
        '
        PolicyStateLabel.AutoSize = True
        PolicyStateLabel.Location = New System.Drawing.Point(121, 9)
        PolicyStateLabel.Name = "PolicyStateLabel"
        PolicyStateLabel.Size = New System.Drawing.Size(67, 13)
        PolicyStateLabel.TabIndex = 3
        PolicyStateLabel.Text = "Current state"
        '
        'CommentedLabel
        '
        CommentedLabel.AutoSize = True
        CommentedLabel.Location = New System.Drawing.Point(230, 9)
        CommentedLabel.Name = "CommentedLabel"
        CommentedLabel.Size = New System.Drawing.Size(63, 13)
        CommentedLabel.TabIndex = 5
        CommentedLabel.Text = "Commented"
        '
        'PolicyTypeCombobox
        '
        Me.PolicyTypeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PolicyTypeCombobox.FormattingEnabled = True
        Me.PolicyTypeCombobox.Items.AddRange(New Object() {"Any", "Policy", "Preference"})
        Me.PolicyTypeCombobox.Location = New System.Drawing.Point(12, 25)
        Me.PolicyTypeCombobox.Name = "PolicyTypeCombobox"
        Me.PolicyTypeCombobox.Size = New System.Drawing.Size(103, 21)
        Me.PolicyTypeCombobox.TabIndex = 0
        '
        'PolicyStateCombobox
        '
        Me.PolicyStateCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PolicyStateCombobox.FormattingEnabled = True
        Me.PolicyStateCombobox.Items.AddRange(New Object() {"Any", "Not Configured", "Configured", "Enabled", "Disabled"})
        Me.PolicyStateCombobox.Location = New System.Drawing.Point(121, 25)
        Me.PolicyStateCombobox.Name = "PolicyStateCombobox"
        Me.PolicyStateCombobox.Size = New System.Drawing.Size(103, 21)
        Me.PolicyStateCombobox.TabIndex = 2
        '
        'CommentedCombobox
        '
        Me.CommentedCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CommentedCombobox.FormattingEnabled = True
        Me.CommentedCombobox.Items.AddRange(New Object() {"Any", "Yes", "No"})
        Me.CommentedCombobox.Location = New System.Drawing.Point(230, 25)
        Me.CommentedCombobox.Name = "CommentedCombobox"
        Me.CommentedCombobox.Size = New System.Drawing.Size(103, 21)
        Me.CommentedCombobox.TabIndex = 4
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkButton.Location = New System.Drawing.Point(258, 311)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 6
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResetButton.Location = New System.Drawing.Point(12, 311)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(75, 23)
        Me.ResetButton.TabIndex = 7
        Me.ResetButton.Text = "Reset"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'RequirementsBox
        '
        Me.RequirementsBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RequirementsBox.Controls.Add(Me.AllowedProductsTreeview)
        Me.RequirementsBox.Controls.Add(Me.MatchBlankSupportCheckbox)
        Me.RequirementsBox.Controls.Add(Me.AlwaysMatchAnyCheckbox)
        Me.RequirementsBox.Enabled = False
        Me.RequirementsBox.Location = New System.Drawing.Point(12, 52)
        Me.RequirementsBox.Name = "RequirementsBox"
        Me.RequirementsBox.Size = New System.Drawing.Size(321, 253)
        Me.RequirementsBox.TabIndex = 8
        Me.RequirementsBox.TabStop = False
        '
        'AllowedProductsTreeview
        '
        Me.AllowedProductsTreeview.CheckBoxes = True
        Me.AllowedProductsTreeview.FullRowSelect = True
        Me.AllowedProductsTreeview.HideSelection = False
        Me.AllowedProductsTreeview.Location = New System.Drawing.Point(6, 69)
        Me.AllowedProductsTreeview.Name = "AllowedProductsTreeview"
        Me.AllowedProductsTreeview.ShowNodeToolTips = True
        Me.AllowedProductsTreeview.Size = New System.Drawing.Size(309, 178)
        Me.AllowedProductsTreeview.TabIndex = 10
        '
        'MatchBlankSupportCheckbox
        '
        Me.MatchBlankSupportCheckbox.AutoSize = True
        Me.MatchBlankSupportCheckbox.Checked = True
        Me.MatchBlankSupportCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MatchBlankSupportCheckbox.Location = New System.Drawing.Point(6, 46)
        Me.MatchBlankSupportCheckbox.Name = "MatchBlankSupportCheckbox"
        Me.MatchBlankSupportCheckbox.Size = New System.Drawing.Size(282, 17)
        Me.MatchBlankSupportCheckbox.TabIndex = 0
        Me.MatchBlankSupportCheckbox.Text = "Match policies with missing or blank support definitions"
        Me.MatchBlankSupportCheckbox.UseVisualStyleBackColor = True
        '
        'AlwaysMatchAnyCheckbox
        '
        Me.AlwaysMatchAnyCheckbox.AutoSize = True
        Me.AlwaysMatchAnyCheckbox.Checked = True
        Me.AlwaysMatchAnyCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AlwaysMatchAnyCheckbox.Location = New System.Drawing.Point(6, 23)
        Me.AlwaysMatchAnyCheckbox.Name = "AlwaysMatchAnyCheckbox"
        Me.AlwaysMatchAnyCheckbox.Size = New System.Drawing.Size(303, 17)
        Me.AlwaysMatchAnyCheckbox.TabIndex = 0
        Me.AlwaysMatchAnyCheckbox.Text = "Match a policy if at least one selected product is supported"
        Me.AlwaysMatchAnyCheckbox.UseVisualStyleBackColor = True
        '
        'SupportedCheckbox
        '
        Me.SupportedCheckbox.AutoSize = True
        Me.SupportedCheckbox.Location = New System.Drawing.Point(18, 52)
        Me.SupportedCheckbox.Name = "SupportedCheckbox"
        Me.SupportedCheckbox.Size = New System.Drawing.Size(90, 17)
        Me.SupportedCheckbox.TabIndex = 9
        Me.SupportedCheckbox.Text = "Supported on"
        Me.SupportedCheckbox.UseVisualStyleBackColor = True
        '
        'FilterOptions
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 346)
        Me.Controls.Add(Me.SupportedCheckbox)
        Me.Controls.Add(Me.RequirementsBox)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(CommentedLabel)
        Me.Controls.Add(Me.CommentedCombobox)
        Me.Controls.Add(PolicyStateLabel)
        Me.Controls.Add(Me.PolicyStateCombobox)
        Me.Controls.Add(PolicyTypeLabel)
        Me.Controls.Add(Me.PolicyTypeCombobox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FilterOptions"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Filter Options"
        Me.RequirementsBox.ResumeLayout(False)
        Me.RequirementsBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PolicyTypeCombobox As ComboBox
    Friend WithEvents PolicyStateCombobox As ComboBox
    Friend WithEvents CommentedCombobox As ComboBox
    Friend WithEvents OkButton As Button
    Friend WithEvents ResetButton As Button
    Friend WithEvents RequirementsBox As GroupBox
    Friend WithEvents SupportedCheckbox As CheckBox
    Friend WithEvents AllowedProductsTreeview As TreeView
    Friend WithEvents MatchBlankSupportCheckbox As CheckBox
    Friend WithEvents AlwaysMatchAnyCheckbox As CheckBox
End Class
