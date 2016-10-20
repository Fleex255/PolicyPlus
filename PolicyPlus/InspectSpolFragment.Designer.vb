<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InspectSpolFragment
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
        Me.TextPolicyName = New System.Windows.Forms.TextBox()
        Me.LabelPolicy = New System.Windows.Forms.Label()
        Me.TextSpol = New System.Windows.Forms.TextBox()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.ButtonCopy = New System.Windows.Forms.Button()
        Me.CheckHeader = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'TextPolicyName
        '
        Me.TextPolicyName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextPolicyName.Location = New System.Drawing.Point(53, 12)
        Me.TextPolicyName.Name = "TextPolicyName"
        Me.TextPolicyName.ReadOnly = True
        Me.TextPolicyName.Size = New System.Drawing.Size(266, 20)
        Me.TextPolicyName.TabIndex = 0
        '
        'LabelPolicy
        '
        Me.LabelPolicy.AutoSize = True
        Me.LabelPolicy.Location = New System.Drawing.Point(12, 15)
        Me.LabelPolicy.Name = "LabelPolicy"
        Me.LabelPolicy.Size = New System.Drawing.Size(35, 13)
        Me.LabelPolicy.TabIndex = 1
        Me.LabelPolicy.Text = "Policy"
        '
        'TextSpol
        '
        Me.TextSpol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextSpol.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSpol.Location = New System.Drawing.Point(12, 38)
        Me.TextSpol.Multiline = True
        Me.TextSpol.Name = "TextSpol"
        Me.TextSpol.ReadOnly = True
        Me.TextSpol.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextSpol.Size = New System.Drawing.Size(307, 172)
        Me.TextSpol.TabIndex = 1
        Me.TextSpol.WordWrap = False
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonClose.Location = New System.Drawing.Point(244, 216)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 4
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ButtonCopy
        '
        Me.ButtonCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCopy.Location = New System.Drawing.Point(163, 216)
        Me.ButtonCopy.Name = "ButtonCopy"
        Me.ButtonCopy.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCopy.TabIndex = 3
        Me.ButtonCopy.Text = "Copy"
        Me.ButtonCopy.UseVisualStyleBackColor = True
        '
        'CheckHeader
        '
        Me.CheckHeader.AutoSize = True
        Me.CheckHeader.Location = New System.Drawing.Point(12, 220)
        Me.CheckHeader.Name = "CheckHeader"
        Me.CheckHeader.Size = New System.Drawing.Size(128, 17)
        Me.CheckHeader.TabIndex = 2
        Me.CheckHeader.Text = "Include SPOL header"
        Me.CheckHeader.UseVisualStyleBackColor = True
        '
        'InspectSpolFragment
        '
        Me.AcceptButton = Me.ButtonClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(331, 251)
        Me.Controls.Add(Me.CheckHeader)
        Me.Controls.Add(Me.ButtonCopy)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.TextSpol)
        Me.Controls.Add(Me.LabelPolicy)
        Me.Controls.Add(Me.TextPolicyName)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(347, 290)
        Me.Name = "InspectSpolFragment"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Semantic Policy Fragment"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextPolicyName As TextBox
    Friend WithEvents LabelPolicy As Label
    Friend WithEvents TextSpol As TextBox
    Friend WithEvents ButtonClose As Button
    Friend WithEvents ButtonCopy As Button
    Friend WithEvents CheckHeader As CheckBox
End Class
