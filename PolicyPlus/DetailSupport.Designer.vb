<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetailSupport
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
        Dim DisplayCodeLabel As System.Windows.Forms.Label
        Dim LogicLabel As System.Windows.Forms.Label
        Dim ProductsLabel As System.Windows.Forms.Label
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.IdTextbox = New System.Windows.Forms.TextBox()
        Me.DefinedTextbox = New System.Windows.Forms.TextBox()
        Me.DisplayCodeTextbox = New System.Windows.Forms.TextBox()
        Me.LogicTextbox = New System.Windows.Forms.TextBox()
        Me.EntriesListview = New System.Windows.Forms.ListView()
        Me.ChName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChMinVer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChMaxVer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CloseButton = New System.Windows.Forms.Button()
        NameLabel = New System.Windows.Forms.Label()
        IdLabel = New System.Windows.Forms.Label()
        DefinedLabel = New System.Windows.Forms.Label()
        DisplayCodeLabel = New System.Windows.Forms.Label()
        LogicLabel = New System.Windows.Forms.Label()
        ProductsLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        NameLabel.AutoSize = True
        NameLabel.Location = New System.Drawing.Point(12, 15)
        NameLabel.Name = "NameLabel"
        NameLabel.Size = New System.Drawing.Size(35, 13)
        NameLabel.TabIndex = 5
        NameLabel.Text = "Name"
        '
        'IdLabel
        '
        IdLabel.AutoSize = True
        IdLabel.Location = New System.Drawing.Point(12, 41)
        IdLabel.Name = "IdLabel"
        IdLabel.Size = New System.Drawing.Size(55, 13)
        IdLabel.TabIndex = 6
        IdLabel.Text = "Unique ID"
        '
        'DefinedLabel
        '
        DefinedLabel.AutoSize = True
        DefinedLabel.Location = New System.Drawing.Point(12, 67)
        DefinedLabel.Name = "DefinedLabel"
        DefinedLabel.Size = New System.Drawing.Size(55, 13)
        DefinedLabel.TabIndex = 7
        DefinedLabel.Text = "Defined in"
        '
        'DisplayCodeLabel
        '
        DisplayCodeLabel.AutoSize = True
        DisplayCodeLabel.Location = New System.Drawing.Point(12, 93)
        DisplayCodeLabel.Name = "DisplayCodeLabel"
        DisplayCodeLabel.Size = New System.Drawing.Size(68, 13)
        DisplayCodeLabel.TabIndex = 8
        DisplayCodeLabel.Text = "Display code"
        '
        'LogicLabel
        '
        LogicLabel.AutoSize = True
        LogicLabel.Location = New System.Drawing.Point(12, 119)
        LogicLabel.Name = "LogicLabel"
        LogicLabel.Size = New System.Drawing.Size(64, 13)
        LogicLabel.TabIndex = 9
        LogicLabel.Text = "Composition"
        '
        'ProductsLabel
        '
        ProductsLabel.AutoSize = True
        ProductsLabel.Location = New System.Drawing.Point(12, 145)
        ProductsLabel.Name = "ProductsLabel"
        ProductsLabel.Size = New System.Drawing.Size(49, 13)
        ProductsLabel.TabIndex = 11
        ProductsLabel.Text = "Products"
        '
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(86, 12)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.ReadOnly = True
        Me.NameTextbox.Size = New System.Drawing.Size(268, 20)
        Me.NameTextbox.TabIndex = 0
        '
        'IdTextbox
        '
        Me.IdTextbox.Location = New System.Drawing.Point(86, 38)
        Me.IdTextbox.Name = "IdTextbox"
        Me.IdTextbox.ReadOnly = True
        Me.IdTextbox.Size = New System.Drawing.Size(268, 20)
        Me.IdTextbox.TabIndex = 1
        '
        'DefinedTextbox
        '
        Me.DefinedTextbox.Location = New System.Drawing.Point(86, 64)
        Me.DefinedTextbox.Name = "DefinedTextbox"
        Me.DefinedTextbox.ReadOnly = True
        Me.DefinedTextbox.Size = New System.Drawing.Size(268, 20)
        Me.DefinedTextbox.TabIndex = 2
        '
        'DisplayCodeTextbox
        '
        Me.DisplayCodeTextbox.Location = New System.Drawing.Point(86, 90)
        Me.DisplayCodeTextbox.Name = "DisplayCodeTextbox"
        Me.DisplayCodeTextbox.ReadOnly = True
        Me.DisplayCodeTextbox.Size = New System.Drawing.Size(268, 20)
        Me.DisplayCodeTextbox.TabIndex = 3
        '
        'LogicTextbox
        '
        Me.LogicTextbox.Location = New System.Drawing.Point(86, 116)
        Me.LogicTextbox.Name = "LogicTextbox"
        Me.LogicTextbox.ReadOnly = True
        Me.LogicTextbox.Size = New System.Drawing.Size(268, 20)
        Me.LogicTextbox.TabIndex = 4
        '
        'EntriesListview
        '
        Me.EntriesListview.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChName, Me.ChMinVer, Me.ChMaxVer})
        Me.EntriesListview.FullRowSelect = True
        Me.EntriesListview.HideSelection = False
        Me.EntriesListview.Location = New System.Drawing.Point(86, 142)
        Me.EntriesListview.MultiSelect = False
        Me.EntriesListview.Name = "EntriesListview"
        Me.EntriesListview.ShowItemToolTips = True
        Me.EntriesListview.Size = New System.Drawing.Size(268, 87)
        Me.EntriesListview.TabIndex = 12
        Me.EntriesListview.UseCompatibleStateImageBehavior = False
        Me.EntriesListview.View = System.Windows.Forms.View.Details
        '
        'ChName
        '
        Me.ChName.Text = "Name"
        Me.ChName.Width = 158
        '
        'ChMinVer
        '
        Me.ChMinVer.Text = "Min"
        Me.ChMinVer.Width = 40
        '
        'ChMaxVer
        '
        Me.ChMaxVer.Text = "Max"
        Me.ChMaxVer.Width = 40
        '
        'CloseButton
        '
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.CloseButton.Location = New System.Drawing.Point(279, 235)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 13
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DetailSupport
        '
        Me.AcceptButton = Me.CloseButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(366, 270)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.EntriesListview)
        Me.Controls.Add(ProductsLabel)
        Me.Controls.Add(LogicLabel)
        Me.Controls.Add(DisplayCodeLabel)
        Me.Controls.Add(DefinedLabel)
        Me.Controls.Add(IdLabel)
        Me.Controls.Add(NameLabel)
        Me.Controls.Add(Me.LogicTextbox)
        Me.Controls.Add(Me.DisplayCodeTextbox)
        Me.Controls.Add(Me.DefinedTextbox)
        Me.Controls.Add(Me.IdTextbox)
        Me.Controls.Add(Me.NameTextbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DetailSupport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Support Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameTextbox As TextBox
    Friend WithEvents IdTextbox As TextBox
    Friend WithEvents DefinedTextbox As TextBox
    Friend WithEvents DisplayCodeTextbox As TextBox
    Friend WithEvents LogicTextbox As TextBox
    Friend WithEvents EntriesListview As ListView
    Friend WithEvents ChName As ColumnHeader
    Friend WithEvents ChMinVer As ColumnHeader
    Friend WithEvents ChMaxVer As ColumnHeader
    Friend WithEvents CloseButton As Button
End Class
