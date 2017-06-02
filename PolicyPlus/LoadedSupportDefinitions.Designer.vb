<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadedSupportDefinitions
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
        Me.LsvSupport = New System.Windows.Forms.ListView()
        Me.ChName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ChDefinedIn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.TextFilter = New System.Windows.Forms.TextBox()
        Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Location = New System.Drawing.Point(12, 15)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(79, 13)
        Label1.TabIndex = 3
        Label1.Text = "Substring (filter)"
        '
        'LsvSupport
        '
        Me.LsvSupport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ChName, Me.ChDefinedIn})
        Me.LsvSupport.FullRowSelect = True
        Me.LsvSupport.Location = New System.Drawing.Point(12, 38)
        Me.LsvSupport.MultiSelect = False
        Me.LsvSupport.Name = "LsvSupport"
        Me.LsvSupport.ShowItemToolTips = True
        Me.LsvSupport.Size = New System.Drawing.Size(435, 190)
        Me.LsvSupport.TabIndex = 2
        Me.LsvSupport.UseCompatibleStateImageBehavior = False
        Me.LsvSupport.View = System.Windows.Forms.View.Details
        '
        'ChName
        '
        Me.ChName.Text = "Name"
        Me.ChName.Width = 317
        '
        'ChDefinedIn
        '
        Me.ChDefinedIn.Text = "ADMX File"
        Me.ChDefinedIn.Width = 97
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonClose.Location = New System.Drawing.Point(372, 234)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 3
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'TextFilter
        '
        Me.TextFilter.Location = New System.Drawing.Point(97, 12)
        Me.TextFilter.Name = "TextFilter"
        Me.TextFilter.Size = New System.Drawing.Size(350, 20)
        Me.TextFilter.TabIndex = 1
        '
        'LoadedSupportDefinitions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(459, 269)
        Me.Controls.Add(Label1)
        Me.Controls.Add(Me.TextFilter)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.LsvSupport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoadedSupportDefinitions"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "All Support Definitions"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LsvSupport As ListView
    Friend WithEvents ChName As ColumnHeader
    Friend WithEvents ChDefinedIn As ColumnHeader
    Friend WithEvents ButtonClose As Button
    Friend WithEvents TextFilter As TextBox
End Class
