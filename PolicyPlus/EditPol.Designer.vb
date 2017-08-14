<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditPol
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
        Me.LsvPol = New System.Windows.Forms.ListView()
        Me.ChItem = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChValue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ButtonAddKey = New System.Windows.Forms.Button()
        Me.ButtonAddValue = New System.Windows.Forms.Button()
        Me.ButtonDeleteValue = New System.Windows.Forms.Button()
        Me.ButtonForget = New System.Windows.Forms.Button()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.ButtonImport = New System.Windows.Forms.Button()
        Me.ButtonExport = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LsvPol
        '
        Me.LsvPol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvPol.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChItem, Me.ChValue})
        Me.LsvPol.FullRowSelect = True
        Me.LsvPol.HideSelection = False
        Me.LsvPol.Location = New System.Drawing.Point(12, 41)
        Me.LsvPol.MultiSelect = False
        Me.LsvPol.Name = "LsvPol"
        Me.LsvPol.ShowItemToolTips = True
        Me.LsvPol.Size = New System.Drawing.Size(555, 235)
        Me.LsvPol.TabIndex = 0
        Me.LsvPol.UseCompatibleStateImageBehavior = False
        Me.LsvPol.View = System.Windows.Forms.View.Details
        '
        'ChItem
        '
        Me.ChItem.Text = "Name"
        Me.ChItem.Width = 377
        '
        'ChValue
        '
        Me.ChValue.Text = "Value"
        Me.ChValue.Width = 160
        '
        'ButtonSave
        '
        Me.ButtonSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSave.Location = New System.Drawing.Point(492, 282)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 11
        Me.ButtonSave.Text = "Done"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ButtonAddKey
        '
        Me.ButtonAddKey.Location = New System.Drawing.Point(12, 12)
        Me.ButtonAddKey.Name = "ButtonAddKey"
        Me.ButtonAddKey.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAddKey.TabIndex = 3
        Me.ButtonAddKey.Text = "Add Key"
        Me.ButtonAddKey.UseVisualStyleBackColor = True
        '
        'ButtonAddValue
        '
        Me.ButtonAddValue.Location = New System.Drawing.Point(93, 12)
        Me.ButtonAddValue.Name = "ButtonAddValue"
        Me.ButtonAddValue.Size = New System.Drawing.Size(87, 23)
        Me.ButtonAddValue.TabIndex = 4
        Me.ButtonAddValue.Text = "Add Value"
        Me.ButtonAddValue.UseVisualStyleBackColor = True
        '
        'ButtonDeleteValue
        '
        Me.ButtonDeleteValue.Location = New System.Drawing.Point(186, 12)
        Me.ButtonDeleteValue.Name = "ButtonDeleteValue"
        Me.ButtonDeleteValue.Size = New System.Drawing.Size(100, 23)
        Me.ButtonDeleteValue.TabIndex = 5
        Me.ButtonDeleteValue.Text = "Delete Value(s)"
        Me.ButtonDeleteValue.UseVisualStyleBackColor = True
        '
        'ButtonForget
        '
        Me.ButtonForget.Location = New System.Drawing.Point(292, 12)
        Me.ButtonForget.Name = "ButtonForget"
        Me.ButtonForget.Size = New System.Drawing.Size(75, 23)
        Me.ButtonForget.TabIndex = 6
        Me.ButtonForget.Text = "Forget"
        Me.ButtonForget.UseVisualStyleBackColor = True
        '
        'ButtonEdit
        '
        Me.ButtonEdit.Location = New System.Drawing.Point(373, 12)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(75, 23)
        Me.ButtonEdit.TabIndex = 7
        Me.ButtonEdit.Text = "Edit"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'ButtonImport
        '
        Me.ButtonImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonImport.Location = New System.Drawing.Point(12, 282)
        Me.ButtonImport.Name = "ButtonImport"
        Me.ButtonImport.Size = New System.Drawing.Size(75, 23)
        Me.ButtonImport.TabIndex = 9
        Me.ButtonImport.Text = "Import REG"
        Me.ButtonImport.UseVisualStyleBackColor = True
        '
        'ButtonExport
        '
        Me.ButtonExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonExport.Location = New System.Drawing.Point(93, 282)
        Me.ButtonExport.Name = "ButtonExport"
        Me.ButtonExport.Size = New System.Drawing.Size(75, 23)
        Me.ButtonExport.TabIndex = 10
        Me.ButtonExport.Text = "Export REG"
        Me.ButtonExport.UseVisualStyleBackColor = True
        '
        'EditPol
        '
        Me.AcceptButton = Me.ButtonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(579, 317)
        Me.Controls.Add(Me.ButtonExport)
        Me.Controls.Add(Me.ButtonImport)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.ButtonForget)
        Me.Controls.Add(Me.ButtonDeleteValue)
        Me.Controls.Add(Me.ButtonAddValue)
        Me.Controls.Add(Me.ButtonAddKey)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.LsvPol)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditPol"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Raw POL"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LsvPol As ListView
    Friend WithEvents ButtonSave As Button
    Friend WithEvents ChItem As ColumnHeader
    Friend WithEvents ChValue As ColumnHeader
    Friend WithEvents ButtonAddKey As Button
    Friend WithEvents ButtonAddValue As Button
    Friend WithEvents ButtonDeleteValue As Button
    Friend WithEvents ButtonForget As Button
    Friend WithEvents ButtonEdit As Button
    Friend WithEvents ButtonImport As Button
    Friend WithEvents ButtonExport As Button
End Class
