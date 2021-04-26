Imports System.ComponentModel
Public Class DownloadAdmx
    Const MicrosoftMsiDownloadLink As String = "https://download.microsoft.com/download/3/0/6/30680643-987a-450c-b906-a455fff4aee8/Administrative%20Templates%20(.admx)%20for%20Windows%2010%20October%202020%20Update.msi"
    Const PolicyDefinitionsMsiSubdirectory As String = "\Microsoft Group Policy\Windows 10 October 2020 Update (20H2)\PolicyDefinitions"
    Dim Downloading As Boolean = False
    Public NewPolicySourceFolder As String
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Using fbd As New FolderBrowserDialog
            If fbd.ShowDialog = DialogResult.OK Then
                TextDestFolder.Text = fbd.SelectedPath
            End If
        End Using
    End Sub
    Private Sub DownloadAdmx_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Downloading Then e.Cancel = True
    End Sub
    Private Sub DownloadAdmx_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        TextDestFolder.Text = Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions")
        NewPolicySourceFolder = ""
        SetIsBusy(False)
    End Sub
    Sub SetIsBusy(Busy As Boolean)
        Downloading = Busy
        LabelProgress.Visible = Busy
        ButtonClose.Enabled = Not Busy
        ButtonStart.Enabled = Not Busy
        TextDestFolder.Enabled = Not Busy
        ButtonBrowse.Enabled = Not Busy
        ProgressSpinner.MarqueeAnimationSpeed = If(Busy, 100, 0)
        ProgressSpinner.Style = If(Busy, ProgressBarStyle.Marquee, ProgressBarStyle.Continuous)
        ProgressSpinner.Value = 0
    End Sub
    Private Sub ButtonStart_Click(sender As Object, e As EventArgs) Handles ButtonStart.Click
        Dim setProgress = Sub(Progress As String) Invoke(Sub() LabelProgress.Text = Progress)
        LabelProgress.Text = ""
        SetIsBusy(True)
        Dim destination = TextDestFolder.Text
        Dim moveFilesInDir = Sub(Source As String, Dest As String)
                                 IO.Directory.CreateDirectory(Dest)
                                 For Each file In IO.Directory.EnumerateFiles(Source)
                                     Dim plainFilename = IO.Path.GetFileName(file)
                                     Dim newName = IO.Path.Combine(Dest, plainFilename)
                                     If IO.File.Exists(newName) Then IO.File.Delete(newName)
                                     IO.File.Move(file, newName)
                                 Next
                             End Sub
        Task.Factory.StartNew(Sub()
                                  Dim failPhase As String = "create a scratch space"
                                  Try
                                      Dim tempPath = Environment.ExpandEnvironmentVariables("%localappdata%\PolicyPlusAdmxDownload\")
                                      IO.Directory.CreateDirectory(tempPath)
                                      failPhase = "download the package"
                                      setProgress("Downloading MSI from Microsoft...")
                                      Dim downloadPath = tempPath & "W10Admx.msi"
                                      Using webcli As New Net.WebClient
                                          webcli.DownloadFile(MicrosoftMsiDownloadLink, downloadPath)
                                      End Using
                                      failPhase = "extract the package"
                                      setProgress("Unpacking MSI...")
                                      Dim unpackPath = tempPath & "MsiUnpack"
                                      Dim proc = Process.Start("msiexec", "/a """ & downloadPath & """ /quiet /qn TARGETDIR=""" & unpackPath & """")
                                      proc.WaitForExit()
                                      If proc.ExitCode <> 0 Then Throw New Exception ' msiexec failed
                                      IO.File.Delete(downloadPath)
                                      If IO.Directory.Exists(destination) Then
                                          failPhase = "take control of the destination"
                                          setProgress("Securing destination...")
                                          Privilege.EnablePrivilege("SeTakeOwnershipPrivilege")
                                          Privilege.EnablePrivilege("SeRestorePrivilege")
                                          Dim dacl = IO.Directory.GetAccessControl(destination)
                                          Dim adminSid As New Security.Principal.SecurityIdentifier(Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, Nothing)
                                          dacl.SetOwner(adminSid)
                                          Dim allowRule As New Security.AccessControl.FileSystemAccessRule(adminSid, Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
                                          dacl.AddAccessRule(allowRule)
                                          IO.Directory.SetAccessControl(destination, dacl)
                                      End If
                                      failPhase = "move the ADMX files"
                                      setProgress("Moving files to destination...")
                                      Dim unpackedDefsPath = unpackPath & PolicyDefinitionsMsiSubdirectory
                                      Dim langSubfolder = Globalization.CultureInfo.CurrentCulture.Name
                                      moveFilesInDir(unpackedDefsPath, destination)
                                      Dim sourceAdmlPath = unpackedDefsPath & "\" & langSubfolder
                                      If IO.Directory.Exists(sourceAdmlPath) Then moveFilesInDir(sourceAdmlPath, destination & "\" & langSubfolder)
                                      If langSubfolder <> "en-US" Then
                                          ' Also copy the English language files as a fallback
                                          moveFilesInDir(unpackedDefsPath & "\en-US", destination & "\en-US")
                                      End If
                                      failPhase = "remove temporary files"
                                      setProgress("Cleaning up...")
                                      IO.Directory.Delete(tempPath, True)
                                      setProgress("Done.")
                                      Invoke(Sub()
                                                 SetIsBusy(False)
                                                 If MsgBox("ADMX files downloaded successfully. Open them now?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                                                     NewPolicySourceFolder = destination
                                                 End If
                                                 DialogResult = DialogResult.OK
                                             End Sub)
                                  Catch ex As Exception
                                      Invoke(Sub()
                                                 SetIsBusy(False)
                                                 MsgBox("Failed to " & failPhase & ".", MsgBoxStyle.Exclamation)
                                             End Sub)
                                  End Try
                              End Sub)
    End Sub
End Class