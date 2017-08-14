<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportReg
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
        Me.TextReg = New System.Windows.Forms.TextBox()
        Me.TextRoot = New System.Windows.Forms.TextBox()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ButtonImport = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(46, 13)
        Label1.TabIndex = 0
        Label1.Text = "REG file"
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(12, 41)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(33, 13)
        Label2.TabIndex = 3
        Label2.Text = "Prefix"
        '
        'TextReg
        '
        Me.TextReg.Location = New System.Drawing.Point(64, 12)
        Me.TextReg.Name = "TextReg"
        Me.TextReg.Size = New System.Drawing.Size(195, 20)
        Me.TextReg.TabIndex = 1
        '
        'TextRoot
        '
        Me.TextRoot.Location = New System.Drawing.Point(64, 38)
        Me.TextRoot.Name = "TextRoot"
        Me.TextRoot.Size = New System.Drawing.Size(276, 20)
        Me.TextRoot.TabIndex = 3
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(265, 10)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 2
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ButtonImport
        '
        Me.ButtonImport.Location = New System.Drawing.Point(265, 64)
        Me.ButtonImport.Name = "ButtonImport"
        Me.ButtonImport.Size = New System.Drawing.Size(75, 23)
        Me.ButtonImport.TabIndex = 4
        Me.ButtonImport.Text = "Import"
        Me.ButtonImport.UseVisualStyleBackColor = True
        '
        'ImportReg
        '
        Me.AcceptButton = Me.ButtonImport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(352, 99)
        Me.Controls.Add(Me.ButtonImport)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.TextRoot)
        Me.Controls.Add(Me.TextReg)
        Me.Controls.Add(Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ImportReg"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import REG"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextReg As TextBox
    Friend WithEvents TextRoot As TextBox
    Friend WithEvents ButtonBrowse As Button
    Friend WithEvents ButtonImport As Button
End Class
