<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditPolValue
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
        Dim Label2 As System.Windows.Forms.Label
        Me.ComboKind = New System.Windows.Forms.ComboBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.TextName = New System.Windows.Forms.TextBox()
        Label1 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ComboKind
        '
        Me.ComboKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboKind.FormattingEnabled = True
        Me.ComboKind.Items.AddRange(New Object() {"String", "Expandable string", "List of strings", "32-bit DWord", "64-bit QWord"})
        Me.ComboKind.Location = New System.Drawing.Point(115, 38)
        Me.ComboKind.Name = "ComboKind"
        Me.ComboKind.Size = New System.Drawing.Size(162, 21)
        Me.ComboKind.TabIndex = 2
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 41)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(97, 13)
        Label1.TabIndex = 1
        Label1.Text = "Registry value type"
        '
        'ButtonOK
        '
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(202, 65)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 3
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'TextName
        '
        Me.TextName.Location = New System.Drawing.Point(115, 12)
        Me.TextName.Name = "TextName"
        Me.TextName.Size = New System.Drawing.Size(162, 20)
        Me.TextName.TabIndex = 1
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(12, 15)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(63, 13)
        Label2.TabIndex = 4
        Label2.Text = "Value name"
        '
        'EditPolValue
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(289, 100)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.TextName)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Label1)
        Me.Controls.Add(Me.ComboKind)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "EditPolValue"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Value"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ComboKind As ComboBox
    Friend WithEvents ButtonOK As Button
    Friend WithEvents TextName As TextBox
End Class
