<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpenUserGpo
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
        Me.UsernameTextbox = New System.Windows.Forms.TextBox()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.SidTextbox = New System.Windows.Forms.TextBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(95, 13)
        Label1.TabIndex = 0
        Label1.Text = "Look up username"
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(12, 41)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(25, 13)
        Label2.TabIndex = 3
        Label2.Text = "SID"
        '
        'UsernameTextbox
        '
        Me.UsernameTextbox.Location = New System.Drawing.Point(113, 12)
        Me.UsernameTextbox.Name = "UsernameTextbox"
        Me.UsernameTextbox.Size = New System.Drawing.Size(153, 20)
        Me.UsernameTextbox.TabIndex = 1
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(272, 10)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(57, 23)
        Me.SearchButton.TabIndex = 2
        Me.SearchButton.Text = "Search"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'SidTextbox
        '
        Me.SidTextbox.Location = New System.Drawing.Point(43, 38)
        Me.SidTextbox.Name = "SidTextbox"
        Me.SidTextbox.Size = New System.Drawing.Size(286, 20)
        Me.SidTextbox.TabIndex = 3
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(254, 64)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 4
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'OpenUserGpo
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 99)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.SidTextbox)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.UsernameTextbox)
        Me.Controls.Add(Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OpenUserGpo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select User SID"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents UsernameTextbox As TextBox
    Friend WithEvents SearchButton As Button
    Friend WithEvents SidTextbox As TextBox
    Friend WithEvents OkButton As Button
End Class
