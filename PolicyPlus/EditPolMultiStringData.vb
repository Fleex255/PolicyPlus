Public Class EditPolMultiStringData
    Private Sub EditPolMultiStringData_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Public Function PresentDialog(ValueName As String, InitialData As String()) As DialogResult
        TextName.Text = ValueName
        TextData.Lines = InitialData
        TextData.Select()
        Return ShowDialog()
    End Function
End Class