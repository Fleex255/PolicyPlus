Public Class EditPolKey
    Private Sub EditPolKey_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub EditPolKey_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        TextName.Select()
        TextName.SelectAll()
    End Sub
    Public Function PresentDialog(InitialName As String) As String
        TextName.Text = InitialName
        If ShowDialog() = DialogResult.OK Then
            Return TextName.Text
        Else
            Return ""
        End If
    End Function
End Class