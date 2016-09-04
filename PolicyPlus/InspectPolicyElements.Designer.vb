<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InspectPolicyElements
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
        Dim PolicyNameLabel As System.Windows.Forms.Label
        Me.PolicyNameTextbox = New System.Windows.Forms.TextBox()
        Me.PolicyDetailsButton = New System.Windows.Forms.Button()
        Me.InfoTreeview = New System.Windows.Forms.TreeView()
        Me.CloseButton = New System.Windows.Forms.Button()
        PolicyNameLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'PolicyNameTextbox
        '
        Me.PolicyNameTextbox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PolicyNameTextbox.Location = New System.Drawing.Point(53, 12)
        Me.PolicyNameTextbox.Name = "PolicyNameTextbox"
        Me.PolicyNameTextbox.ReadOnly = True
        Me.PolicyNameTextbox.Size = New System.Drawing.Size(248, 20)
        Me.PolicyNameTextbox.TabIndex = 0
        '
        'PolicyDetailsButton
        '
        Me.PolicyDetailsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PolicyDetailsButton.Location = New System.Drawing.Point(307, 10)
        Me.PolicyDetailsButton.Name = "PolicyDetailsButton"
        Me.PolicyDetailsButton.Size = New System.Drawing.Size(75, 23)
        Me.PolicyDetailsButton.TabIndex = 1
        Me.PolicyDetailsButton.Text = "Details"
        Me.PolicyDetailsButton.UseVisualStyleBackColor = True
        '
        'PolicyNameLabel
        '
        PolicyNameLabel.AutoSize = True
        PolicyNameLabel.Location = New System.Drawing.Point(12, 15)
        PolicyNameLabel.Name = "PolicyNameLabel"
        PolicyNameLabel.Size = New System.Drawing.Size(35, 13)
        PolicyNameLabel.TabIndex = 2
        PolicyNameLabel.Text = "Policy"
        '
        'InfoTreeview
        '
        Me.InfoTreeview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InfoTreeview.HideSelection = False
        Me.InfoTreeview.Location = New System.Drawing.Point(15, 38)
        Me.InfoTreeview.Name = "InfoTreeview"
        Me.InfoTreeview.ShowNodeToolTips = True
        Me.InfoTreeview.Size = New System.Drawing.Size(367, 193)
        Me.InfoTreeview.TabIndex = 3
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(307, 237)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'InspectPolicyElements
        '
        Me.AcceptButton = Me.CloseButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(394, 272)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.InfoTreeview)
        Me.Controls.Add(PolicyNameLabel)
        Me.Controls.Add(Me.PolicyDetailsButton)
        Me.Controls.Add(Me.PolicyNameTextbox)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(305, 219)
        Me.Name = "InspectPolicyElements"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Element Inspector"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PolicyNameTextbox As TextBox
    Friend WithEvents PolicyDetailsButton As Button
    Friend WithEvents InfoTreeview As TreeView
    Friend WithEvents CloseButton As Button
End Class
