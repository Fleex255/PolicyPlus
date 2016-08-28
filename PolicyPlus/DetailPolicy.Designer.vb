<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetailPolicy
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
        Dim DisplayLabel As System.Windows.Forms.Label
        Dim InfoLabel As System.Windows.Forms.Label
        Dim PresentLabel As System.Windows.Forms.Label
        Dim SectionLabel As System.Windows.Forms.Label
        Dim SupportLabel As System.Windows.Forms.Label
        Dim CategoryLabel As System.Windows.Forms.Label
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.IdTextbox = New System.Windows.Forms.TextBox()
        Me.DefinedTextbox = New System.Windows.Forms.TextBox()
        Me.DisplayCodeTextbox = New System.Windows.Forms.TextBox()
        Me.InfoCodeTextbox = New System.Windows.Forms.TextBox()
        Me.PresentCodeTextbox = New System.Windows.Forms.TextBox()
        Me.SectionTextbox = New System.Windows.Forms.TextBox()
        Me.SupportTextbox = New System.Windows.Forms.TextBox()
        Me.CategoryTextbox = New System.Windows.Forms.TextBox()
        Me.CategoryButton = New System.Windows.Forms.Button()
        Me.SupportButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        NameLabel = New System.Windows.Forms.Label()
        IdLabel = New System.Windows.Forms.Label()
        DefinedLabel = New System.Windows.Forms.Label()
        DisplayLabel = New System.Windows.Forms.Label()
        InfoLabel = New System.Windows.Forms.Label()
        PresentLabel = New System.Windows.Forms.Label()
        SectionLabel = New System.Windows.Forms.Label()
        SupportLabel = New System.Windows.Forms.Label()
        CategoryLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(111, 12)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.ReadOnly = True
        Me.NameTextbox.Size = New System.Drawing.Size(258, 20)
        Me.NameTextbox.TabIndex = 0
        '
        'IdTextbox
        '
        Me.IdTextbox.Location = New System.Drawing.Point(111, 38)
        Me.IdTextbox.Name = "IdTextbox"
        Me.IdTextbox.ReadOnly = True
        Me.IdTextbox.Size = New System.Drawing.Size(258, 20)
        Me.IdTextbox.TabIndex = 1
        '
        'DefinedTextbox
        '
        Me.DefinedTextbox.Location = New System.Drawing.Point(111, 64)
        Me.DefinedTextbox.Name = "DefinedTextbox"
        Me.DefinedTextbox.ReadOnly = True
        Me.DefinedTextbox.Size = New System.Drawing.Size(258, 20)
        Me.DefinedTextbox.TabIndex = 2
        '
        'DisplayCodeTextbox
        '
        Me.DisplayCodeTextbox.Location = New System.Drawing.Point(111, 90)
        Me.DisplayCodeTextbox.Name = "DisplayCodeTextbox"
        Me.DisplayCodeTextbox.ReadOnly = True
        Me.DisplayCodeTextbox.Size = New System.Drawing.Size(258, 20)
        Me.DisplayCodeTextbox.TabIndex = 3
        '
        'InfoCodeTextbox
        '
        Me.InfoCodeTextbox.Location = New System.Drawing.Point(111, 116)
        Me.InfoCodeTextbox.Name = "InfoCodeTextbox"
        Me.InfoCodeTextbox.ReadOnly = True
        Me.InfoCodeTextbox.Size = New System.Drawing.Size(258, 20)
        Me.InfoCodeTextbox.TabIndex = 4
        '
        'PresentCodeTextbox
        '
        Me.PresentCodeTextbox.Location = New System.Drawing.Point(111, 142)
        Me.PresentCodeTextbox.Name = "PresentCodeTextbox"
        Me.PresentCodeTextbox.ReadOnly = True
        Me.PresentCodeTextbox.Size = New System.Drawing.Size(258, 20)
        Me.PresentCodeTextbox.TabIndex = 5
        '
        'SectionTextbox
        '
        Me.SectionTextbox.Location = New System.Drawing.Point(111, 168)
        Me.SectionTextbox.Name = "SectionTextbox"
        Me.SectionTextbox.ReadOnly = True
        Me.SectionTextbox.Size = New System.Drawing.Size(258, 20)
        Me.SectionTextbox.TabIndex = 6
        '
        'SupportTextbox
        '
        Me.SupportTextbox.Location = New System.Drawing.Point(111, 194)
        Me.SupportTextbox.Name = "SupportTextbox"
        Me.SupportTextbox.ReadOnly = True
        Me.SupportTextbox.Size = New System.Drawing.Size(177, 20)
        Me.SupportTextbox.TabIndex = 7
        '
        'CategoryTextbox
        '
        Me.CategoryTextbox.Location = New System.Drawing.Point(111, 220)
        Me.CategoryTextbox.Name = "CategoryTextbox"
        Me.CategoryTextbox.ReadOnly = True
        Me.CategoryTextbox.Size = New System.Drawing.Size(177, 20)
        Me.CategoryTextbox.TabIndex = 8
        '
        'NameLabel
        '
        NameLabel.AutoSize = True
        NameLabel.Location = New System.Drawing.Point(12, 15)
        NameLabel.Name = "NameLabel"
        NameLabel.Size = New System.Drawing.Size(35, 13)
        NameLabel.TabIndex = 9
        NameLabel.Text = "Name"
        '
        'IdLabel
        '
        IdLabel.AutoSize = True
        IdLabel.Location = New System.Drawing.Point(12, 41)
        IdLabel.Name = "IdLabel"
        IdLabel.Size = New System.Drawing.Size(55, 13)
        IdLabel.TabIndex = 10
        IdLabel.Text = "Unique ID"
        '
        'DefinedLabel
        '
        DefinedLabel.AutoSize = True
        DefinedLabel.Location = New System.Drawing.Point(12, 67)
        DefinedLabel.Name = "DefinedLabel"
        DefinedLabel.Size = New System.Drawing.Size(55, 13)
        DefinedLabel.TabIndex = 11
        DefinedLabel.Text = "Defined in"
        '
        'DisplayLabel
        '
        DisplayLabel.AutoSize = True
        DisplayLabel.Location = New System.Drawing.Point(12, 93)
        DisplayLabel.Name = "DisplayLabel"
        DisplayLabel.Size = New System.Drawing.Size(68, 13)
        DisplayLabel.TabIndex = 12
        DisplayLabel.Text = "Display code"
        '
        'InfoLabel
        '
        InfoLabel.AutoSize = True
        InfoLabel.Location = New System.Drawing.Point(12, 119)
        InfoLabel.Name = "InfoLabel"
        InfoLabel.Size = New System.Drawing.Size(52, 13)
        InfoLabel.TabIndex = 13
        InfoLabel.Text = "Info code"
        '
        'PresentLabel
        '
        PresentLabel.AutoSize = True
        PresentLabel.Location = New System.Drawing.Point(12, 145)
        PresentLabel.Name = "PresentLabel"
        PresentLabel.Size = New System.Drawing.Size(93, 13)
        PresentLabel.TabIndex = 14
        PresentLabel.Text = "Presentation code"
        '
        'SectionLabel
        '
        SectionLabel.AutoSize = True
        SectionLabel.Location = New System.Drawing.Point(12, 171)
        SectionLabel.Name = "SectionLabel"
        SectionLabel.Size = New System.Drawing.Size(43, 13)
        SectionLabel.TabIndex = 15
        SectionLabel.Text = "Section"
        '
        'SupportLabel
        '
        SupportLabel.AutoSize = True
        SupportLabel.Location = New System.Drawing.Point(12, 197)
        SupportLabel.Name = "SupportLabel"
        SupportLabel.Size = New System.Drawing.Size(71, 13)
        SupportLabel.TabIndex = 16
        SupportLabel.Text = "Supported on"
        '
        'CategoryLabel
        '
        CategoryLabel.AutoSize = True
        CategoryLabel.Location = New System.Drawing.Point(12, 223)
        CategoryLabel.Name = "CategoryLabel"
        CategoryLabel.Size = New System.Drawing.Size(49, 13)
        CategoryLabel.TabIndex = 17
        CategoryLabel.Text = "Category"
        '
        'CategoryButton
        '
        Me.CategoryButton.Location = New System.Drawing.Point(294, 218)
        Me.CategoryButton.Name = "CategoryButton"
        Me.CategoryButton.Size = New System.Drawing.Size(75, 23)
        Me.CategoryButton.TabIndex = 18
        Me.CategoryButton.Text = "Details"
        Me.CategoryButton.UseVisualStyleBackColor = True
        '
        'SupportButton
        '
        Me.SupportButton.Location = New System.Drawing.Point(294, 192)
        Me.SupportButton.Name = "SupportButton"
        Me.SupportButton.Size = New System.Drawing.Size(75, 23)
        Me.SupportButton.TabIndex = 17
        Me.SupportButton.Text = "Details"
        Me.SupportButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.CloseButton.Location = New System.Drawing.Point(294, 247)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 19
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DetailPolicy
        '
        Me.AcceptButton = Me.CloseButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(381, 282)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.SupportButton)
        Me.Controls.Add(Me.CategoryButton)
        Me.Controls.Add(CategoryLabel)
        Me.Controls.Add(SupportLabel)
        Me.Controls.Add(SectionLabel)
        Me.Controls.Add(PresentLabel)
        Me.Controls.Add(InfoLabel)
        Me.Controls.Add(DisplayLabel)
        Me.Controls.Add(DefinedLabel)
        Me.Controls.Add(IdLabel)
        Me.Controls.Add(NameLabel)
        Me.Controls.Add(Me.CategoryTextbox)
        Me.Controls.Add(Me.SupportTextbox)
        Me.Controls.Add(Me.SectionTextbox)
        Me.Controls.Add(Me.PresentCodeTextbox)
        Me.Controls.Add(Me.InfoCodeTextbox)
        Me.Controls.Add(Me.DisplayCodeTextbox)
        Me.Controls.Add(Me.DefinedTextbox)
        Me.Controls.Add(Me.IdTextbox)
        Me.Controls.Add(Me.NameTextbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DetailPolicy"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Policy Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameTextbox As TextBox
    Friend WithEvents IdTextbox As TextBox
    Friend WithEvents DefinedTextbox As TextBox
    Friend WithEvents DisplayCodeTextbox As TextBox
    Friend WithEvents InfoCodeTextbox As TextBox
    Friend WithEvents PresentCodeTextbox As TextBox
    Friend WithEvents SectionTextbox As TextBox
    Friend WithEvents SupportTextbox As TextBox
    Friend WithEvents CategoryTextbox As TextBox
    Friend WithEvents CategoryButton As Button
    Friend WithEvents SupportButton As Button
    Friend WithEvents CloseButton As Button
End Class
