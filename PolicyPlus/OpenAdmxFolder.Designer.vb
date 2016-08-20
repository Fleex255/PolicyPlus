<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpenAdmxFolder
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
        Dim LabelFromWhere As System.Windows.Forms.Label
        Me.OptLocalFolder = New System.Windows.Forms.RadioButton()
        Me.OptSysvol = New System.Windows.Forms.RadioButton()
        Me.OptCustomFolder = New System.Windows.Forms.RadioButton()
        Me.TextFolder = New System.Windows.Forms.TextBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ClearWorkspaceCheckbox = New System.Windows.Forms.CheckBox()
        LabelFromWhere = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LabelFromWhere
        '
        LabelFromWhere.AutoSize = True
        LabelFromWhere.Location = New System.Drawing.Point(12, 9)
        LabelFromWhere.Name = "LabelFromWhere"
        LabelFromWhere.Size = New System.Drawing.Size(228, 13)
        LabelFromWhere.TabIndex = 0
        LabelFromWhere.Text = "Where would you like to load ADMX files from?"
        '
        'OptLocalFolder
        '
        Me.OptLocalFolder.AutoSize = True
        Me.OptLocalFolder.Location = New System.Drawing.Point(15, 25)
        Me.OptLocalFolder.Name = "OptLocalFolder"
        Me.OptLocalFolder.Size = New System.Drawing.Size(196, 17)
        Me.OptLocalFolder.TabIndex = 1
        Me.OptLocalFolder.TabStop = True
        Me.OptLocalFolder.Text = "This system's PolicyDefinitions folder"
        Me.OptLocalFolder.UseVisualStyleBackColor = True
        '
        'OptSysvol
        '
        Me.OptSysvol.AutoSize = True
        Me.OptSysvol.Location = New System.Drawing.Point(15, 48)
        Me.OptSysvol.Name = "OptSysvol"
        Me.OptSysvol.Size = New System.Drawing.Size(133, 17)
        Me.OptSysvol.TabIndex = 2
        Me.OptSysvol.TabStop = True
        Me.OptSysvol.Text = "The domain's SYSVOL"
        Me.OptSysvol.UseVisualStyleBackColor = True
        '
        'OptCustomFolder
        '
        Me.OptCustomFolder.AutoSize = True
        Me.OptCustomFolder.Location = New System.Drawing.Point(15, 71)
        Me.OptCustomFolder.Name = "OptCustomFolder"
        Me.OptCustomFolder.Size = New System.Drawing.Size(77, 17)
        Me.OptCustomFolder.TabIndex = 3
        Me.OptCustomFolder.TabStop = True
        Me.OptCustomFolder.Text = "This folder:"
        Me.OptCustomFolder.UseVisualStyleBackColor = True
        '
        'TextFolder
        '
        Me.TextFolder.Location = New System.Drawing.Point(98, 70)
        Me.TextFolder.Name = "TextFolder"
        Me.TextFolder.Size = New System.Drawing.Size(265, 20)
        Me.TextFolder.TabIndex = 4
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(354, 96)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 7
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(369, 68)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(60, 23)
        Me.ButtonBrowse.TabIndex = 5
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ClearWorkspaceCheckbox
        '
        Me.ClearWorkspaceCheckbox.AutoSize = True
        Me.ClearWorkspaceCheckbox.Checked = True
        Me.ClearWorkspaceCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ClearWorkspaceCheckbox.Location = New System.Drawing.Point(15, 100)
        Me.ClearWorkspaceCheckbox.Name = "ClearWorkspaceCheckbox"
        Me.ClearWorkspaceCheckbox.Size = New System.Drawing.Size(239, 17)
        Me.ClearWorkspaceCheckbox.TabIndex = 6
        Me.ClearWorkspaceCheckbox.Text = "Clear the workspace before adding this folder"
        Me.ClearWorkspaceCheckbox.UseVisualStyleBackColor = True
        '
        'OpenAdmxFolder
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 131)
        Me.Controls.Add(Me.ClearWorkspaceCheckbox)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.TextFolder)
        Me.Controls.Add(Me.OptCustomFolder)
        Me.Controls.Add(Me.OptSysvol)
        Me.Controls.Add(Me.OptLocalFolder)
        Me.Controls.Add(LabelFromWhere)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OpenAdmxFolder"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Open ADMX Folder"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OptLocalFolder As RadioButton
    Friend WithEvents OptSysvol As RadioButton
    Friend WithEvents OptCustomFolder As RadioButton
    Friend WithEvents TextFolder As TextBox
    Friend WithEvents ButtonOK As Button
    Friend WithEvents ButtonBrowse As Button
    Friend WithEvents ClearWorkspaceCheckbox As CheckBox
End Class
