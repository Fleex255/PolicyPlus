Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Public Class PolicyLoader
    Dim SourceType As PolicyLoaderSource
    Dim User As Boolean ' Whether this is for a user policy source
    Dim SourceObject As IPolicySource
    Dim MainSourcePath As String ' Path to the POL file or NTUSER.DAT
    Dim MainSourceRegKey As RegistryKey ' The hive key, or the mounted hive file
    Dim GptIniPath As String ' Path to the gpt.ini file, used to increment the version
    Dim Writable As Boolean
    Public Sub New(Source As PolicyLoaderSource, Argument As String, IsUser As Boolean)
        SourceType = Source
        User = IsUser
        Select Case Source
            Case PolicyLoaderSource.LocalGpo
                MainSourcePath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\GroupPolicy\" & IIf(IsUser, "User", "Machine") & "\Registry.pol")
                GptIniPath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\GroupPolicy\gpt.ini")
            Case PolicyLoaderSource.LocalRegistry
                Dim pathParts = Split(Argument, "\", 2)
                Dim baseName = pathParts(0).ToLowerInvariant
                Dim baseKey As RegistryKey
                If baseName = "hkcu" Or baseName = "hkey_current_user" Then
                    baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                ElseIf baseName = "hku" Or baseName = "hkey_users" Then
                    baseKey = RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Default)
                ElseIf baseName = "hklm" Or baseName = "hkey_local_machine" Then
                    baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
                Else
                    Throw New Exception("The root key is not valid.")
                End If
                If pathParts.Length = 2 Then
                    MainSourceRegKey = baseKey.CreateSubKey(pathParts(1))
                Else
                    MainSourceRegKey = baseKey
                End If
            Case PolicyLoaderSource.PolFile
                MainSourcePath = Argument
            Case PolicyLoaderSource.SidGpo
                MainSourcePath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\GroupPolicyUsers\" & Argument & "\User\Registry.pol")
                GptIniPath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\GroupPolicyUsers\" & Argument & "\gpt.ini")
            Case PolicyLoaderSource.NtUserDat
                MainSourcePath = Argument
            Case PolicyLoaderSource.Null
                MainSourcePath = ""
        End Select
    End Sub
    Public Function OpenSource() As IPolicySource
        Select Case SourceType
            Case PolicyLoaderSource.LocalRegistry
                Dim regPol = RegistryPolicyProxy.EncapsulateKey(MainSourceRegKey)
                Try
                    regPol.SetValue("Software\Policies", "_PolicyPlusSecCheck", "Testing to see whether Policy Plus can write to policy keys", RegistryValueKind.String)
                    regPol.DeleteValue("Software\Policies", "_PolicyPlusSecCheck")
                    Writable = True
                Catch ex As Exception
                    Writable = False
                End Try
                SourceObject = regPol
            Case PolicyLoaderSource.NtUserDat
                ' Turn on the backup and restore privileges to allow the use of RegLoadKey
                Dim restoreLuid, backupLuid As PInvokeLuid
                Dim restorePriv, backupPriv As PInvokeTokenPrivileges
                Dim thisProcToken As IntPtr
                PInvoke.OpenProcessToken(PInvoke.GetCurrentProcess, &H28, thisProcToken) ' 0x28 = TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY
                PInvoke.LookupPrivilegeValueW(Nothing, "SeRestorePrivilege", restoreLuid)
                PInvoke.LookupPrivilegeValueW(Nothing, "SeBackupPrivilege", backupLuid)
                restorePriv.PrivilegeCount = 1
                restorePriv.Attributes = 2 ' SE_PRIVILEGE_ENABLED
                restorePriv.LUID = restoreLuid
                backupPriv.PrivilegeCount = 1
                backupPriv.Attributes = 2
                backupPriv.LUID = backupLuid
                PInvoke.AdjustTokenPrivileges(thisProcToken, False, restorePriv, Marshal.SizeOf(restorePriv), IntPtr.Zero, 0)
                PInvoke.AdjustTokenPrivileges(thisProcToken, False, backupPriv, Marshal.SizeOf(backupPriv), IntPtr.Zero, 0)
                PInvoke.CloseHandle(thisProcToken)
                ' Load the hive
                Using machHive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
                    Dim subkeyName = "PolicyPlusMount:" & Guid.NewGuid().ToString
                    PInvoke.RegLoadKeyW(New IntPtr(&H80000002), subkeyName, MainSourcePath) ' HKEY_LOCAL_MACHINE
                    MainSourceRegKey = machHive.OpenSubKey(subkeyName, True)
                    If MainSourceRegKey Is Nothing Then
                        Writable = False
                        SourceObject = New PolFile
                        Return SourceObject
                    Else
                        Writable = True
                    End If
                End Using
                SourceObject = RegistryPolicyProxy.EncapsulateKey(MainSourceRegKey)
            Case PolicyLoaderSource.Null
                SourceObject = New PolFile
            Case Else
                If IO.File.Exists(MainSourcePath) Then
                    Try
                        Using fPol As New IO.FileStream(MainSourcePath, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                            Writable = True
                        End Using
                    Catch ex As Exception
                        Writable = False
                    End Try
                    SourceObject = PolFile.Load(MainSourcePath)
                Else
                    Try
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(MainSourcePath))
                        Dim pol As New PolFile
                        pol.Save(MainSourcePath)
                        SourceObject = pol
                        Writable = True
                    Catch ex As Exception
                        SourceObject = New PolFile
                        Writable = False
                    End Try
                End If
        End Select
        Return SourceObject
    End Function
    Public Function Close() As Boolean ' Whether cleanup was successful
        If SourceType = PolicyLoaderSource.NtUserDat And TypeOf SourceObject Is RegistryPolicyProxy Then
            Dim subkeyName = Split(MainSourceRegKey.Name, "\", 2)(1) ' Remove the host hive name
            MainSourceRegKey.Dispose()
            Return PInvoke.RegUnLoadKeyW(New IntPtr(&H80000002), subkeyName) = 0
        End If
        Return True
    End Function
    Public Function Save() As String ' Returns human-readable info on what happened
        Select Case SourceType
            Case PolicyLoaderSource.LocalGpo
                Dim oldPol As PolFile
                If IO.File.Exists(MainSourcePath) Then oldPol = PolFile.Load(MainSourcePath) Else oldPol = New PolFile
                Dim pol = CType(SourceObject, PolFile)
                pol.Save(MainSourcePath)
                UpdateGptIni()
                ' secpol.msc only exists on Pro editions (not testing for gpedit.msc because people like to move it from Pro to Home)
                If IO.File.Exists(Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\secpol.msc")) Then
                    PInvoke.RefreshPolicyEx(Not User, 0)
                    Return "saved to disk and invoked policy refresh"
                Else
                    pol.ApplyDifference(oldPol, RegistryPolicyProxy.EncapsulateKey(IIf(User, RegistryHive.CurrentUser, RegistryHive.LocalMachine)))
                    Return "saved to disk and applied diff to Registry"
                End If
            Case PolicyLoaderSource.LocalRegistry
                Return "already applied"
            Case PolicyLoaderSource.NtUserDat
                Return "will apply when policy source is closed"
            Case PolicyLoaderSource.Null
                Return "discarded"
            Case PolicyLoaderSource.PolFile
                CType(SourceObject, PolFile).Save(MainSourcePath)
                Return "saved to disk"
            Case PolicyLoaderSource.SidGpo
                CType(SourceObject, PolFile).Save(MainSourcePath)
                UpdateGptIni()
                PInvoke.RefreshPolicyEx(False, 0)
                Return "saved to disk and invoked policy refresh"
        End Select
        Return ""
    End Function
    Public Function GetCmtxPath() As String
        If SourceType = PolicyLoaderSource.PolFile Or SourceType = PolicyLoaderSource.NtUserDat Then
            Return IO.Path.ChangeExtension(MainSourcePath, "cmtx")
        ElseIf SourceType = PolicyLoaderSource.LocalRegistry Then
            Return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\Policy Plus\Reg" & IIf(User, "User", "Machine") & ".cmtx")
        ElseIf MainSourcePath <> "" Then
            Return IO.Path.Combine(IO.Path.GetDirectoryName(MainSourcePath), "comment.cmtx")
        Else
            Return ""
        End If
    End Function
    Public Function GetWritability() As PolicySourceWritability
        If SourceType = PolicyLoaderSource.Null Then
            Return PolicySourceWritability.Writable
        ElseIf SourceType = PolicyLoaderSource.LocalRegistry Then
            Return IIf(Writable, PolicySourceWritability.Writable, PolicySourceWritability.NoWriting)
        Else
            Return IIf(Writable, PolicySourceWritability.Writable, PolicySourceWritability.NoCommit)
        End If
    End Function
    Private Sub UpdateGptIni()
        If IO.File.Exists(GptIniPath) Then
            Dim lines = IO.File.ReadLines(GptIniPath).ToList
            Using fGpt As New IO.StreamWriter(GptIniPath, False)
                For Each line In lines
                    If line.StartsWith("Version", StringComparison.InvariantCultureIgnoreCase) Then
                        Dim curVersion As Integer = Split(line, "=", 2)(1)
                        curVersion += IIf(User, &H10000, 1)
                        fGpt.WriteLine("Version=" & curVersion)
                    Else
                        fGpt.WriteLine(line)
                    End If
                Next
            End Using
        Else
            Using fGpt As New IO.StreamWriter(GptIniPath)
                fGpt.WriteLine("[General]")
                fGpt.WriteLine("gPCMachineExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F72-3407-48AE-BA88-E8213C6761F1}]")
                fGpt.WriteLine("Version=" & &H10001)
                fGpt.WriteLine("gPCUserExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F73-3407-48AE-BA88-E8213C6761F1}]")
            End Using
        End If
    End Sub
End Class
Public Enum PolicyLoaderSource
    LocalGpo
    LocalRegistry
    PolFile
    SidGpo
    NtUserDat
    Null
End Enum
Public Enum PolicySourceWritability
    Writable ' Full writability
    NoCommit ' Enable the OK button, but don't try to save
    NoWriting ' Disable the OK button (there's no buffer)
End Enum