Imports System.DirectoryServices.ActiveDirectory
Public Class OpenAdmxFolder
    Dim SysvolPolicyDefinitionsPath As String = ""
    Public SelectedFolder As String
    Public ClearWorkspace As Boolean
    Private Sub Options_CheckedChanged(sender As Object, e As EventArgs) Handles OptCustomFolder.CheckedChanged, OptSysvol.CheckedChanged, OptLocalFolder.CheckedChanged
        Dim customSelected = OptCustomFolder.Checked
        TextFolder.Enabled = customSelected
        ButtonBrowse.Enabled = customSelected
    End Sub
    Private Sub OpenAdmxFolder_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        OptCustomFolder.Checked = True
        Dim compDomain As Domain = Nothing
        Try
            compDomain = Domain.GetComputerDomain
        Catch ex As Exception
            ' Not domain-joined, or no domain controller is available
        End Try
        If compDomain Is Nothing Then
            SysvolPolicyDefinitionsPath = ""
        Else
            Dim possiblePath = "\\" & compDomain.Name & "\SYSVOL\" & compDomain.Name & "\Policies\PolicyDefinitions"
            If IO.Directory.Exists(possiblePath) Then SysvolPolicyDefinitionsPath = possiblePath Else SysvolPolicyDefinitionsPath = ""
        End If
        OptSysvol.Enabled = (SysvolPolicyDefinitionsPath <> "")
    End Sub
    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        If OptLocalFolder.Checked Then
            SelectedFolder = Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions")
        ElseIf OptSysvol.Checked Then
            SelectedFolder = SysvolPolicyDefinitionsPath
        ElseIf OptCustomFolder.Checked Then
            SelectedFolder = TextFolder.Text
        End If
        If IO.Directory.Exists(SelectedFolder) Then
            ClearWorkspace = ClearWorkspaceCheckbox.Checked
            DialogResult = DialogResult.OK
        Else
            MsgBox("The folder you specified does not exist.", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Using fbd As New FolderBrowserDialog
            If fbd.ShowDialog <> DialogResult.OK Then Exit Sub
            TextFolder.Text = fbd.SelectedPath
        End Using
    End Sub
    Private Sub OpenAdmxFolder_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class