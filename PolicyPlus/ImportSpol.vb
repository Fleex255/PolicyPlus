Public Class ImportSpol
    Public Spol As SpolFile
    Private Sub ButtonOpenFile_Click(sender As Object, e As EventArgs) Handles ButtonOpenFile.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Semantic Policy files|*.spol|All files|*.*"
            If ofd.ShowDialog = DialogResult.OK Then
                TextSpol.Text = IO.File.ReadAllText(ofd.FileName)
            End If
        End Using
    End Sub
    Private Sub ButtonVerify_Click(sender As Object, e As EventArgs) Handles ButtonVerify.Click
        Try
            Dim spol = SpolFile.FromText(TextSpol.Text)
            MsgBox("Validation successful, " & spol.Policies.Count & " policy settings found.", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("SPOL validation failed: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click
        Try
            Spol = SpolFile.FromText(TextSpol.Text) ' Tell the main form that the SPOL is ready to be committed
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MsgBox("The SPOL text is invalid: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub ImportSpol_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Spol = Nothing
    End Sub
    Private Sub ImportSpol_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape And Not (TextSpol.Focused And TextSpol.SelectionLength > 0) Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub TextSpol_KeyDown(sender As Object, e As KeyEventArgs) Handles TextSpol.KeyDown
        If e.KeyCode = Keys.A And e.Control Then TextSpol.SelectAll()
    End Sub
    Private Sub ButtonReset_Click(sender As Object, e As EventArgs) Handles ButtonReset.Click
        If MsgBox("Are you sure you want to reset the text box?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            TextSpol.Text = "Policy Plus Semantic Policy" & vbCrLf & vbCrLf
        End If
    End Sub
End Class