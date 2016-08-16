Public Class OpenPol
    Sub BrowseForPol(DestTextbox As TextBox)
        Using sfd As New SaveFileDialog
            sfd.OverwritePrompt = False
            sfd.Filter = "Registry policy files|*.pol"
            If sfd.ShowDialog = DialogResult.OK Then DestTextbox.Text = sfd.FileName
        End Using
    End Sub
    Private Sub CompOptionsCheckedChanged(sender As Object, e As EventArgs) Handles CompLocalOption.CheckedChanged, CompRegistryOption.CheckedChanged, CompFileOption.CheckedChanged, CompNullOption.CheckedChanged
        Dim textboxActive = CompFileOption.Checked
        CompPolFilenameTextbox.Enabled = textboxActive
        CompFileBrowseButton.Enabled = textboxActive
    End Sub
    Private Sub OpenPol_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        CompLocalOption.Checked = True
        UserLocalOption.Checked = True
        CompPolFilenameTextbox.Text = ""
        UserPolFilenameTextbox.Text = ""
        UserGpoSidTextbox.Text = ""
        UserHivePathTextbox.Text = ""
    End Sub
    Private Sub UserOptionsCheckedChanged(sender As Object, e As EventArgs) Handles UserLocalOption.CheckedChanged, UserRegistryOption.CheckedChanged, UserFileOption.CheckedChanged, UserPerUserGpoOption.CheckedChanged, UserPerUserRegOption.CheckedChanged, UserNullOption.CheckedChanged
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
    Private Sub UserBrowseGpoButton_Click(sender As Object, e As EventArgs) Handles UserBrowseGpoButton.Click
        If OpenUserGpo.ShowDialog = DialogResult.OK Then UserGpoSidTextbox.Text = OpenUserGpo.SelectedSid
    End Sub
End Class