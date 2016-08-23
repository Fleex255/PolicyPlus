<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class OpenPol
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ComputerGroup = New System.Windows.Forms.GroupBox()
        Me.CompNullOption = New System.Windows.Forms.RadioButton()
        Me.CompFileBrowseButton = New System.Windows.Forms.Button()
        Me.CompPolFilenameTextbox = New System.Windows.Forms.TextBox()
        Me.CompFileOption = New System.Windows.Forms.RadioButton()
        Me.CompRegistryOption = New System.Windows.Forms.RadioButton()
        Me.CompLocalOption = New System.Windows.Forms.RadioButton()
        Me.UserGroup = New System.Windows.Forms.GroupBox()
        Me.UserPerUserRegOption = New System.Windows.Forms.RadioButton()
        Me.UserPerUserGpoOption = New System.Windows.Forms.RadioButton()
        Me.UserBrowseHiveButton = New System.Windows.Forms.Button()
        Me.UserNullOption = New System.Windows.Forms.RadioButton()
        Me.UserBrowseGpoButton = New System.Windows.Forms.Button()
        Me.UserHivePathTextbox = New System.Windows.Forms.TextBox()
        Me.UserFileBrowseButton = New System.Windows.Forms.Button()
        Me.UserGpoSidTextbox = New System.Windows.Forms.TextBox()
        Me.UserPolFilenameTextbox = New System.Windows.Forms.TextBox()
        Me.UserFileOption = New System.Windows.Forms.RadioButton()
        Me.UserRegistryOption = New System.Windows.Forms.RadioButton()
        Me.UserLocalOption = New System.Windows.Forms.RadioButton()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CompRegTextbox = New System.Windows.Forms.TextBox()
        Me.UserRegTextbox = New System.Windows.Forms.TextBox()
        Me.ComputerGroup.SuspendLayout()
        Me.UserGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComputerGroup
        '
        Me.ComputerGroup.Controls.Add(Me.CompRegTextbox)
        Me.ComputerGroup.Controls.Add(Me.CompNullOption)
        Me.ComputerGroup.Controls.Add(Me.CompFileBrowseButton)
        Me.ComputerGroup.Controls.Add(Me.CompPolFilenameTextbox)
        Me.ComputerGroup.Controls.Add(Me.CompFileOption)
        Me.ComputerGroup.Controls.Add(Me.CompRegistryOption)
        Me.ComputerGroup.Controls.Add(Me.CompLocalOption)
        Me.ComputerGroup.Location = New System.Drawing.Point(12, 12)
        Me.ComputerGroup.Name = "ComputerGroup"
        Me.ComputerGroup.Size = New System.Drawing.Size(224, 111)
        Me.ComputerGroup.TabIndex = 0
        Me.ComputerGroup.TabStop = False
        Me.ComputerGroup.Text = "Computer"
        '
        'CompNullOption
        '
        Me.CompNullOption.AutoSize = True
        Me.CompNullOption.Location = New System.Drawing.Point(6, 88)
        Me.CompNullOption.Name = "CompNullOption"
        Me.CompNullOption.Size = New System.Drawing.Size(119, 17)
        Me.CompNullOption.TabIndex = 6
        Me.CompNullOption.TabStop = True
        Me.CompNullOption.Text = "Scratch space (null)"
        Me.CompNullOption.UseVisualStyleBackColor = True
        '
        'CompFileBrowseButton
        '
        Me.CompFileBrowseButton.Location = New System.Drawing.Point(180, 62)
        Me.CompFileBrowseButton.Name = "CompFileBrowseButton"
        Me.CompFileBrowseButton.Size = New System.Drawing.Size(38, 23)
        Me.CompFileBrowseButton.TabIndex = 5
        Me.CompFileBrowseButton.Text = "..."
        Me.CompFileBrowseButton.UseVisualStyleBackColor = True
        '
        'CompPolFilenameTextbox
        '
        Me.CompPolFilenameTextbox.Location = New System.Drawing.Point(74, 64)
        Me.CompPolFilenameTextbox.Name = "CompPolFilenameTextbox"
        Me.CompPolFilenameTextbox.Size = New System.Drawing.Size(100, 20)
        Me.CompPolFilenameTextbox.TabIndex = 4
        '
        'CompFileOption
        '
        Me.CompFileOption.AutoSize = True
        Me.CompFileOption.Location = New System.Drawing.Point(6, 65)
        Me.CompFileOption.Name = "CompFileOption"
        Me.CompFileOption.Size = New System.Drawing.Size(62, 17)
        Me.CompFileOption.TabIndex = 3
        Me.CompFileOption.TabStop = True
        Me.CompFileOption.Text = "POL file"
        Me.CompFileOption.UseVisualStyleBackColor = True
        '
        'CompRegistryOption
        '
        Me.CompRegistryOption.AutoSize = True
        Me.CompRegistryOption.Location = New System.Drawing.Point(6, 42)
        Me.CompRegistryOption.Name = "CompRegistryOption"
        Me.CompRegistryOption.Size = New System.Drawing.Size(92, 17)
        Me.CompRegistryOption.TabIndex = 1
        Me.CompRegistryOption.TabStop = True
        Me.CompRegistryOption.Text = "Local Registry"
        Me.CompRegistryOption.UseVisualStyleBackColor = True
        '
        'CompLocalOption
        '
        Me.CompLocalOption.AutoSize = True
        Me.CompLocalOption.Location = New System.Drawing.Point(6, 19)
        Me.CompLocalOption.Name = "CompLocalOption"
        Me.CompLocalOption.Size = New System.Drawing.Size(77, 17)
        Me.CompLocalOption.TabIndex = 0
        Me.CompLocalOption.TabStop = True
        Me.CompLocalOption.Text = "Local GPO"
        Me.CompLocalOption.UseVisualStyleBackColor = True
        '
        'UserGroup
        '
        Me.UserGroup.Controls.Add(Me.UserRegTextbox)
        Me.UserGroup.Controls.Add(Me.UserPerUserRegOption)
        Me.UserGroup.Controls.Add(Me.UserPerUserGpoOption)
        Me.UserGroup.Controls.Add(Me.UserBrowseHiveButton)
        Me.UserGroup.Controls.Add(Me.UserNullOption)
        Me.UserGroup.Controls.Add(Me.UserBrowseGpoButton)
        Me.UserGroup.Controls.Add(Me.UserHivePathTextbox)
        Me.UserGroup.Controls.Add(Me.UserFileBrowseButton)
        Me.UserGroup.Controls.Add(Me.UserGpoSidTextbox)
        Me.UserGroup.Controls.Add(Me.UserPolFilenameTextbox)
        Me.UserGroup.Controls.Add(Me.UserFileOption)
        Me.UserGroup.Controls.Add(Me.UserRegistryOption)
        Me.UserGroup.Controls.Add(Me.UserLocalOption)
        Me.UserGroup.Location = New System.Drawing.Point(242, 12)
        Me.UserGroup.Name = "UserGroup"
        Me.UserGroup.Size = New System.Drawing.Size(224, 157)
        Me.UserGroup.TabIndex = 1
        Me.UserGroup.TabStop = False
        Me.UserGroup.Text = "User"
        '
        'UserPerUserRegOption
        '
        Me.UserPerUserRegOption.AutoSize = True
        Me.UserPerUserRegOption.Location = New System.Drawing.Point(6, 111)
        Me.UserPerUserRegOption.Name = "UserPerUserRegOption"
        Me.UserPerUserRegOption.Size = New System.Drawing.Size(70, 17)
        Me.UserPerUserRegOption.TabIndex = 9
        Me.UserPerUserRegOption.TabStop = True
        Me.UserPerUserRegOption.Text = "User hive"
        Me.UserPerUserRegOption.UseVisualStyleBackColor = True
        '
        'UserPerUserGpoOption
        '
        Me.UserPerUserGpoOption.AutoSize = True
        Me.UserPerUserGpoOption.Location = New System.Drawing.Point(6, 88)
        Me.UserPerUserGpoOption.Name = "UserPerUserGpoOption"
        Me.UserPerUserGpoOption.Size = New System.Drawing.Size(73, 17)
        Me.UserPerUserGpoOption.TabIndex = 6
        Me.UserPerUserGpoOption.TabStop = True
        Me.UserPerUserGpoOption.Text = "User GPO"
        Me.UserPerUserGpoOption.UseVisualStyleBackColor = True
        '
        'UserBrowseHiveButton
        '
        Me.UserBrowseHiveButton.Location = New System.Drawing.Point(180, 108)
        Me.UserBrowseHiveButton.Name = "UserBrowseHiveButton"
        Me.UserBrowseHiveButton.Size = New System.Drawing.Size(38, 23)
        Me.UserBrowseHiveButton.TabIndex = 11
        Me.UserBrowseHiveButton.Text = "..."
        Me.UserBrowseHiveButton.UseVisualStyleBackColor = True
        '
        'UserNullOption
        '
        Me.UserNullOption.AutoSize = True
        Me.UserNullOption.Location = New System.Drawing.Point(6, 134)
        Me.UserNullOption.Name = "UserNullOption"
        Me.UserNullOption.Size = New System.Drawing.Size(119, 17)
        Me.UserNullOption.TabIndex = 12
        Me.UserNullOption.TabStop = True
        Me.UserNullOption.Text = "Scratch space (null)"
        Me.UserNullOption.UseVisualStyleBackColor = True
        '
        'UserBrowseGpoButton
        '
        Me.UserBrowseGpoButton.Location = New System.Drawing.Point(180, 85)
        Me.UserBrowseGpoButton.Name = "UserBrowseGpoButton"
        Me.UserBrowseGpoButton.Size = New System.Drawing.Size(38, 23)
        Me.UserBrowseGpoButton.TabIndex = 8
        Me.UserBrowseGpoButton.Text = "..."
        Me.UserBrowseGpoButton.UseVisualStyleBackColor = True
        '
        'UserHivePathTextbox
        '
        Me.UserHivePathTextbox.Location = New System.Drawing.Point(85, 110)
        Me.UserHivePathTextbox.Name = "UserHivePathTextbox"
        Me.UserHivePathTextbox.Size = New System.Drawing.Size(89, 20)
        Me.UserHivePathTextbox.TabIndex = 10
        '
        'UserFileBrowseButton
        '
        Me.UserFileBrowseButton.Location = New System.Drawing.Point(180, 62)
        Me.UserFileBrowseButton.Name = "UserFileBrowseButton"
        Me.UserFileBrowseButton.Size = New System.Drawing.Size(38, 23)
        Me.UserFileBrowseButton.TabIndex = 5
        Me.UserFileBrowseButton.Text = "..."
        Me.UserFileBrowseButton.UseVisualStyleBackColor = True
        '
        'UserGpoSidTextbox
        '
        Me.UserGpoSidTextbox.Location = New System.Drawing.Point(85, 87)
        Me.UserGpoSidTextbox.Name = "UserGpoSidTextbox"
        Me.UserGpoSidTextbox.Size = New System.Drawing.Size(89, 20)
        Me.UserGpoSidTextbox.TabIndex = 7
        '
        'UserPolFilenameTextbox
        '
        Me.UserPolFilenameTextbox.Location = New System.Drawing.Point(74, 64)
        Me.UserPolFilenameTextbox.Name = "UserPolFilenameTextbox"
        Me.UserPolFilenameTextbox.Size = New System.Drawing.Size(100, 20)
        Me.UserPolFilenameTextbox.TabIndex = 4
        '
        'UserFileOption
        '
        Me.UserFileOption.AutoSize = True
        Me.UserFileOption.Location = New System.Drawing.Point(6, 65)
        Me.UserFileOption.Name = "UserFileOption"
        Me.UserFileOption.Size = New System.Drawing.Size(62, 17)
        Me.UserFileOption.TabIndex = 3
        Me.UserFileOption.TabStop = True
        Me.UserFileOption.Text = "POL file"
        Me.UserFileOption.UseVisualStyleBackColor = True
        '
        'UserRegistryOption
        '
        Me.UserRegistryOption.AutoSize = True
        Me.UserRegistryOption.Location = New System.Drawing.Point(6, 42)
        Me.UserRegistryOption.Name = "UserRegistryOption"
        Me.UserRegistryOption.Size = New System.Drawing.Size(92, 17)
        Me.UserRegistryOption.TabIndex = 1
        Me.UserRegistryOption.TabStop = True
        Me.UserRegistryOption.Text = "Local Registry"
        Me.UserRegistryOption.UseVisualStyleBackColor = True
        '
        'UserLocalOption
        '
        Me.UserLocalOption.AutoSize = True
        Me.UserLocalOption.Location = New System.Drawing.Point(6, 19)
        Me.UserLocalOption.Name = "UserLocalOption"
        Me.UserLocalOption.Size = New System.Drawing.Size(77, 17)
        Me.UserLocalOption.TabIndex = 0
        Me.UserLocalOption.TabStop = True
        Me.UserLocalOption.Text = "Local GPO"
        Me.UserLocalOption.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(391, 175)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 18
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CompRegTextbox
        '
        Me.CompRegTextbox.Location = New System.Drawing.Point(104, 41)
        Me.CompRegTextbox.Name = "CompRegTextbox"
        Me.CompRegTextbox.Size = New System.Drawing.Size(114, 20)
        Me.CompRegTextbox.TabIndex = 2
        Me.CompRegTextbox.Text = "HKLM"
        '
        'UserRegTextbox
        '
        Me.UserRegTextbox.Location = New System.Drawing.Point(104, 41)
        Me.UserRegTextbox.Name = "UserRegTextbox"
        Me.UserRegTextbox.Size = New System.Drawing.Size(114, 20)
        Me.UserRegTextbox.TabIndex = 2
        Me.UserRegTextbox.Text = "HKCU"
        '
        'OpenPol
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 210)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.UserGroup)
        Me.Controls.Add(Me.ComputerGroup)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OpenPol"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Open Policy Resources"
        Me.ComputerGroup.ResumeLayout(False)
        Me.ComputerGroup.PerformLayout()
        Me.UserGroup.ResumeLayout(False)
        Me.UserGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ComputerGroup As GroupBox
    Friend WithEvents CompNullOption As RadioButton
    Friend WithEvents CompFileBrowseButton As Button
    Friend WithEvents CompPolFilenameTextbox As TextBox
    Friend WithEvents CompFileOption As RadioButton
    Friend WithEvents CompRegistryOption As RadioButton
    Friend WithEvents CompLocalOption As RadioButton
    Friend WithEvents UserGroup As GroupBox
    Friend WithEvents UserPerUserRegOption As RadioButton
    Friend WithEvents UserPerUserGpoOption As RadioButton
    Friend WithEvents UserBrowseHiveButton As Button
    Friend WithEvents UserNullOption As RadioButton
    Friend WithEvents UserBrowseGpoButton As Button
    Friend WithEvents UserHivePathTextbox As TextBox
    Friend WithEvents UserFileBrowseButton As Button
    Friend WithEvents UserGpoSidTextbox As TextBox
    Friend WithEvents UserPolFilenameTextbox As TextBox
    Friend WithEvents UserFileOption As RadioButton
    Friend WithEvents UserRegistryOption As RadioButton
    Friend WithEvents UserLocalOption As RadioButton
    Friend WithEvents OkButton As Button
    Friend WithEvents CompRegTextbox As TextBox
    Friend WithEvents UserRegTextbox As TextBox
End Class
