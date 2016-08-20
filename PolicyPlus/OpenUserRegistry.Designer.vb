<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpenUserRegistry
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
        Me.SubfoldersListview = New System.Windows.Forms.ListView()
        Me.ChUsername = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChAccess = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.OkButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SubfoldersListview
        '
        Me.SubfoldersListview.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChUsername, Me.ChAccess})
        Me.SubfoldersListview.FullRowSelect = True
        Me.SubfoldersListview.Location = New System.Drawing.Point(12, 12)
        Me.SubfoldersListview.MultiSelect = False
        Me.SubfoldersListview.Name = "SubfoldersListview"
        Me.SubfoldersListview.Size = New System.Drawing.Size(314, 111)
        Me.SubfoldersListview.TabIndex = 0
        Me.SubfoldersListview.UseCompatibleStateImageBehavior = False
        Me.SubfoldersListview.View = System.Windows.Forms.View.Details
        '
        'ChUsername
        '
        Me.ChUsername.Text = "Folder Name"
        Me.ChUsername.Width = 196
        '
        'ChAccess
        '
        Me.ChAccess.Text = "Accessible"
        Me.ChAccess.Width = 95
        '
        'OkButton
        '
        Me.OkButton.Location = New System.Drawing.Point(251, 129)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 1
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'OpenUserRegistry
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 164)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.SubfoldersListview)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OpenUserRegistry"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Open User Hive"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SubfoldersListview As ListView
    Friend WithEvents ChUsername As ColumnHeader
    Friend WithEvents ChAccess As ColumnHeader
    Friend WithEvents OkButton As Button
End Class
