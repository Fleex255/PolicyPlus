Public Class EditPolStringData
    Public Function PresentDialog(ValueName As String, InitialData As String) As DialogResult
        TextName.Text = ValueName
        TextData.Text = InitialData
        TextData.Select()
        TextData.SelectAll()
        Return ShowDialog()
    End Function
    Private Sub EditPolStringData_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class