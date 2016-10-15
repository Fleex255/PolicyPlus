<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportSpol
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
        Me.ButtonOpenFile = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.TextSpol = New System.Windows.Forms.TextBox()
        Me.ButtonVerify = New System.Windows.Forms.Button()
        Me.ButtonReset = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonOpenFile
        '
        Me.ButtonOpenFile.Location = New System.Drawing.Point(157, 12)
        Me.ButtonOpenFile.Name = "ButtonOpenFile"
        Me.ButtonOpenFile.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOpenFile.TabIndex = 0
        Me.ButtonOpenFile.Text = "Open File"
        Me.ButtonOpenFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Semantic Policy (SPOL) text"
        '
        'ButtonApply
        '
        Me.ButtonApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonApply.Location = New System.Drawing.Point(303, 219)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.Size = New System.Drawing.Size(75, 23)
        Me.ButtonApply.TabIndex = 4
        Me.ButtonApply.Text = "Apply"
        Me.ButtonApply.UseVisualStyleBackColor = True
        '
        'TextSpol
        '
        Me.TextSpol.AcceptsReturn = True
        Me.TextSpol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextSpol.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSpol.Location = New System.Drawing.Point(12, 41)
        Me.TextSpol.Multiline = True
        Me.TextSpol.Name = "TextSpol"
        Me.TextSpol.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextSpol.Size = New System.Drawing.Size(366, 172)
        Me.TextSpol.TabIndex = 2
        Me.TextSpol.Text = "Policy Plus Semantic Policy" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.TextSpol.WordWrap = False
        '
        'ButtonVerify
        '
        Me.ButtonVerify.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonVerify.Location = New System.Drawing.Point(222, 219)
        Me.ButtonVerify.Name = "ButtonVerify"
        Me.ButtonVerify.Size = New System.Drawing.Size(75, 23)
        Me.ButtonVerify.TabIndex = 3
        Me.ButtonVerify.Text = "Verify"
        Me.ButtonVerify.UseVisualStyleBackColor = True
        '
        'ButtonReset
        '
        Me.ButtonReset.Location = New System.Drawing.Point(238, 12)
        Me.ButtonReset.Name = "ButtonReset"
        Me.ButtonReset.Size = New System.Drawing.Size(75, 23)
        Me.ButtonReset.TabIndex = 1
        Me.ButtonReset.Text = "Reset"
        Me.ButtonReset.UseVisualStyleBackColor = True
        '
        'ImportSpol
        '
        Me.AcceptButton = Me.ButtonApply
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(390, 254)
        Me.Controls.Add(Me.ButtonReset)
        Me.Controls.Add(Me.ButtonVerify)
        Me.Controls.Add(Me.TextSpol)
        Me.Controls.Add(Me.ButtonApply)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonOpenFile)
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(373, 266)
        Me.Name = "ImportSpol"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import Semantic Policy"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ButtonOpenFile As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ButtonApply As Button
    Friend WithEvents TextSpol As TextBox
    Friend WithEvents ButtonVerify As Button
    Friend WithEvents ButtonReset As Button
End Class
