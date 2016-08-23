Public Class OpenPol
    Public SelectedUser, SelectedComputer As PolicyLoader
    Sub BrowseForPol(DestTextbox As TextBox)
        Using sfd As New SaveFileDialog
            sfd.OverwritePrompt = False
            sfd.Filter = "Registry policy files|*.pol"
            If sfd.ShowDialog = DialogResult.OK Then DestTextbox.Text = sfd.FileName
        End Using
    End Sub
    Private Sub CompOptionsCheckedChanged(sender As Object, e As EventArgs) Handles CompLocalOption.CheckedChanged, CompRegistryOption.CheckedChanged, CompFileOption.CheckedChanged, CompNullOption.CheckedChanged
        Dim regMount = CompRegistryOption.Checked
        CompRegTextbox.Enabled = regMount
        Dim polActive = CompFileOption.Checked
        CompPolFilenameTextbox.Enabled = polActive
        CompFileBrowseButton.Enabled = polActive
    End Sub
    Private Sub OpenPol_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        CompLocalOption.Checked = True
        UserLocalOption.Checked = True
        CompRegTextbox.Text = "HKLM"
        CompPolFilenameTextbox.Text = ""
        UserRegTextbox.Text = "HKCU"
        UserPolFilenameTextbox.Text = ""
        UserGpoSidTextbox.Text = ""
        UserHivePathTextbox.Text = ""
    End Sub
    Private Sub UserOptionsCheckedChanged(sender As Object, e As EventArgs) Handles UserLocalOption.CheckedChanged, UserRegistryOption.CheckedChanged, UserFileOption.CheckedChanged, UserPerUserGpoOption.CheckedChanged, UserPerUserRegOption.CheckedChanged, UserNullOption.CheckedChanged
        Dim regMount = UserRegistryOption.Checked
        UserRegTextbox.Enabled = regMount
        Dim file = UserFileOption.Checked
        UserPolFilenameTextbox.Enabled = file
        UserFileBrowseButton.Enabled = file
        Dim perUserGpo = UserPerUserGpoOption.Checked
        UserGpoSidTextbox.Enabled = perUserGpo
        UserBrowseGpoButton.Enabled = perUserGpo
        Dim perUserHive = UserPerUserRegOption.Checked
        UserHivePathTextbox.Enabled = perUserHive
        UserBrowseHiveButton.Enabled = perUserHive
    End Sub
    Private Sub CompFileBrowseButton_Click(sender As Object, e As EventArgs) Handles CompFileBrowseButton.Click
        BrowseForPol(CompPolFilenameTextbox)
    End Sub
    Private Sub UserFileBrowseButton_Click(sender As Object, e As EventArgs) Handles UserFileBrowseButton.Click
        BrowseForPol(UserPolFilenameTextbox)
    End Sub
    Private Sub UserBrowseRegistryButton_Click(sender As Object, e As EventArgs) Handles UserBrowseHiveButton.Click
        If OpenUserRegistry.ShowDialog = DialogResult.OK Then UserHivePathTextbox.Text = OpenUserRegistry.SelectedFilePath
    End Sub
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        Try
            If CompLocalOption.Checked Then
                SelectedComputer = New PolicyLoader(PolicyLoaderSource.LocalGpo, "", False)
            ElseIf CompRegistryOption.Checked Then
                SelectedComputer = New PolicyLoader(PolicyLoaderSource.LocalRegistry, CompRegTextbox.Text, False)
            ElseIf CompFileOption.Checked Then
                SelectedComputer = New PolicyLoader(PolicyLoaderSource.PolFile, CompPolFilenameTextbox.Text, False)
            Else
                SelectedComputer = New PolicyLoader(PolicyLoaderSource.Null, "", False)
            End If
        Catch ex As Exception
            MsgBox("The computer policy loader could not be created. " & ex.Message, MsgBoxStyle.Exclamation)
            Exit Sub
        End Try
        Try
            If UserLocalOption.Checked Then
                SelectedUser = New PolicyLoader(PolicyLoaderSource.LocalGpo, "", True)
            ElseIf UserRegistryOption.Checked Then
                SelectedUser = New PolicyLoader(PolicyLoaderSource.LocalRegistry, UserRegTextbox.Text, True)
            ElseIf UserFileOption.Checked Then
                SelectedUser = New PolicyLoader(PolicyLoaderSource.PolFile, UserPolFilenameTextbox.Text, True)
            ElseIf UserPerUserGpoOption.Checked Then
                SelectedUser = New PolicyLoader(PolicyLoaderSource.SidGpo, UserGpoSidTextbox.Text, True)
            ElseIf UserPerUserRegOption.Checked Then
                SelectedUser = New PolicyLoader(PolicyLoaderSource.NtUserDat, UserHivePathTextbox.Text, True)
            Else
                SelectedUser = New PolicyLoader(PolicyLoaderSource.Null, "", True)
            End If
        Catch ex As Exception
            MsgBox("The user policy loader could not be created. " & ex.Message, MsgBoxStyle.Exclamation)
            Exit Sub
        End Try
        DialogResult = DialogResult.OK
    End Sub
    Private Sub UserBrowseGpoButton_Click(sender As Object, e As EventArgs) Handles UserBrowseGpoButton.Click
        If OpenUserGpo.ShowDialog = DialogResult.OK Then UserGpoSidTextbox.Text = OpenUserGpo.SelectedSid
    End Sub
    Private Sub OpenPol_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class