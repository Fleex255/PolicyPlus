Public Class ExportReg
    Dim Source As PolFile
    Public Function PresentDialog(Branch As String, Pol As PolFile, IsUser As Boolean) As DialogResult
        Source = Pol
        TextBranch.Text = Branch
        TextRoot.Text = If(IsUser, "HKEY_CURRENT_USER\", "HKEY_LOCAL_MACHINE\")
        TextReg.Text = ""
        Return ShowDialog()
    End Function
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Using sfd As New SaveFileDialog
            sfd.Filter = "Registry scripts|*.reg"
            If sfd.ShowDialog = DialogResult.OK Then TextReg.Text = sfd.FileName
        End Using
    End Sub
    Private Sub ExportReg_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        If TextReg.Text = "" Then
            MsgBox("Please specify a filename and path for the exported REG.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim reg As New RegFile
        reg.SetPrefix(TextRoot.Text)
        reg.SetSourceBranch(TextBranch.Text)
        Try
            Source.Apply(reg)
            reg.Save(TextReg.Text)
            MsgBox("REG exported successfully.", MsgBoxStyle.Information)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MsgBox("Failed to export REG!", MsgBoxStyle.Exclamation)
        End Try
    End Sub
End Class