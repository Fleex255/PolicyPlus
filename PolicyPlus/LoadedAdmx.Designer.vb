<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadedAdmx
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
        Me.LsvAdmx = New System.Windows.Forms.ListView()
        Me.ChFileTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChFolder = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChNamespace = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LsvAdmx
        '
        Me.LsvAdmx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LsvAdmx.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChFileTitle, Me.ChFolder, Me.ChNamespace})
        Me.LsvAdmx.FullRowSelect = True
        Me.LsvAdmx.HideSelection = False
        Me.LsvAdmx.Location = New System.Drawing.Point(12, 12)
        Me.LsvAdmx.MultiSelect = False
        Me.LsvAdmx.Name = "LsvAdmx"
        Me.LsvAdmx.ShowItemToolTips = True
        Me.LsvAdmx.Size = New System.Drawing.Size(487, 233)
        Me.LsvAdmx.TabIndex = 0
        Me.LsvAdmx.UseCompatibleStateImageBehavior = False
        Me.LsvAdmx.View = System.Windows.Forms.View.Details
        '
        'ChFileTitle
        '
        Me.ChFileTitle.Text = "File"
        Me.ChFileTitle.Width = 88
        '
        'ChFolder
        '
        Me.ChFolder.Text = "Folder"
        Me.ChFolder.Width = 203
        '
        'ChNamespace
        '
        Me.ChNamespace.Text = "Namespace"
        Me.ChNamespace.Width = 172
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonClose.Location = New System.Drawing.Point(424, 251)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'LoadedAdmx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(511, 286)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.LsvAdmx)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoadedAdmx"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Loaded ADMX Files"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LsvAdmx As ListView
    Friend WithEvents ChFileTitle As ColumnHeader
    Friend WithEvents ChFolder As ColumnHeader
    Friend WithEvents ChNamespace As ColumnHeader
    Friend WithEvents ButtonClose As Button
End Class
