<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindByRegistry
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
        Dim KeyPathLabel As System.Windows.Forms.Label
        Dim ValueLabel As System.Windows.Forms.Label
        Me.KeyTextbox = New System.Windows.Forms.TextBox()
        Me.ValueTextbox = New System.Windows.Forms.TextBox()
        Me.SearchButton = New System.Windows.Forms.Button()
        KeyPathLabel = New System.Windows.Forms.Label()
        ValueLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'KeyPathLabel
        '
        KeyPathLabel.AutoSize = True
        KeyPathLabel.Location = New System.Drawing.Point(12, 15)
        KeyPathLabel.Name = "KeyPathLabel"
        KeyPathLabel.Size = New System.Drawing.Size(90, 13)
        KeyPathLabel.TabIndex = 0
        KeyPathLabel.Text = "Key path or name"
        '
        'ValueLabel
        '
        ValueLabel.AutoSize = True
        ValueLabel.Location = New System.Drawing.Point(12, 41)
        ValueLabel.Name = "ValueLabel"
        ValueLabel.Size = New System.Drawing.Size(63, 13)
        ValueLabel.TabIndex = 2
        ValueLabel.Text = "Value name"
        '
        'KeyTextbox
        '
        Me.KeyTextbox.Location = New System.Drawing.Point(108, 12)
        Me.KeyTextbox.Name = "KeyTextbox"
        Me.KeyTextbox.Size = New System.Drawing.Size(260, 20)
        Me.KeyTextbox.TabIndex = 1
        '
        'ValueTextbox
        '
        Me.ValueTextbox.Location = New System.Drawing.Point(108, 38)
        Me.ValueTextbox.Name = "ValueTextbox"
        Me.ValueTextbox.Size = New System.Drawing.Size(260, 20)
        Me.ValueTextbox.TabIndex = 2
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(293, 64)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 3
        Me.SearchButton.Text = "Search"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'FindByRegistry
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(380, 99)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(ValueLabel)
        Me.Controls.Add(Me.ValueTextbox)
        Me.Controls.Add(Me.KeyTextbox)
        Me.Controls.Add(KeyPathLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindByRegistry"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Find by Registry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents KeyTextbox As TextBox
    Friend WithEvents ValueTextbox As TextBox
    Friend WithEvents SearchButton As Button
End Class
