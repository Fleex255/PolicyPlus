Imports Microsoft.Win32
Public Class PolicyLoader
    Dim SourceType As PolicyLoaderSource
    Dim OriginalArgument As String
    Dim User As Boolean ' Whether this is for a user policy source
    Dim SourceObject As IPolicySource
    Dim MainSourcePath As String ' Path to the POL file or NTUSER.DAT
    Dim MainSourceRegKey As RegistryKey ' The hive key, or the mounted hive file
    Dim GptIniPath As String ' Path to the gpt.ini file, used to increment the version
    Dim Writable As Boolean
    Public Sub New(Source As PolicyLoaderSource, Argument As String, IsUser As Boolean)
        SourceType = Source
        User = IsUser
        OriginalArgument = Argument
        ' Parse the argument and open the physical resource
        Select Case Source
            Case PolicyLoaderSource.LocalGpo
                MainSourcePath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%\System32\GroupPolicy\" & If(IsUser, "User", "Machine") & "\Registry.pol")
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
        ' Create an IPolicySource so PolicyProcessing can work
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
                Privilege.EnablePrivilege("SeBackupPrivilege")
                Privilege.EnablePrivilege("SeRestorePrivilege")
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
                    ' Open a POL file
                    Try
                        Using fPol As New IO.FileStream(MainSourcePath, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                            Writable = True
                        End Using
                    Catch ex As Exception
                        Writable = False
                    End Try
                    SourceObject = PolFile.Load(MainSourcePath)
                Else
                    ' Create a new POL file
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
                ' Figure out whether this edition can handle Group Policy application by itself
                If HasGroupPolicyInfrastructure() Then
                    PInvoke.RefreshPolicyEx(Not User, 0)
                    Return "saved to disk and invoked policy refresh"
                Else
                    pol.ApplyDifference(oldPol, RegistryPolicyProxy.EncapsulateKey(If(User, RegistryHive.CurrentUser, RegistryHive.LocalMachine)))
                    PInvoke.SendNotifyMessageW(New IntPtr(&HFFFF), &H1A, UIntPtr.Zero, IntPtr.Zero) ' Broadcast WM_SETTINGCHANGE
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
        ' Get the path to the comments file, or nothing if comments don't work
        If SourceType = PolicyLoaderSource.PolFile Or SourceType = PolicyLoaderSource.NtUserDat Then
            Return IO.Path.ChangeExtension(MainSourcePath, "cmtx")
        ElseIf SourceType = PolicyLoaderSource.LocalRegistry Then
            Return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\Policy Plus\Reg" & If(User, "User", "Machine") & ".cmtx")
        ElseIf MainSourcePath <> "" Then
            Return IO.Path.Combine(IO.Path.GetDirectoryName(MainSourcePath), "comment.cmtx")
        Else
            Return ""
        End If
    End Function
    Public Function GetWritability() As PolicySourceWritability
        ' Get whether the source can be updated
        If SourceType = PolicyLoaderSource.Null Then
            Return PolicySourceWritability.Writable
        ElseIf SourceType = PolicyLoaderSource.LocalRegistry Then
            Return If(Writable, PolicySourceWritability.Writable, PolicySourceWritability.NoWriting)
        Else
            Return If(Writable, PolicySourceWritability.Writable, PolicySourceWritability.NoCommit)
        End If
    End Function
    Private Sub UpdateGptIni()
        ' Increment the version number in gpt.ini
        Const MachExtensionsLine = "gPCMachineExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F72-3407-48AE-BA88-E8213C6761F1}]"
        Const UserExtensionsLine = "gPCUserExtensionNames=[{35378EAC-683F-11D2-A89A-00C04FBBCFA2}{D02B1F73-3407-48AE-BA88-E8213C6761F1}]"
        If IO.File.Exists(GptIniPath) Then
            ' Alter the existing gpt.ini's Version line and add any necessary other lines
            Dim lines = IO.File.ReadLines(GptIniPath).ToList
            Using fGpt As New IO.StreamWriter(GptIniPath, False)
                Dim seenMachExts, seenUserExts, seenVersion As Boolean
                For Each line In lines
                    If line.StartsWith("Version", StringComparison.InvariantCultureIgnoreCase) Then
                        Dim curVersion As Integer = Split(line, "=", 2)(1)
                        curVersion += If(User, &H10000, 1)
                        fGpt.WriteLine("Version=" & curVersion)
                        seenVersion = True
                    Else
                        fGpt.WriteLine(line)
                        If line.StartsWith("gPCMachineExtensionNames=", StringComparison.InvariantCultureIgnoreCase) Then seenMachExts = True
                        If line.StartsWith("gPCUserExtensionNames=", StringComparison.InvariantCultureIgnoreCase) Then seenUserExts = True
                    End If
                Next
                If Not seenVersion Then fGpt.WriteLine("Version=" & &H10001)
                If Not seenMachExts Then fGpt.WriteLine(MachExtensionsLine)
                If Not seenUserExts Then fGpt.WriteLine(UserExtensionsLine)
            End Using
        Else
            ' Create a new gpt.ini
            Using fGpt As New IO.StreamWriter(GptIniPath)
                fGpt.WriteLine("[General]")
                fGpt.WriteLine(MachExtensionsLine)
                fGpt.WriteLine(UserExtensionsLine)
                fGpt.WriteLine("Version=" & &H10001)
            End Using
        End If
    End Sub
    Public ReadOnly Property Source As PolicyLoaderSource
        Get
            Return SourceType
        End Get
    End Property
    Public ReadOnly Property LoaderData As String
        Get
            Return OriginalArgument
        End Get
    End Property
    Public Function GetDisplayInfo() As String
        ' Get the human-readable name of the loader for display in the status bar
        Dim name As String = ""
        Select Case SourceType
            Case PolicyLoaderSource.LocalGpo
                name = "Local GPO"
            Case PolicyLoaderSource.LocalRegistry
                name = "Registry"
            Case PolicyLoaderSource.PolFile
                name = "File"
            Case PolicyLoaderSource.SidGpo
                name = "User GPO"
            Case PolicyLoaderSource.NtUserDat
                name = "User hive"
            Case PolicyLoaderSource.Null
                name = "Scratch space"
        End Select
        If OriginalArgument <> "" Then Return name & " (" & OriginalArgument & ")" Else Return name
    End Function
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