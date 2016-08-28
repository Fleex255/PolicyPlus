<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetailCategory
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
        Dim NameLabel As System.Windows.Forms.Label
        Dim IdLabel As System.Windows.Forms.Label
        Dim DefinedLabel As System.Windows.Forms.Label
        Dim DisplayCode As System.Windows.Forms.Label
        Dim InfoCodeLabel As System.Windows.Forms.Label
        Dim ParentLabel As System.Windows.Forms.Label
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.IdTextbox = New System.Windows.Forms.TextBox()
        Me.DefinedTextbox = New System.Windows.Forms.TextBox()
        Me.DisplayCodeTextbox = New System.Windows.Forms.TextBox()
        Me.InfoCodeTextbox = New System.Windows.Forms.TextBox()
        Me.ParentTextbox = New System.Windows.Forms.TextBox()
        Me.ParentButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        NameLabel = New System.Windows.Forms.Label()
        IdLabel = New System.Windows.Forms.Label()
        DefinedLabel = New System.Windows.Forms.Label()
        DisplayCode = New System.Windows.Forms.Label()
        InfoCodeLabel = New System.Windows.Forms.Label()
        ParentLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        NameLabel.AutoSize = True
        NameLabel.Location = New System.Drawing.Point(12, 15)
        NameLabel.Name = "NameLabel"
        NameLabel.Size = New System.Drawing.Size(35, 13)
        NameLabel.TabIndex = 7
        NameLabel.Text = "Name"
        '
        'IdLabel
        '
        IdLabel.AutoSize = True
        IdLabel.Location = New System.Drawing.Point(12, 41)
        IdLabel.Name = "IdLabel"
        IdLabel.Size = New System.Drawing.Size(55, 13)
        IdLabel.TabIndex = 8
        IdLabel.Text = "Unique ID"
        '
        'DefinedLabel
        '
        DefinedLabel.AutoSize = True
        DefinedLabel.Location = New System.Drawing.Point(12, 67)
        DefinedLabel.Name = "DefinedLabel"
        DefinedLabel.Size = New System.Drawing.Size(55, 13)
        DefinedLabel.TabIndex = 9
        DefinedLabel.Text = "Defined in"
        '
        'DisplayCode
        '
        DisplayCode.AutoSize = True
        DisplayCode.Location = New System.Drawing.Point(12, 93)
        DisplayCode.Name = "DisplayCode"
        DisplayCode.Size = New System.Drawing.Size(68, 13)
        DisplayCode.TabIndex = 10
        DisplayCode.Text = "Display code"
        '
        'InfoCodeLabel
        '
        InfoCodeLabel.AutoSize = True
        InfoCodeLabel.Location = New System.Drawing.Point(12, 119)
        InfoCodeLabel.Name = "InfoCodeLabel"
        InfoCodeLabel.Size = New System.Drawing.Size(52, 13)
        InfoCodeLabel.TabIndex = 11
        InfoCodeLabel.Text = "Info code"
        '
        'ParentLabel
        '
        ParentLabel.AutoSize = True
        ParentLabel.Location = New System.Drawing.Point(12, 145)
        ParentLabel.Name = "ParentLabel"
        ParentLabel.Size = New System.Drawing.Size(38, 13)
        ParentLabel.TabIndex = 12
        ParentLabel.Text = "Parent"
        '
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(86, 12)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.ReadOnly = True
        Me.NameTextbox.Size = New System.Drawing.Size(225, 20)
        Me.NameTextbox.TabIndex = 0
        '
        'IdTextbox
        '
        Me.IdTextbox.Location = New System.Drawing.Point(86, 38)
        Me.IdTextbox.Name = "IdTextbox"
        Me.IdTextbox.ReadOnly = True
        Me.IdTextbox.Size = New System.Drawing.Size(225, 20)
        Me.IdTextbox.TabIndex = 1
        '
        'DefinedTextbox
        '
        Me.DefinedTextbox.Location = New System.Drawing.Point(86, 64)
        Me.DefinedTextbox.Name = "DefinedTextbox"
        Me.DefinedTextbox.ReadOnly = True
        Me.DefinedTextbox.Size = New System.Drawing.Size(225, 20)
        Me.DefinedTextbox.TabIndex = 2
        '
        'DisplayCodeTextbox
        '
        Me.DisplayCodeTextbox.Location = New System.Drawing.Point(86, 90)
        Me.DisplayCodeTextbox.Name = "DisplayCodeTextbox"
        Me.DisplayCodeTextbox.ReadOnly = True
        Me.DisplayCodeTextbox.Size = New System.Drawing.Size(225, 20)
        Me.DisplayCodeTextbox.TabIndex = 3
        '
        'InfoCodeTextbox
        '
        Me.InfoCodeTextbox.Location = New System.Drawing.Point(86, 116)
        Me.InfoCodeTextbox.Name = "InfoCodeTextbox"
        Me.InfoCodeTextbox.ReadOnly = True
        Me.InfoCodeTextbox.Size = New System.Drawing.Size(225, 20)
        Me.InfoCodeTextbox.TabIndex = 4
        '
        'ParentTextbox
        '
        Me.ParentTextbox.Location = New System.Drawing.Point(86, 142)
        Me.ParentTextbox.Name = "ParentTextbox"
        Me.ParentTextbox.ReadOnly = True
        Me.ParentTextbox.Size = New System.Drawing.Size(144, 20)
        Me.ParentTextbox.TabIndex = 5
        '
        'ParentButton
        '
        Me.ParentButton.Location = New System.Drawing.Point(236, 140)
        Me.ParentButton.Name = "ParentButton"
        Me.ParentButton.Size = New System.Drawing.Size(75, 23)
        Me.ParentButton.TabIndex = 6
        Me.ParentButton.Text = "Details"
        Me.ParentButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.CloseButton.Location = New System.Drawing.Point(236, 169)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 13
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DetailCategory
        '
        Me.AcceptButton = Me.CloseButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(323, 204)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(ParentLabel)
        Me.Controls.Add(InfoCodeLabel)
        Me.Controls.Add(DisplayCode)
        Me.Controls.Add(DefinedLabel)
        Me.Controls.Add(IdLabel)
        Me.Controls.Add(NameLabel)
        Me.Controls.Add(Me.ParentButton)
        Me.Controls.Add(Me.ParentTextbox)
        Me.Controls.Add(Me.InfoCodeTextbox)
        Me.Controls.Add(Me.DisplayCodeTextbox)
        Me.Controls.Add(Me.DefinedTextbox)
        Me.Controls.Add(Me.IdTextbox)
        Me.Controls.Add(Me.NameTextbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DetailCategory"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Category Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameTextbox As TextBox
    Friend WithEvents IdTextbox As TextBox
    Friend WithEvents DefinedTextbox As TextBox
    Friend WithEvents DisplayCodeTextbox As TextBox
    Friend WithEvents InfoCodeTextbox As TextBox
    Friend WithEvents ParentTextbox As TextBox
    Friend WithEvents ParentButton As Button
    Friend WithEvents CloseButton As Button
End Class
