Imports System.ComponentModel
Public Class DownloadAdmx
    Const MicrosoftMsiDownloadLink As String = "https://download.microsoft.com/download/4/e/0/4e0fe8d7-ba82-4354-9cc2-18ac02cfd6b5/Administrative%20Templates%20(.admx)%20for%20Windows%2010%20November%202021%20Update.msi"
    Const PolicyDefinitionsMsiSubdirectory As String = "\Microsoft Group Policy\Windows 10 November 2021 Update (21H2)\PolicyDefinitions"
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
        Dim isAdmin = False
        Using identity = Security.Principal.WindowsIdentity.GetCurrent()
            isAdmin = New Security.Principal.WindowsPrincipal(identity).IsInRole(Security.Principal.WindowsBuiltInRole.Administrator)
        End Using
        Dim takeOwnership = Sub(Folder As String)
                                Dim dacl = IO.Directory.GetAccessControl(Folder)
                                Dim adminSid As New Security.Principal.SecurityIdentifier(Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, Nothing)
                                dacl.SetOwner(adminSid)
                                IO.Directory.SetAccessControl(Folder, dacl)
                                dacl = IO.Directory.GetAccessControl(Folder)
                                Dim allowRule As New Security.AccessControl.FileSystemAccessRule(adminSid, Security.AccessControl.FileSystemRights.FullControl, Security.AccessControl.AccessControlType.Allow)
                                dacl.AddAccessRule(allowRule)
                                IO.Directory.SetAccessControl(Folder, dacl)
                            End Sub
        Dim moveFilesInDir = Sub(Source As String, Dest As String, InheritAcl As Boolean)
                                 Dim creatingNew = Not IO.Directory.Exists(Dest)
                                 IO.Directory.CreateDirectory(Dest)
                                 If isAdmin Then
                                     If creatingNew And InheritAcl Then
                                         Dim dirAcl = New Security.AccessControl.DirectorySecurity()
                                         dirAcl.SetAccessRuleProtection(False, True)
                                         IO.Directory.SetAccessControl(Dest, dirAcl)
                                     ElseIf Not creatingNew Then
                                         takeOwnership(Dest)
                                     End If
                                 End If
                                 For Each file In IO.Directory.EnumerateFiles(Source)
                                     Dim plainFilename = IO.Path.GetFileName(file)
                                     Dim newName = IO.Path.Combine(Dest, plainFilename)
                                     If IO.File.Exists(newName) Then IO.File.Delete(newName)
                                     IO.File.Move(file, newName)
                                     If isAdmin Then
                                         Dim fileAcl = New Security.AccessControl.FileSecurity()
                                         fileAcl.SetAccessRuleProtection(False, True)
                                         IO.File.SetAccessControl(newName, fileAcl)
                                     End If
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
                                      If IO.Directory.Exists(destination) And isAdmin Then
                                          failPhase = "take control of the destination"
                                          setProgress("Securing destination...")
                                          Privilege.EnablePrivilege("SeTakeOwnershipPrivilege")
                                          Privilege.EnablePrivilege("SeRestorePrivilege")
                                          takeOwnership(destination)
                                      End If
                                      failPhase = "move the ADMX files"
                                      setProgress("Moving files to destination...")
                                      Dim unpackedDefsPath = unpackPath & PolicyDefinitionsMsiSubdirectory
                                      Dim langSubfolder = Globalization.CultureInfo.CurrentCulture.Name
                                      moveFilesInDir(unpackedDefsPath, destination, False)
                                      Dim sourceAdmlPath = unpackedDefsPath & "\" & langSubfolder
                                      If IO.Directory.Exists(sourceAdmlPath) Then moveFilesInDir(sourceAdmlPath, destination & "\" & langSubfolder, True)
                                      If langSubfolder <> "en-US" Then
                                          ' Also copy the English language files as a fallback
                                          moveFilesInDir(unpackedDefsPath & "\en-US", destination & "\en-US", True)
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