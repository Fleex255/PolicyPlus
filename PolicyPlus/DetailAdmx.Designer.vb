<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DetailAdmx
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
        Dim Label5 As System.Windows.Forms.Label
        Dim Label6 As System.Windows.Forms.Label
        Dim Label7 As System.Windows.Forms.Label
        Me.TextPath = New System.Windows.Forms.TextBox()
        Me.TextNamespace = New System.Windows.Forms.TextBox()
        Me.TextSupersededAdm = New System.Windows.Forms.TextBox()
        Me.LsvPolicies = New System.Windows.Forms.ListView()
        Me.ChPolicyId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LsvCategories = New System.Windows.Forms.ListView()
        Me.ChCategoryId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChCategoryName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LsvProducts = New System.Windows.Forms.ListView()
        Me.ChProductId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChProductName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LsvSupportDefinitions = New System.Windows.Forms.ListView()
        Me.ChSupportId = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChSupportName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonClose = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        Label2 = New System.Windows.Forms.Label()
        Label3 = New System.Windows.Forms.Label()
        Label4 = New System.Windows.Forms.Label()
        Label5 = New System.Windows.Forms.Label()
        Label6 = New System.Windows.Forms.Label()
        Label7 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(29, 13)
        Label1.TabIndex = 1
        Label1.Text = "Path"
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(12, 41)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(64, 13)
        Label2.TabIndex = 3
        Label2.Text = "Namespace"
        '
        'Label3
        '
        Label3.AutoSize = True
        Label3.Location = New System.Drawing.Point(12, 67)
        Label3.Name = "Label3"
        Label3.Size = New System.Drawing.Size(91, 13)
        Label3.TabIndex = 5
        Label3.Text = "Superseded ADM"
        '
        'Label4
        '
        Label4.AutoSize = True
        Label4.Location = New System.Drawing.Point(12, 93)
        Label4.Name = "Label4"
        Label4.Size = New System.Drawing.Size(43, 13)
        Label4.TabIndex = 7
        Label4.Text = "Policies"
        '
        'Label5
        '
        Label5.AutoSize = True
        Label5.Location = New System.Drawing.Point(12, 196)
        Label5.Name = "Label5"
        Label5.Size = New System.Drawing.Size(57, 13)
        Label5.TabIndex = 9
        Label5.Text = "Categories"
        '
        'Label6
        '
        Label6.AutoSize = True
        Label6.Location = New System.Drawing.Point(12, 299)
        Label6.Name = "Label6"
        Label6.Size = New System.Drawing.Size(49, 13)
        Label6.TabIndex = 11
        Label6.Text = "Products"
        '
        'Label7
        '
        Label7.AutoSize = True
        Label7.Location = New System.Drawing.Point(12, 402)
        Label7.Name = "Label7"
        Label7.Size = New System.Drawing.Size(69, 13)
        Label7.TabIndex = 13
        Label7.Text = "Support rules"
        '
        'TextPath
        '
        Me.TextPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextPath.Location = New System.Drawing.Point(109, 12)
        Me.TextPath.Name = "TextPath"
        Me.TextPath.ReadOnly = True
        Me.TextPath.Size = New System.Drawing.Size(332, 20)
        Me.TextPath.TabIndex = 0
        '
        'TextNamespace
        '
        Me.TextNamespace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextNamespace.Location = New System.Drawing.Point(109, 38)
        Me.TextNamespace.Name = "TextNamespace"
        Me.TextNamespace.ReadOnly = True
        Me.TextNamespace.Size = New System.Drawing.Size(332, 20)
        Me.TextNamespace.TabIndex = 2
        '
        'TextSupersededAdm
        '
        Me.TextSupersededAdm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextSupersededAdm.Location = New System.Drawing.Point(109, 64)
        Me.TextSupersededAdm.Name = "TextSupersededAdm"
        Me.TextSupersededAdm.ReadOnly = True
        Me.TextSupersededAdm.Size = New System.Drawing.Size(332, 20)
        Me.TextSupersededAdm.TabIndex = 4
        '
        'LsvPolicies
        '
        Me.LsvPolicies.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvPolicies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChPolicyId, Me.ChName})
        Me.LsvPolicies.FullRowSelect = True
        Me.LsvPolicies.Location = New System.Drawing.Point(109, 90)
        Me.LsvPolicies.MultiSelect = False
        Me.LsvPolicies.Name = "LsvPolicies"
        Me.LsvPolicies.ShowItemToolTips = True
        Me.LsvPolicies.Size = New System.Drawing.Size(332, 97)
        Me.LsvPolicies.TabIndex = 6
        Me.LsvPolicies.UseCompatibleStateImageBehavior = False
        Me.LsvPolicies.View = System.Windows.Forms.View.Details
        '
        'ChPolicyId
        '
        Me.ChPolicyId.Text = "Local ID"
        Me.ChPolicyId.Width = 73
        '
        'ChName
        '
        Me.ChName.Text = "Name"
        Me.ChName.Width = 166
        '
        'LsvCategories
        '
        Me.LsvCategories.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvCategories.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChCategoryId, Me.ChCategoryName})
        Me.LsvCategories.FullRowSelect = True
        Me.LsvCategories.Location = New System.Drawing.Point(109, 193)
        Me.LsvCategories.MultiSelect = False
        Me.LsvCategories.Name = "LsvCategories"
        Me.LsvCategories.ShowItemToolTips = True
        Me.LsvCategories.Size = New System.Drawing.Size(332, 97)
        Me.LsvCategories.TabIndex = 8
        Me.LsvCategories.UseCompatibleStateImageBehavior = False
        Me.LsvCategories.View = System.Windows.Forms.View.Details
        '
        'ChCategoryId
        '
        Me.ChCategoryId.Text = "Local ID"
        Me.ChCategoryId.Width = 73
        '
        'ChCategoryName
        '
        Me.ChCategoryName.Text = "Name"
        Me.ChCategoryName.Width = 166
        '
        'LsvProducts
        '
        Me.LsvProducts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChProductId, Me.ChProductName})
        Me.LsvProducts.FullRowSelect = True
        Me.LsvProducts.Location = New System.Drawing.Point(109, 296)
        Me.LsvProducts.MultiSelect = False
        Me.LsvProducts.Name = "LsvProducts"
        Me.LsvProducts.ShowItemToolTips = True
        Me.LsvProducts.Size = New System.Drawing.Size(332, 97)
        Me.LsvProducts.TabIndex = 10
        Me.LsvProducts.UseCompatibleStateImageBehavior = False
        Me.LsvProducts.View = System.Windows.Forms.View.Details
        '
        'ChProductId
        '
        Me.ChProductId.Text = "Local ID"
        Me.ChProductId.Width = 73
        '
        'ChProductName
        '
        Me.ChProductName.Text = "Name"
        Me.ChProductName.Width = 166
        '
        'LsvSupportDefinitions
        '
        Me.LsvSupportDefinitions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvSupportDefinitions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChSupportId, Me.ChSupportName})
        Me.LsvSupportDefinitions.FullRowSelect = True
        Me.LsvSupportDefinitions.Location = New System.Drawing.Point(109, 399)
        Me.LsvSupportDefinitions.MultiSelect = False
        Me.LsvSupportDefinitions.Name = "LsvSupportDefinitions"
        Me.LsvSupportDefinitions.ShowItemToolTips = True
        Me.LsvSupportDefinitions.Size = New System.Drawing.Size(332, 97)
        Me.LsvSupportDefinitions.TabIndex = 12
        Me.LsvSupportDefinitions.UseCompatibleStateImageBehavior = False
        Me.LsvSupportDefinitions.View = System.Windows.Forms.View.Details
        '
        'ChSupportId
        '
        Me.ChSupportId.Text = "Local ID"
        Me.ChSupportId.Width = 73
        '
        'ChSupportName
        '
        Me.ChSupportName.Text = "Name"
        Me.ChSupportName.Width = 166
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonClose.Location = New System.Drawing.Point(366, 502)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 14
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'DetailAdmx
        '
        Me.AcceptButton = Me.ButtonClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(453, 537)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Label7)
        Me.Controls.Add(Me.LsvSupportDefinitions)
        Me.Controls.Add(Label6)
        Me.Controls.Add(Me.LsvProducts)
        Me.Controls.Add(Label5)
        Me.Controls.Add(Me.LsvCategories)
        Me.Controls.Add(Label4)
        Me.Controls.Add(Me.LsvPolicies)
        Me.Controls.Add(Label3)
        Me.Controls.Add(Me.TextSupersededAdm)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Me.TextNamespace)
        Me.Controls.Add(Label1)
        Me.Controls.Add(Me.TextPath)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DetailAdmx"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ADMX Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextPath As TextBox
    Friend WithEvents TextNamespace As TextBox
    Friend WithEvents TextSupersededAdm As TextBox
    Friend WithEvents LsvPolicies As ListView
    Friend WithEvents ChPolicyId As ColumnHeader
    Friend WithEvents ChName As ColumnHeader
    Friend WithEvents LsvCategories As ListView
    Friend WithEvents ChCategoryId As ColumnHeader
    Friend WithEvents ChCategoryName As ColumnHeader
    Friend WithEvents LsvProducts As ListView
    Friend WithEvents ChProductId As ColumnHeader
    Friend WithEvents ChProductName As ColumnHeader
    Friend WithEvents LsvSupportDefinitions As ListView
    Friend WithEvents ChSupportId As ColumnHeader
    Friend WithEvents ChSupportName As ColumnHeader
    Friend WithEvents ButtonClose As Button
End Class
