<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindByText
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
        Me.StringTextbox = New System.Windows.Forms.TextBox()
        Me.TitleCheckbox = New System.Windows.Forms.CheckBox()
        Me.DescriptionCheckbox = New System.Windows.Forms.CheckBox()
        Me.CommentCheckbox = New System.Windows.Forms.CheckBox()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'StringTextbox
        '
        Me.StringTextbox.Location = New System.Drawing.Point(12, 12)
        Me.StringTextbox.Name = "StringTextbox"
        Me.StringTextbox.Size = New System.Drawing.Size(352, 20)
        Me.StringTextbox.TabIndex = 0
        '
        'TitleCheckbox
        '
        Me.TitleCheckbox.AutoSize = True
        Me.TitleCheckbox.Checked = True
        Me.TitleCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TitleCheckbox.Location = New System.Drawing.Point(12, 38)
        Me.TitleCheckbox.Name = "TitleCheckbox"
        Me.TitleCheckbox.Size = New System.Drawing.Size(54, 17)
        Me.TitleCheckbox.TabIndex = 1
        Me.TitleCheckbox.Text = "In title"
        Me.TitleCheckbox.UseVisualStyleBackColor = True
        '
        'DescriptionCheckbox
        '
        Me.DescriptionCheckbox.AutoSize = True
        Me.DescriptionCheckbox.Checked = True
        Me.DescriptionCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.DescriptionCheckbox.Location = New System.Drawing.Point(72, 38)
        Me.DescriptionCheckbox.Name = "DescriptionCheckbox"
        Me.DescriptionCheckbox.Size = New System.Drawing.Size(89, 17)
        Me.DescriptionCheckbox.TabIndex = 2
        Me.DescriptionCheckbox.Text = "In description"
        Me.DescriptionCheckbox.UseVisualStyleBackColor = True
        '
        'CommentCheckbox
        '
        Me.CommentCheckbox.AutoSize = True
        Me.CommentCheckbox.Checked = True
        Me.CommentCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CommentCheckbox.Location = New System.Drawing.Point(167, 38)
        Me.CommentCheckbox.Name = "CommentCheckbox"
        Me.CommentCheckbox.Size = New System.Drawing.Size(81, 17)
        Me.CommentCheckbox.TabIndex = 3
        Me.CommentCheckbox.Text = "In comment"
        Me.CommentCheckbox.UseVisualStyleBackColor = True
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(289, 61)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 4
        Me.SearchButton.Text = "Search"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'FindByText
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(376, 96)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.CommentCheckbox)
        Me.Controls.Add(Me.DescriptionCheckbox)
        Me.Controls.Add(Me.TitleCheckbox)
        Me.Controls.Add(Me.StringTextbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindByText"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Find by Text"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StringTextbox As TextBox
    Friend WithEvents TitleCheckbox As CheckBox
    Friend WithEvents DescriptionCheckbox As CheckBox
    Friend WithEvents CommentCheckbox As CheckBox
    Friend WithEvents SearchButton As Button
End Class
