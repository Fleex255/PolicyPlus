<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportReg
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
        Dim Label3 As System.Windows.Forms.Label
        Dim Label4 As System.Windows.Forms.Label
        Me.TextReg = New System.Windows.Forms.TextBox()
        Me.TextBranch = New System.Windows.Forms.TextBox()
        Me.TextRoot = New System.Windows.Forms.TextBox()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ButtonExport = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Label3 = New System.Windows.Forms.Label()
        Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(77, 13)
        Label1.TabIndex = 0
        Label1.Text = "Source branch"
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(12, 67)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(73, 13)
        Label2.TabIndex = 3
        Label2.Text = "Registry prefix"
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(12, 41)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(46, 13)
        Label3.TabIndex = 4
        Label3.Text = "REG file"
        '
        'Label4
        '
        Label4.AutoSize = True
        Label4.Location = New System.Drawing.Point(275, 15)
        Label4.Name = "Label4"
        Label4.Size = New System.Drawing.Size(96, 13)
        Label4.TabIndex = 8
        Label4.Text = "(blank to export all)"
        '
        'TextReg
        '
        Me.TextReg.Location = New System.Drawing.Point(95, 38)
        Me.TextReg.Name = "TextReg"
        Me.TextReg.Size = New System.Drawing.Size(195, 20)
        Me.TextReg.TabIndex = 2
        '
        'TextBranch
        '
        Me.TextBranch.Location = New System.Drawing.Point(95, 12)
        Me.TextBranch.Name = "TextBranch"
        Me.TextBranch.Size = New System.Drawing.Size(174, 20)
        Me.TextBranch.TabIndex = 1
        '
        'TextRoot
        '
        Me.TextRoot.Location = New System.Drawing.Point(95, 64)
        Me.TextRoot.Name = "TextRoot"
        Me.TextRoot.Size = New System.Drawing.Size(276, 20)
        Me.TextRoot.TabIndex = 4
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(296, 36)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 3
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ButtonExport
        '
        Me.ButtonExport.Location = New System.Drawing.Point(296, 90)
        Me.ButtonExport.Name = "ButtonExport"
        Me.ButtonExport.Size = New System.Drawing.Size(75, 23)
        Me.ButtonExport.TabIndex = 5
        Me.ButtonExport.Text = "Export"
        Me.ButtonExport.UseVisualStyleBackColor = True
        '
        'ExportReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(383, 125)
        Me.Controls.Add(Label4)
        Me.Controls.Add(Me.ButtonExport)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.TextRoot)
        Me.Controls.Add(Label3)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.TextBranch)
        Me.Controls.Add(Me.TextReg)
        Me.Controls.Add(Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExportReg"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Export REG"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextReg As TextBox
    Friend WithEvents TextBranch As TextBox
    Friend WithEvents TextRoot As TextBox
    Friend WithEvents ButtonBrowse As Button
    Friend WithEvents ButtonExport As Button
End Class
