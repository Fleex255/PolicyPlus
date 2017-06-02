<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadedProducts
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
        Dim ColumnHeader1 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader2 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader3 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader4 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader5 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader6 As System.Windows.Forms.ColumnHeader
        Dim ColumnHeader7 As System.Windows.Forms.ColumnHeader
        Me.LsvTopLevelProducts = New System.Windows.Forms.ListView()
        Me.LabelMajorVersion = New System.Windows.Forms.Label()
        Me.LsvMajorVersions = New System.Windows.Forms.ListView()
        Me.LabelMinorVersion = New System.Windows.Forms.Label()
        Me.LsvMinorVersions = New System.Windows.Forms.ListView()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Label1 = New System.Windows.Forms.Label()
        ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(95, 13)
        Label1.TabIndex = 1
        Label1.Text = "Top-level products"
        '
        'ColumnHeader1
        '
        ColumnHeader1.Text = "Name"
        ColumnHeader1.Width = 308
        '
        'ColumnHeader2
        '
        ColumnHeader2.Text = "Children"
        ColumnHeader2.Width = 53
        '
        'ColumnHeader3
        '
        ColumnHeader3.Text = "Name"
        ColumnHeader3.Width = 252
        '
        'ColumnHeader4
        '
        ColumnHeader4.Text = "Version"
        ColumnHeader4.Width = 53
        '
        'ColumnHeader5
        '
        ColumnHeader5.Text = "Name"
        ColumnHeader5.Width = 308
        '
        'ColumnHeader6
        '
        ColumnHeader6.Text = "Version"
        ColumnHeader6.Width = 53
        '
        'ColumnHeader7
        '
        ColumnHeader7.Text = "Children"
        '
        'LsvTopLevelProducts
        '
        Me.LsvTopLevelProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {ColumnHeader1, ColumnHeader2})
        Me.LsvTopLevelProducts.FullRowSelect = True
        Me.LsvTopLevelProducts.HideSelection = False
        Me.LsvTopLevelProducts.Location = New System.Drawing.Point(12, 25)
        Me.LsvTopLevelProducts.MultiSelect = False
        Me.LsvTopLevelProducts.Name = "LsvTopLevelProducts"
        Me.LsvTopLevelProducts.ShowItemToolTips = True
        Me.LsvTopLevelProducts.Size = New System.Drawing.Size(385, 97)
        Me.LsvTopLevelProducts.TabIndex = 0
        Me.LsvTopLevelProducts.UseCompatibleStateImageBehavior = False
        Me.LsvTopLevelProducts.View = System.Windows.Forms.View.Details
        '
        'LabelMajorVersion
        '
        Me.LabelMajorVersion.AutoSize = True
        Me.LabelMajorVersion.Location = New System.Drawing.Point(12, 125)
        Me.LabelMajorVersion.Name = "LabelMajorVersion"
        Me.LabelMajorVersion.Size = New System.Drawing.Size(75, 13)
        Me.LabelMajorVersion.TabIndex = 3
        Me.LabelMajorVersion.Text = "Major versions"
        '
        'LsvMajorVersions
        '
        Me.LsvMajorVersions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {ColumnHeader3, ColumnHeader4, ColumnHeader7})
        Me.LsvMajorVersions.FullRowSelect = True
        Me.LsvMajorVersions.HideSelection = False
        Me.LsvMajorVersions.Location = New System.Drawing.Point(12, 141)
        Me.LsvMajorVersions.MultiSelect = False
        Me.LsvMajorVersions.Name = "LsvMajorVersions"
        Me.LsvMajorVersions.ShowItemToolTips = True
        Me.LsvMajorVersions.Size = New System.Drawing.Size(385, 97)
        Me.LsvMajorVersions.TabIndex = 1
        Me.LsvMajorVersions.UseCompatibleStateImageBehavior = False
        Me.LsvMajorVersions.View = System.Windows.Forms.View.Details
        '
        'LabelMinorVersion
        '
        Me.LabelMinorVersion.AutoSize = True
        Me.LabelMinorVersion.Location = New System.Drawing.Point(12, 241)
        Me.LabelMinorVersion.Name = "LabelMinorVersion"
        Me.LabelMinorVersion.Size = New System.Drawing.Size(75, 13)
        Me.LabelMinorVersion.TabIndex = 5
        Me.LabelMinorVersion.Text = "Minor versions"
        '
        'LsvMinorVersions
        '
        Me.LsvMinorVersions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {ColumnHeader5, ColumnHeader6})
        Me.LsvMinorVersions.FullRowSelect = True
        Me.LsvMinorVersions.HideSelection = False
        Me.LsvMinorVersions.Location = New System.Drawing.Point(12, 257)
        Me.LsvMinorVersions.MultiSelect = False
        Me.LsvMinorVersions.Name = "LsvMinorVersions"
        Me.LsvMinorVersions.ShowItemToolTips = True
        Me.LsvMinorVersions.Size = New System.Drawing.Size(385, 97)
        Me.LsvMinorVersions.TabIndex = 2
        Me.LsvMinorVersions.UseCompatibleStateImageBehavior = False
        Me.LsvMinorVersions.View = System.Windows.Forms.View.Details
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonClose.Location = New System.Drawing.Point(322, 360)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 3
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'LoadedProducts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(409, 395)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.LabelMinorVersion)
        Me.Controls.Add(Me.LsvMinorVersions)
        Me.Controls.Add(Me.LabelMajorVersion)
        Me.Controls.Add(Me.LsvMajorVersions)
        Me.Controls.Add(Label1)
        Me.Controls.Add(Me.LsvTopLevelProducts)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoadedProducts"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "All Products"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LsvTopLevelProducts As ListView
    Friend WithEvents LabelMajorVersion As Label
    Friend WithEvents LsvMajorVersions As ListView
    Friend WithEvents LabelMinorVersion As Label
    Friend WithEvents LsvMinorVersions As ListView
    Friend WithEvents ButtonClose As Button
End Class
