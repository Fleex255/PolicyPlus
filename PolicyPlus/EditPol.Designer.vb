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
        Me.LsvPol.Location = New System.Drawing.Point(12, 12)
        Me.LsvPol.MultiSelect = False
        Me.LsvPol.Name = "LsvPol"
        Me.LsvPol.ShowItemToolTips = True
        Me.LsvPol.Size = New System.Drawing.Size(555, 223)
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
        Me.ButtonSave.Location = New System.Drawing.Point(492, 241)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 2
        Me.ButtonSave.Text = "OK"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'EditPol
        '
        Me.AcceptButton = Me.ButtonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(579, 276)
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
End Class
