<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditSetting
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
        Dim CommentLabel As System.Windows.Forms.Label
        Dim SupportedLabel As System.Windows.Forms.Label
        Dim SectionLabel As System.Windows.Forms.Label
        Me.SettingNameLabel = New System.Windows.Forms.Label()
        Me.CommentTextbox = New System.Windows.Forms.TextBox()
        Me.SupportedTextbox = New System.Windows.Forms.TextBox()
        Me.NotConfiguredOption = New System.Windows.Forms.RadioButton()
        Me.EnabledOption = New System.Windows.Forms.RadioButton()
        Me.DisabledOption = New System.Windows.Forms.RadioButton()
        Me.ExtraOptionsPanel = New System.Windows.Forms.Panel()
        Me.ExtraOptionsTable = New System.Windows.Forms.TableLayoutPanel()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.HelpTextbox = New System.Windows.Forms.TextBox()
        Me.SectionDropdown = New System.Windows.Forms.ComboBox()
        Me.ApplyButton = New System.Windows.Forms.Button()
        CommentLabel = New System.Windows.Forms.Label()
        SupportedLabel = New System.Windows.Forms.Label()
        SectionLabel = New System.Windows.Forms.Label()
        Me.ExtraOptionsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommentLabel
        '
        CommentLabel.AutoSize = True
        CommentLabel.Location = New System.Drawing.Point(260, 28)
        CommentLabel.Name = "CommentLabel"
        CommentLabel.Size = New System.Drawing.Size(51, 13)
        CommentLabel.TabIndex = 2
        CommentLabel.Text = "Comment"
        '
        'SupportedLabel
        '
        SupportedLabel.AutoSize = True
        SupportedLabel.Location = New System.Drawing.Point(240, 103)
        SupportedLabel.Name = "SupportedLabel"
        SupportedLabel.Size = New System.Drawing.Size(71, 13)
        SupportedLabel.TabIndex = 4
        SupportedLabel.Text = "Supported on"
        '
        'SectionLabel
        '
        SectionLabel.AutoSize = True
        SectionLabel.Location = New System.Drawing.Point(12, 28)
        SectionLabel.Name = "SectionLabel"
        SectionLabel.Size = New System.Drawing.Size(54, 13)
        SectionLabel.TabIndex = 12
        SectionLabel.Text = "Editing for"
        '
        'SettingNameLabel
        '
        Me.SettingNameLabel.AutoEllipsis = True
        Me.SettingNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.SettingNameLabel.Name = "SettingNameLabel"
        Me.SettingNameLabel.Size = New System.Drawing.Size(614, 13)
        Me.SettingNameLabel.TabIndex = 0
        Me.SettingNameLabel.Text = "Policy name"
        '
        'CommentTextbox
        '
        Me.CommentTextbox.AcceptsReturn = True
        Me.CommentTextbox.Location = New System.Drawing.Point(317, 25)
        Me.CommentTextbox.Multiline = True
        Me.CommentTextbox.Name = "CommentTextbox"
        Me.CommentTextbox.Size = New System.Drawing.Size(309, 69)
        Me.CommentTextbox.TabIndex = 100
        '
        'SupportedTextbox
        '
        Me.SupportedTextbox.Location = New System.Drawing.Point(317, 100)
        Me.SupportedTextbox.Multiline = True
        Me.SupportedTextbox.Name = "SupportedTextbox"
        Me.SupportedTextbox.ReadOnly = True
        Me.SupportedTextbox.Size = New System.Drawing.Size(309, 44)
        Me.SupportedTextbox.TabIndex = 101
        '
        'NotConfiguredOption
        '
        Me.NotConfiguredOption.AutoSize = True
        Me.NotConfiguredOption.Location = New System.Drawing.Point(12, 52)
        Me.NotConfiguredOption.Name = "NotConfiguredOption"
        Me.NotConfiguredOption.Size = New System.Drawing.Size(96, 17)
        Me.NotConfiguredOption.TabIndex = 1
        Me.NotConfiguredOption.TabStop = True
        Me.NotConfiguredOption.Text = "Not Configured"
        Me.NotConfiguredOption.UseVisualStyleBackColor = True
        '
        'EnabledOption
        '
        Me.EnabledOption.AutoSize = True
        Me.EnabledOption.Location = New System.Drawing.Point(12, 75)
        Me.EnabledOption.Name = "EnabledOption"
        Me.EnabledOption.Size = New System.Drawing.Size(64, 17)
        Me.EnabledOption.TabIndex = 2
        Me.EnabledOption.TabStop = True
        Me.EnabledOption.Text = "Enabled"
        Me.EnabledOption.UseVisualStyleBackColor = True
        '
        'DisabledOption
        '
        Me.DisabledOption.AutoSize = True
        Me.DisabledOption.Location = New System.Drawing.Point(12, 98)
        Me.DisabledOption.Name = "DisabledOption"
        Me.DisabledOption.Size = New System.Drawing.Size(66, 17)
        Me.DisabledOption.TabIndex = 3
        Me.DisabledOption.TabStop = True
        Me.DisabledOption.Text = "Disabled"
        Me.DisabledOption.UseVisualStyleBackColor = True
        '
        'ExtraOptionsPanel
        '
        Me.ExtraOptionsPanel.AutoScroll = True
        Me.ExtraOptionsPanel.BackColor = System.Drawing.Color.White
        Me.ExtraOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ExtraOptionsPanel.Controls.Add(Me.ExtraOptionsTable)
        Me.ExtraOptionsPanel.Location = New System.Drawing.Point(12, 150)
        Me.ExtraOptionsPanel.Name = "ExtraOptionsPanel"
        Me.ExtraOptionsPanel.Size = New System.Drawing.Size(299, 244)
        Me.ExtraOptionsPanel.TabIndex = 8
        '
        'ExtraOptionsTable
        '
        Me.ExtraOptionsTable.AutoSize = True
        Me.ExtraOptionsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ExtraOptionsTable.ColumnCount = 1
        Me.ExtraOptionsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 298.0!))
        Me.ExtraOptionsTable.Location = New System.Drawing.Point(0, 0)
        Me.ExtraOptionsTable.Margin = New System.Windows.Forms.Padding(0)
        Me.ExtraOptionsTable.MaximumSize = New System.Drawing.Size(297, 0)
        Me.ExtraOptionsTable.MinimumSize = New System.Drawing.Size(297, 0)
        Me.ExtraOptionsTable.Name = "ExtraOptionsTable"
        Me.ExtraOptionsTable.RowCount = 1
        Me.ExtraOptionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.ExtraOptionsTable.Size = New System.Drawing.Size(297, 20)
        Me.ExtraOptionsTable.TabIndex = 0
        '
        'CloseButton
        '
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(470, 400)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 104
        Me.CloseButton.Text = "Cancel"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(389, 400)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 103
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'HelpTextbox
        '
        Me.HelpTextbox.Location = New System.Drawing.Point(317, 150)
        Me.HelpTextbox.Multiline = True
        Me.HelpTextbox.Name = "HelpTextbox"
        Me.HelpTextbox.ReadOnly = True
        Me.HelpTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.HelpTextbox.Size = New System.Drawing.Size(309, 244)
        Me.HelpTextbox.TabIndex = 102
        '
        'SectionDropdown
        '
        Me.SectionDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SectionDropdown.FormattingEnabled = True
        Me.SectionDropdown.Items.AddRange(New Object() {"User", "Computer"})
        Me.SectionDropdown.Location = New System.Drawing.Point(72, 25)
        Me.SectionDropdown.Name = "SectionDropdown"
        Me.SectionDropdown.Size = New System.Drawing.Size(112, 21)
        Me.SectionDropdown.TabIndex = 4
        '
        'ApplyButton
        '
        Me.ApplyButton.Location = New System.Drawing.Point(551, 400)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(75, 23)
        Me.ApplyButton.TabIndex = 105
        Me.ApplyButton.Text = "Apply"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'EditSetting
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(638, 435)
        Me.Controls.Add(Me.ApplyButton)
        Me.Controls.Add(SectionLabel)
        Me.Controls.Add(Me.SectionDropdown)
        Me.Controls.Add(Me.HelpTextbox)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ExtraOptionsPanel)
        Me.Controls.Add(Me.DisabledOption)
        Me.Controls.Add(Me.EnabledOption)
        Me.Controls.Add(Me.NotConfiguredOption)
        Me.Controls.Add(SupportedLabel)
        Me.Controls.Add(Me.SupportedTextbox)
        Me.Controls.Add(CommentLabel)
        Me.Controls.Add(Me.CommentTextbox)
        Me.Controls.Add(Me.SettingNameLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditSetting"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Policy Setting"
        Me.ExtraOptionsPanel.ResumeLayout(False)
        Me.ExtraOptionsPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SettingNameLabel As Label
    Friend WithEvents CommentTextbox As TextBox
    Friend WithEvents SupportedTextbox As TextBox
    Friend WithEvents NotConfiguredOption As RadioButton
    Friend WithEvents EnabledOption As RadioButton
    Friend WithEvents DisabledOption As RadioButton
    Friend WithEvents ExtraOptionsPanel As Panel
    Friend WithEvents ExtraOptionsTable As TableLayoutPanel
    Friend WithEvents CloseButton As Button
    Friend WithEvents OkButton As Button
    Friend WithEvents HelpTextbox As TextBox
    Friend WithEvents SectionDropdown As ComboBox
    Friend WithEvents ApplyButton As Button
End Class
