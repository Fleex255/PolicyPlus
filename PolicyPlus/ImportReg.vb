Public Class ImportReg
    Dim PolicySource As IPolicySource
    Public Function PresentDialog(Target As IPolicySource) As DialogResult
        TextReg.Text = ""
        TextRoot.Text = ""
        PolicySource = Target
        Return ShowDialog()
    End Function
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Registry scripts|*.reg"
            If ofd.ShowDialog() <> DialogResult.OK Then Exit Sub
            TextReg.Text = ofd.FileName
            If TextRoot.Text = "" Then
                Try
                    Dim reg = RegFile.Load(ofd.FileName, "")
                    TextRoot.Text = reg.GuessPrefix()
                    If reg.HasDefaultValues Then MsgBox("This REG file contains data for default values, which cannot be applied to all policy sources.", MsgBoxStyle.Exclamation)
                Catch ex As Exception
                    MsgBox("An error occurred while trying to guess the prefix.", MsgBoxStyle.Exclamation)
                End Try
            End If
        End Using
    End Sub
    Private Sub ImportReg_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        If TextReg.Text = "" Then
            MsgBox("Please specify a REG file to import.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If TextRoot.Text = "" Then
            MsgBox("Please specify the prefix used to fully qualify paths in the REG file.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Try
            Dim reg = RegFile.Load(TextReg.Text, TextRoot.Text)
            reg.Apply(PolicySource)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MsgBox("Failed to import the REG file.", MsgBoxStyle.Exclamation)
        End Try
    End Sub
End Class