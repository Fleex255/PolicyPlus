Imports System.ComponentModel
Public Class DownloadAdmx
    Const MicrosoftMsiDownloadLink As String = "https://download.microsoft.com/download/0/C/0/0C098953-38C6-4DF7-A2B6-DE10A57A1C55/Windows%2010%20and%20Windows%20Server%202016%20ADMX.msi"
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
                                     IO.File.Move(file, IO.Path.Combine(Dest, plainFilename))
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
                                      failPhase = "move the ADMX files"
                                      setProgress("Moving files to destination...")
                                      Dim langSubfolder = Globalization.CultureInfo.CurrentCulture.Name
                                      moveFilesInDir(unpackPath & "\PolicyDefinitions", destination)
                                      moveFilesInDir(unpackPath & "\PolicyDefinitions\" & langSubfolder, destination & "\" & langSubfolder)
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