Imports System.Security.Principal
Public Class OpenUserGpo
    Public SelectedSid As String
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        ' Resolve the username to a security identifier (SID)
        Try
            Dim userAccount As New NTAccount(UsernameTextbox.Text)
            Dim sid As SecurityIdentifier = userAccount.Translate(GetType(SecurityIdentifier))
            SidTextbox.Text = sid.ToString
        Catch ex As Exception
            MsgBox("The name could not be translated to a SID.", MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        If SidTextbox.Text = "" And UsernameTextbox.Text <> "" Then
            ' Automatically resolve if the user didn't click Search
            SearchButton_Click(Nothing, Nothing)
            If SidTextbox.Text = "" Then Exit Sub
        End If
        Try
            ' Make sure the SID is valid
            Dim sid As New SecurityIdentifier(SidTextbox.Text)
        Catch ex As Exception
            MsgBox("The SID is not valid. Enter a SID in the lower box, or enter a username in the top box and press Search to translate.", MsgBoxStyle.Exclamation)
            Exit Sub
        End Try
        SelectedSid = SidTextbox.Text
        DialogResult = DialogResult.OK
    End Sub
    Private Sub OpenUserGpo_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SidTextbox.Text = ""
        UsernameTextbox.Text = ""
    End Sub
    Private Sub OpenUserGpo_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class