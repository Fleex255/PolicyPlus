<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditPolDelete
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
        Dim Label1 As System.Windows.Forms.Label
        Me.TextKey = New System.Windows.Forms.TextBox()
        Me.OptPurge = New System.Windows.Forms.RadioButton()
        Me.OptClearFirst = New System.Windows.Forms.RadioButton()
        Me.OptDeleteOne = New System.Windows.Forms.RadioButton()
        Me.TextValueName = New System.Windows.Forms.TextBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(72, 13)
        Label1.TabIndex = 1
        Label1.Text = "Container key"
        '
        'TextKey
        '
        Me.TextKey.Location = New System.Drawing.Point(90, 12)
        Me.TextKey.Name = "TextKey"
        Me.TextKey.ReadOnly = True
        Me.TextKey.Size = New System.Drawing.Size(179, 20)
        Me.TextKey.TabIndex = 0
        '
        'OptPurge
        '
        Me.OptPurge.AutoSize = True
        Me.OptPurge.Location = New System.Drawing.Point(15, 38)
        Me.OptPurge.Name = "OptPurge"
        Me.OptPurge.Size = New System.Drawing.Size(189, 17)
        Me.OptPurge.TabIndex = 2
        Me.OptPurge.TabStop = True
        Me.OptPurge.Text = "Delete all the values under the key"
        Me.OptPurge.UseVisualStyleBackColor = True
        '
        'OptClearFirst
        '
        Me.OptClearFirst.AutoSize = True
        Me.OptClearFirst.Location = New System.Drawing.Point(15, 61)
        Me.OptClearFirst.Name = "OptClearFirst"
        Me.OptClearFirst.Size = New System.Drawing.Size(189, 17)
        Me.OptClearFirst.TabIndex = 3
        Me.OptClearFirst.TabStop = True
        Me.OptClearFirst.Text = "Clear the key before adding values"
        Me.OptClearFirst.UseVisualStyleBackColor = True
        '
        'OptDeleteOne
        '
        Me.OptDeleteOne.AutoSize = True
        Me.OptDeleteOne.Location = New System.Drawing.Point(15, 84)
        Me.OptDeleteOne.Name = "OptDeleteOne"
        Me.OptDeleteOne.Size = New System.Drawing.Size(107, 17)
        Me.OptDeleteOne.TabIndex = 4
        Me.OptDeleteOne.TabStop = True
        Me.OptDeleteOne.Text = "Delete this value:"
        Me.OptDeleteOne.UseVisualStyleBackColor = True
        '
        'TextValueName
        '
        Me.TextValueName.Enabled = False
        Me.TextValueName.Location = New System.Drawing.Point(128, 83)
        Me.TextValueName.Name = "TextValueName"
        Me.TextValueName.Size = New System.Drawing.Size(141, 20)
        Me.TextValueName.TabIndex = 5
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(194, 109)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 6
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'EditPolDelete
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(281, 144)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.TextValueName)
        Me.Controls.Add(Me.OptDeleteOne)
        Me.Controls.Add(Me.OptClearFirst)
        Me.Controls.Add(Me.OptPurge)
        Me.Controls.Add(Label1)
        Me.Controls.Add(Me.TextKey)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditPolDelete"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Delete Value(s)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextKey As TextBox
    Friend WithEvents OptPurge As RadioButton
    Friend WithEvents OptClearFirst As RadioButton
    Friend WithEvents OptDeleteOne As RadioButton
    Friend WithEvents TextValueName As TextBox
    Friend WithEvents ButtonOK As Button
End Class
