Public Class LanguageOptions
    Private OriginalLanguage As String
    Public NewLanguage As String
    Public Function PresentDialog(CurrentLanguage As String) As DialogResult
        OriginalLanguage = CurrentLanguage
        TextAdmlLanguage.Text = CurrentLanguage
        Return ShowDialog()
    End Function
    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        Dim selection = TextAdmlLanguage.Text.Trim()
        If selection.Split("-"c).Length <> 2 Then
            MsgBox("Please enter a valid language code.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If selection = OriginalLanguage Then
            DialogResult = DialogResult.Cancel
        Else
            NewLanguage = selection
            DialogResult = DialogResult.OK
        End If
    End Sub
End Class