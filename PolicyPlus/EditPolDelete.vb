Public Class EditPolDelete
    Public Function PresentDialog(ContainerKey As String) As DialogResult
        OptClearFirst.Checked = False
        OptDeleteOne.Checked = False
        OptPurge.Checked = False
        TextKey.Text = ContainerKey
        TextValueName.Text = ""
        Return ShowDialog()
    End Function
    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        If OptClearFirst.Checked Or OptPurge.Checked Then DialogResult = DialogResult.OK
        If OptDeleteOne.Checked Then
            If TextValueName.Text = "" Then
                MsgBox("You must enter a value name.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            DialogResult = DialogResult.OK
        End If
    End Sub
    Private Sub EditPolDelete_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Sub ChoiceChanged(sender As Object, e As EventArgs) Handles OptClearFirst.CheckedChanged, OptDeleteOne.CheckedChanged, OptPurge.CheckedChanged
        TextValueName.Enabled = OptDeleteOne.Checked
    End Sub
End Class