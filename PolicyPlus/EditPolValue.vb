Imports Microsoft.Win32
Public Class EditPolValue
    Public SelectedKind As RegistryValueKind
    Public ChosenName As String
    Private Sub EditPolValueType_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Public Function PresentDialog() As DialogResult
        TextName.Text = ""
        ComboKind.SelectedIndex = 0
        Dim dlgRes = ShowDialog()
        If dlgRes = DialogResult.OK Then
            Select Case ComboKind.SelectedIndex
                Case 0
                    SelectedKind = RegistryValueKind.String
                Case 1
                    SelectedKind = RegistryValueKind.ExpandString
                Case 2
                    SelectedKind = RegistryValueKind.MultiString
                Case 3
                    SelectedKind = RegistryValueKind.DWord
                Case 4
                    SelectedKind = RegistryValueKind.QWord
            End Select
            ChosenName = TextName.Text
        End If
        Return dlgRes
    End Function
End Class