<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindById
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
        Me.StatusImage = New System.Windows.Forms.PictureBox()
        Me.IdTextbox = New System.Windows.Forms.TextBox()
        Me.GoButton = New System.Windows.Forms.Button()
        CType(Me.StatusImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'StatusImage
        '
        Me.StatusImage.Location = New System.Drawing.Point(12, 14)
        Me.StatusImage.Name = "StatusImage"
        Me.StatusImage.Size = New System.Drawing.Size(16, 16)
        Me.StatusImage.TabIndex = 0
        Me.StatusImage.TabStop = False
        '
        'IdTextbox
        '
        Me.IdTextbox.Location = New System.Drawing.Point(34, 12)
        Me.IdTextbox.Name = "IdTextbox"
        Me.IdTextbox.Size = New System.Drawing.Size(277, 20)
        Me.IdTextbox.TabIndex = 1
        Me.IdTextbox.Text = " "
        '
        'GoButton
        '
        Me.GoButton.Location = New System.Drawing.Point(236, 38)
        Me.GoButton.Name = "GoButton"
        Me.GoButton.Size = New System.Drawing.Size(75, 23)
        Me.GoButton.TabIndex = 2
        Me.GoButton.Text = "Go"
        Me.GoButton.UseVisualStyleBackColor = True
        '
        'FindById
        '
        Me.AcceptButton = Me.GoButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(323, 73)
        Me.Controls.Add(Me.GoButton)
        Me.Controls.Add(Me.IdTextbox)
        Me.Controls.Add(Me.StatusImage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindById"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Find by ID"
        CType(Me.StatusImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusImage As PictureBox
    Friend WithEvents IdTextbox As TextBox
    Friend WithEvents GoButton As Button
End Class
