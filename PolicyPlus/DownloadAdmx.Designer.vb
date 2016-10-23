<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DownloadAdmx
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
        Dim LabelWhatsThis As System.Windows.Forms.Label
        Dim LabelDestFolder As System.Windows.Forms.Label
        Me.TextDestFolder = New System.Windows.Forms.TextBox()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ProgressSpinner = New System.Windows.Forms.ProgressBar()
        Me.LabelProgress = New System.Windows.Forms.Label()
        Me.ButtonStart = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        LabelWhatsThis = New System.Windows.Forms.Label()
        LabelDestFolder = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LabelWhatsThis
        '
        LabelWhatsThis.AutoSize = True
        LabelWhatsThis.Location = New System.Drawing.Point(12, 9)
        LabelWhatsThis.Name = "LabelWhatsThis"
        LabelWhatsThis.Size = New System.Drawing.Size(331, 13)
        LabelWhatsThis.TabIndex = 0
        LabelWhatsThis.Text = "Download the full set of policy definitions (ADMX files) from Microsoft."
        '
        'LabelDestFolder
        '
        LabelDestFolder.AutoSize = True
        LabelDestFolder.Location = New System.Drawing.Point(12, 30)
        LabelDestFolder.Name = "LabelDestFolder"
        LabelDestFolder.Size = New System.Drawing.Size(89, 13)
        LabelDestFolder.TabIndex = 3
        LabelDestFolder.Text = "Destination folder"
        '
        'TextDestFolder
        '
        Me.TextDestFolder.Location = New System.Drawing.Point(107, 27)
        Me.TextDestFolder.Name = "TextDestFolder"
        Me.TextDestFolder.Size = New System.Drawing.Size(184, 20)
        Me.TextDestFolder.TabIndex = 1
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(297, 25)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 2
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ProgressSpinner
        '
        Me.ProgressSpinner.Location = New System.Drawing.Point(12, 53)
        Me.ProgressSpinner.Name = "ProgressSpinner"
        Me.ProgressSpinner.Size = New System.Drawing.Size(360, 23)
        Me.ProgressSpinner.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressSpinner.TabIndex = 4
        '
        'LabelProgress
        '
        Me.LabelProgress.AutoSize = True
        Me.LabelProgress.Location = New System.Drawing.Point(12, 87)
        Me.LabelProgress.Name = "LabelProgress"
        Me.LabelProgress.Size = New System.Drawing.Size(48, 13)
        Me.LabelProgress.TabIndex = 5
        Me.LabelProgress.Text = "Progress"
        Me.LabelProgress.Visible = False
        '
        'ButtonStart
        '
        Me.ButtonStart.Location = New System.Drawing.Point(297, 82)
        Me.ButtonStart.Name = "ButtonStart"
        Me.ButtonStart.Size = New System.Drawing.Size(75, 23)
        Me.ButtonStart.TabIndex = 6
        Me.ButtonStart.Text = "Begin"
        Me.ButtonStart.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Location = New System.Drawing.Point(216, 82)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 7
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'DownloadAdmx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 117)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ButtonStart)
        Me.Controls.Add(Me.LabelProgress)
        Me.Controls.Add(Me.ProgressSpinner)
        Me.Controls.Add(LabelDestFolder)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.TextDestFolder)
        Me.Controls.Add(LabelWhatsThis)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DownloadAdmx"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Acquire ADMX Files"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextDestFolder As TextBox
    Friend WithEvents ButtonBrowse As Button
    Friend WithEvents ProgressSpinner As ProgressBar
    Friend WithEvents LabelProgress As Label
    Friend WithEvents ButtonStart As Button
    Friend WithEvents ButtonClose As Button
End Class
