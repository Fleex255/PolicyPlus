<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListEditor
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
        Me.EntriesDatagrid = New System.Windows.Forms.DataGridView()
        Me.NameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ValueColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ElementNameLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.OkButton = New System.Windows.Forms.Button()
        CType(Me.EntriesDatagrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EntriesDatagrid
        '
        Me.EntriesDatagrid.AllowUserToResizeRows = False
        Me.EntriesDatagrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EntriesDatagrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.EntriesDatagrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EntriesDatagrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameColumn, Me.ValueColumn})
        Me.EntriesDatagrid.Location = New System.Drawing.Point(12, 25)
        Me.EntriesDatagrid.Name = "EntriesDatagrid"
        Me.EntriesDatagrid.Size = New System.Drawing.Size(404, 170)
        Me.EntriesDatagrid.TabIndex = 0
        '
        'NameColumn
        '
        Me.NameColumn.HeaderText = "Name"
        Me.NameColumn.Name = "NameColumn"
        '
        'ValueColumn
        '
        Me.ValueColumn.HeaderText = "Value"
        Me.ValueColumn.Name = "ValueColumn"
        '
        'ElementNameLabel
        '
        Me.ElementNameLabel.AutoEllipsis = True
        Me.ElementNameLabel.AutoSize = True
        Me.ElementNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.ElementNameLabel.MaximumSize = New System.Drawing.Size(400, 0)
        Me.ElementNameLabel.Name = "ElementNameLabel"
        Me.ElementNameLabel.Size = New System.Drawing.Size(92, 13)
        Me.ElementNameLabel.TabIndex = 1
        Me.ElementNameLabel.Text = "List element name"
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(341, 201)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 2
        Me.CloseButton.Text = "Cancel"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkButton.Location = New System.Drawing.Point(260, 201)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 1
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'ListEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(428, 236)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ElementNameLabel)
        Me.Controls.Add(Me.EntriesDatagrid)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(444, 275)
        Me.Name = "ListEditor"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit List"
        CType(Me.EntriesDatagrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EntriesDatagrid As DataGridView
    Friend WithEvents NameColumn As DataGridViewTextBoxColumn
    Friend WithEvents ValueColumn As DataGridViewTextBoxColumn
    Friend WithEvents ElementNameLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents OkButton As Button
End Class
