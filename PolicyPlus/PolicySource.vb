Imports Microsoft.Win32
Imports System.IO
Public Interface IPolicySource
    Function ContainsValue(Key As String, Value As String) As Boolean
    Function GetValue(Key As String, Value As String) As Object
    Function WillDeleteValue(Key As String, Value As String) As Boolean
    Function GetValueNames(Key As String) As List(Of String)
    Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind)
    Sub ForgetValue(Key As String, Value As String) ' Stop keeping track of a value
    Sub DeleteValue(Key As String, Value As String) ' Mark a value as queued for deletion
    Sub ClearKey(Key As String) ' Destroy all values in a key
    Sub ForgetKeyClearance(Key As String) ' Unmark a key as cleared
End Interface
Public Class PolFile
    Implements IPolicySource
    ' The sortedness is important because key clearances need to be processed before the addition of their values
    ' Fortunately, clearing entries start with **, which sorts before normal values
    Private ReadOnly Entries As New SortedDictionary(Of String, PolEntryData) ' Keys are lowercase Registry keys and values, separated by "\\"
    Private ReadOnly CasePreservation As New Dictionary(Of String, String) ' Keep track of the original cases
    Private Function GetDictKey(Key As String, Value As String) As String
        Dim origCase = Key & "\\" & Value
        Dim lowerCase = origCase.ToLowerInvariant
        If Not CasePreservation.ContainsKey(lowerCase) Then CasePreservation.Add(lowerCase, origCase)
        Return lowerCase
    End Function
    Public Shared Function Load(File As String) As PolFile
        Using fPol As New FileStream(File, FileMode.Open, FileAccess.Read), binary As New BinaryReader(fPol)
            Return Load(binary)
        End Using
    End Function
    Public Shared Function Load(Stream As BinaryReader) As PolFile
        Dim pol As New PolFile
        If Stream.ReadUInt32() <> &H67655250 Then Throw New InvalidDataException("Missing PReg signature")
        If Stream.ReadUInt32() <> 1 Then Throw New InvalidDataException("Unknown (newer) version of POL format")
        Dim readSz = Function() As String ' Read a null-terminated UTF-16LE string
                         Dim sb As New Text.StringBuilder
                         Do
                             Dim charCode As Integer = Stream.ReadUInt16
                             If charCode = 0 Then Exit Do
                             sb.Append(ChrW(charCode))
                         Loop
                         Return sb.ToString
                     End Function
        Do Until Stream.BaseStream.Position = Stream.BaseStream.Length
            Dim ped As New PolEntryData
            Stream.BaseStream.Position += 2 ' Skip the "[" character
            Dim key = readSz()
            Stream.BaseStream.Position += 2 ' Skip ";"
            Dim value = readSz()
            If Stream.ReadUInt16 <> Asc(";"c) Then Stream.BaseStream.Position += 2 ' MS documentation indicates there might be an extra null before the ";" after the value name
            ped.Kind = Stream.ReadInt32
            Stream.BaseStream.Position += 2 ' ";"
            Dim length = Stream.ReadUInt32
            Stream.BaseStream.Position += 2 ' ";"
            Dim data(length - 1) As Byte
            Stream.Read(data, 0, length)
            ped.Data = data
            Stream.BaseStream.Position += 2 ' "]"
            pol.Entries.Add(pol.GetDictKey(key, value), ped)
        Loop
        Return pol
    End Function
    Public Sub Save(File As String)
        Using fPol As New FileStream(File, FileMode.Create), binary As New BinaryWriter(fPol, Text.Encoding.Unicode)
            Save(binary)
        End Using
    End Sub
    Public Sub Save(Writer As BinaryWriter)
        Dim writeSz = Sub(Text As String)
                          For Each c In Text
                              Writer.Write(c)
                          Next
                          Writer.Write(0S)
                      End Sub
        Writer.Write(&H67655250UI)
        Writer.Write(1)
        For Each kv In Entries
            Writer.Write("["c)
            Dim pathparts = Split(CasePreservation(kv.Key), "\\", 2)
            writeSz(pathparts(0)) ' Key name
            Writer.Write(";"c)
            writeSz(pathparts(1)) ' Value name
            Writer.Write(";"c)
            Writer.Write(kv.Value.Kind)
            Writer.Write(";"c)
            Writer.Write(kv.Value.Data.Length)
            Writer.Write(";"c)
            Writer.Write(kv.Value.Data)
            Writer.Write("]"c)
        Next
    End Sub
    Public Sub DeleteValue(Key As String, Value As String) Implements IPolicySource.DeleteValue
        ForgetValue(Key, Value)
        If Not WillDeleteValue(Key, Value) Then
            Dim ped = PolEntryData.FromDword(32) ' It's what Microsoft does
            Entries.Add(GetDictKey(Key, "**del." & Value), ped)
        End If
    End Sub
    Public Sub ForgetValue(Key As String, Value As String) Implements IPolicySource.ForgetValue
        Dim dictKey = GetDictKey(Key, Value)
        If Entries.ContainsKey(dictKey) Then Entries.Remove(dictKey)
        Dim deleterKey = GetDictKey(Key, "**del." & Value)
        If Entries.ContainsKey(deleterKey) Then Entries.Remove(deleterKey)
    End Sub
    Public Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind) Implements IPolicySource.SetValue
        Dim dictKey = GetDictKey(Key, Value)
        If Entries.ContainsKey(dictKey) Then Entries.Remove(dictKey)
        Entries.Add(dictKey, PolEntryData.FromArbitrary(Data, DataType))
    End Sub
    Public Function ContainsValue(Key As String, Value As String) As Boolean Implements IPolicySource.ContainsValue
        If WillDeleteValue(Key, Value) Then Return False
        Return Entries.ContainsKey(GetDictKey(Key, Value))
    End Function
    Public Function GetValue(Key As String, Value As String) As Object Implements IPolicySource.GetValue
        If Not ContainsValue(Key, Value) Then Return Nothing
        Return Entries(GetDictKey(Key, Value)).AsArbitrary
    End Function
    Public Function WillDeleteValue(Key As String, Value As String) As Boolean Implements IPolicySource.WillDeleteValue
        Dim willDelete = False
        Dim keyRoot = GetDictKey(Key, "")
        For Each kv In Entries.Where(Function(e) e.Key.StartsWith(keyRoot))
            If kv.Key = GetDictKey(Key, "**del." & Value) Then
                willDelete = True
            ElseIf kv.Key.StartsWith(GetDictKey(Key, "**delvals")) Then ' MS POL files also use "**delvals."
                willDelete = True
            ElseIf kv.Key = GetDictKey(Key, "**deletevalues") Then
                Dim lowerVal = Value.ToLowerInvariant
                Dim deletedValues = Split(kv.Value.AsString, ";")
                If deletedValues.Any(Function(s) s.ToLowerInvariant = lowerVal) Then willDelete = True
            ElseIf kv.Key = GetDictKey(Key, Value) Then
                willDelete = False ' In case the key is cleared out before setting the values
            End If
        Next
        Return willDelete
    End Function
    Public Function GetValueNames(Key As String) As List(Of String) Implements IPolicySource.GetValueNames
        Return GetValueNames(Key, True)
    End Function
    Public Function GetValueNames(Key As String, OnlyValues As Boolean) As List(Of String)
        Dim prefix = GetDictKey(Key, "")
        Dim valNames As New List(Of String)
        For Each k In Entries.Keys
            If k.StartsWith(prefix) Then
                Dim valName = Split(CasePreservation(k), "\\", 2)(1)
                If Not (OnlyValues And valName.StartsWith("**")) Then valNames.Add(valName)
            End If
        Next
        Return valNames
    End Function
    Public Sub ApplyDifference(OldVersion As PolFile, Target As IPolicySource)
        ' Figure out which values have changed and commit only the changes
        If OldVersion Is Nothing Then OldVersion = New PolFile
        Dim oldEntries = OldVersion.Entries.Keys.Where(Function(k) Not k.Contains("\\**")).ToList
        For Each kv In Entries
            Dim parts = Split(kv.Key, "\\", 2) ' Key, value
            Dim casedParts = Split(CasePreservation(kv.Key), "\\", 2)
            If parts(1).StartsWith("**del.") Then
                Target.DeleteValue(parts(0), Split(parts(1), ".", 2)(1))
            ElseIf parts(1).StartsWith("**delvals") Then
                Target.ClearKey(parts(0))
            ElseIf parts(1) = "**deletevalues" Then
                For Each entry In Split(kv.Value.AsString, ";")
                    Target.DeleteValue(parts(0), entry)
                Next
            ElseIf parts(1).StartsWith("**deletekeys") Then
                For Each entry In Split(kv.Value.AsString, ";")
                    Target.ClearKey(parts(0) & "\" & entry)
                Next
            ElseIf parts(1) <> "" And Not parts(1).StartsWith("**") Then
                Target.SetValue(casedParts(0), casedParts(1), kv.Value.AsArbitrary(), kv.Value.Kind)
                If oldEntries.Contains(kv.Key) Then oldEntries.Remove(kv.Key) ' It's not forgotten
            End If
        Next
        For Each e In oldEntries.Where(AddressOf RegistryPolicyProxy.IsPolicyKey) ' Remove the forgotten entries from the Registry
            Dim parts = Split(e, "\\", 2)
            Target.ForgetValue(parts(0), parts(1))
        Next
    End Sub
    Public Sub Apply(Target As IPolicySource)
        ' Apply all the values to the policy source
        ApplyDifference(Nothing, Target)
    End Sub
    Public Sub ClearKey(Key As String) Implements IPolicySource.ClearKey
        For Each value In GetValueNames(Key, False)
            ForgetValue(Key, value)
        Next
        Dim ped = PolEntryData.FromString(" ")
        Entries.Add(GetDictKey(Key, "**delvals."), ped)
    End Sub
    Public Sub ForgetKeyClearance(Key As String) Implements IPolicySource.ForgetKeyClearance
        Dim keyDeleter = GetDictKey(Key, "**delvals")
        For Each kv In Entries.Where(Function(e) e.Key.StartsWith(keyDeleter)).ToList ' "**delvals" and "**delvals." are both valid
            Entries.Remove(kv.Key)
        Next
    End Sub
    Public Function GetKeyNames(Key As String) As List(Of String)
        Dim subkeyNames As New List(Of String)
        Dim prefix = If(Key = "", "", Key & "\") ' Let an empty key name mean the root
        For Each entry In Entries.Keys.Where(Function(e) e.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            If entry.StartsWith(prefix & "\", StringComparison.InvariantCultureIgnoreCase) Then Continue For ' Values are delimited by an extra slash
            Dim properCased = Split(CasePreservation(entry), "\\", 2)(0)
            If prefix.Length >= properCased.Length Then Continue For ' Do not return the requested key itself
            Dim localKeyName = Split(properCased.Substring(prefix.Length), "\", 2)(0)
            If Not subkeyNames.Contains(localKeyName, StringComparer.InvariantCultureIgnoreCase) Then subkeyNames.Add(localKeyName)
        Next
        Return subkeyNames
    End Function
    Public Function GetValueKind(Key As String, Value As String) As RegistryValueKind
        Return Entries(GetDictKey(Key, Value)).Kind
    End Function
    Public Function Duplicate() As PolFile
        Using ms As New MemoryStream
            Using writer As New BinaryWriter(ms, Text.Encoding.Unicode, True)
                Save(writer)
            End Using
            ms.Position = 0
            Using reader As New BinaryReader(ms, Text.Encoding.Unicode)
                Return Load(reader)
            End Using
        End Using
    End Function
    Public Shared Function ObjectToBytes(Data As Object, Kind As RegistryValueKind) As Byte()
        Return PolEntryData.FromArbitrary(Data, Kind).Data
    End Function
    Public Shared Function BytesToObject(Data As Byte(), Kind As RegistryValueKind) As Object
        Return (New PolEntryData With {.Data = Data, .Kind = Kind}).AsArbitrary()
    End Function
    Private Class PolEntryData ' Represents one record in a POL file
        Public Kind As RegistryValueKind
        Public Data As Byte()
        Public Function AsString() As String ' Get a UTF-16LE string
            Dim sb As New Text.StringBuilder
            For x = 0 To (Data.Length \ 2) - 1
                Dim charCode = CInt(Data(x * 2)) + (Data((x * 2) + 1) << 8)
                If charCode = 0 Then Exit For
                sb.Append(ChrW(charCode))
            Next
            Return sb.ToString
        End Function
        Public Shared Function FromString(Text As String, Optional Expand As Boolean = False) As PolEntryData ' Save a UTF-16LE string
            Dim ped As New PolEntryData With {.Kind = RegistryValueKind.String}
            If Expand Then ped.Kind = RegistryValueKind.ExpandString
            Dim data((Text.Length * 2) + 1) As Byte
            For x = 0 To Text.Length - 1
                Dim charCode = AscW(Text(x))
                data(x * 2) = charCode And &HFF
                data((x * 2) + 1) = charCode >> 8
            Next
            ped.Data = data
            Return ped
        End Function
        Public Function AsDword() As UInteger
            Return Data(0) + (CUInt(Data(1)) << 8) + (CUInt(Data(2)) << 16) + (CUInt(Data(3)) << 24)
        End Function
        Public Shared Function FromDword(Dword As UInteger) As PolEntryData
            Dim ped As New PolEntryData With {.Kind = RegistryValueKind.DWord}
            Dim data(3) As Byte
            data(0) = Dword And &HFFUI
            data(1) = (Dword >> 8) And &HFFUI
            data(2) = (Dword >> 16) And &HFFUI
            data(3) = Dword >> 24
            ped.Data = data
            Return ped
        End Function
        Public Function AsQword() As ULong
            Dim value As ULong = 0
            For n = 0 To 7
                value += (CULng(Data(n)) << (n * 8))
            Next
            Return value
        End Function
        Public Shared Function FromQword(Qword As ULong) As PolEntryData
            Dim ped As New PolEntryData With {.Kind = RegistryValueKind.QWord}
            Dim data(7) As Byte
            For n = 0 To 7
                data(n) = (Qword >> (n * 8)) And &HFFUL
            Next
            ped.Data = data
            Return ped
        End Function
        Public Function AsMultiString() As String()
            Dim strings As New List(Of String)
            Dim sb As New Text.StringBuilder
            For n = 0 To (Data.Length / 2) - 1
                Dim charCode = Data(n * 2) + (Data((n * 2) + 1) << 8)
                If charCode = 0 Then
                    If sb.Length = 0 Then Exit For
                    strings.Add(sb.ToString)
                    sb.Clear()
                Else
                    sb.Append(ChrW(charCode))
                End If
            Next
            Return strings.ToArray
        End Function
        Public Shared Function FromMultiString(Strings As String()) As PolEntryData
            Dim ped As New PolEntryData With {.Kind = RegistryValueKind.MultiString}
            Dim data((Strings.Sum(Function(s) s.Length + 1) + 1) * 2 - 1) As Byte
            Dim n As Integer = 0
            For Each s In Strings
                For Each c In s
                    Dim charCode = AscW(c)
                    data(n) = charCode And &HFF
                    data(n + 1) = charCode >> 8
                    n += 2
                Next
                n += 2 ' Leave two null bytes after each string
            Next
            ped.Data = data
            Return ped
        End Function
        Public Function AsBinary() As Byte()
            Return Data.Clone
        End Function
        Public Shared Function FromBinary(Binary As Byte(), Optional Kind As RegistryValueKind = RegistryValueKind.Binary) As PolEntryData
            Dim ped As New PolEntryData With {.Kind = Kind}
            ped.Data = Binary.Clone
            Return ped
        End Function
        Public Function AsArbitrary() As Object
            ' Get the data in the best .NET type for it
            Select Case Kind
                Case RegistryValueKind.String
                    Return AsString()
                Case RegistryValueKind.DWord
                    Return AsDword()
                Case RegistryValueKind.ExpandString
                    Return AsString()
                Case RegistryValueKind.QWord
                    Return AsQword()
                Case RegistryValueKind.MultiString
                    Return AsMultiString()
                Case Else
                    Return AsBinary()
            End Select
        End Function
        Public Shared Function FromArbitrary(Data As Object, Kind As RegistryValueKind) As PolEntryData
            ' Take an arbitrary .NET object and turn it into bytes
            Select Case Kind
                Case RegistryValueKind.String
                    Return FromString(Data)
                Case RegistryValueKind.DWord
                    Return FromDword(Data)
                Case RegistryValueKind.ExpandString
                    Return FromString(Data, True)
                Case RegistryValueKind.QWord
                    Return FromQword(Data)
                Case RegistryValueKind.MultiString
                    Return FromMultiString(Data)
                Case Else
                    Return FromBinary(Data, Kind)
            End Select
        End Function
    End Class
End Class
Public Class RegistryPolicyProxy ' Pass operations through to the real Registry
    Implements IPolicySource
    Private RootKey As RegistryKey
    Public Shared Function EncapsulateKey(Key As RegistryKey) As RegistryPolicyProxy
        Return New RegistryPolicyProxy With {.RootKey = Key}
    End Function
    Public Shared Function EncapsulateKey(Key As RegistryHive) As RegistryPolicyProxy
        Return EncapsulateKey(RegistryKey.OpenBaseKey(Key, RegistryView.Default))
    End Function
    Public Sub DeleteValue(Key As String, Value As String) Implements IPolicySource.DeleteValue
        Using regKey = RootKey.OpenSubKey(Key, True)
            If regKey Is Nothing Then Exit Sub
            regKey.DeleteValue(Value, False)
        End Using
    End Sub
    Public Sub ForgetValue(Key As String, Value As String) Implements IPolicySource.ForgetValue
        DeleteValue(Key, Value) ' The Registry has no concept of "will delete this when I see it"
    End Sub
    Public Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind) Implements IPolicySource.SetValue
        If TypeOf Data Is UInteger Then
            Data = (New ReinterpretableDword With {.Unsigned = Data}).Signed
        ElseIf TypeOf Data Is ULong Then
            Data = (New ReinterpretableQword With {.Unsigned = Data}).Signed
        End If
        Using regKey = RootKey.CreateSubKey(Key)
            regKey.SetValue(Value, Data, DataType)
        End Using
    End Sub
    Public Function ContainsValue(Key As String, Value As String) As Boolean Implements IPolicySource.ContainsValue
        Using regKey = RootKey.OpenSubKey(Key)
            If regKey Is Nothing Then Return False
            If Value = "" Then Return True
            Return regKey.GetValueNames().Any(Function(s) s.Equals(Value, StringComparison.InvariantCultureIgnoreCase))
        End Using
    End Function
    Public Function GetValue(Key As String, Value As String) As Object Implements IPolicySource.GetValue
        Using regKey = RootKey.OpenSubKey(Key, False)
            If regKey Is Nothing Then Return Nothing
            Dim data = regKey.GetValue(Value, Nothing, RegistryValueOptions.DoNotExpandEnvironmentNames)
            If TypeOf data Is Integer Then
                Return (New ReinterpretableDword With {.Signed = data}).Unsigned
            ElseIf TypeOf data Is Long Then
                Return (New ReinterpretableQword With {.Signed = data}).Unsigned
            Else
                Return data
            End If
        End Using
    End Function
    Public Function GetValueNames(Key As String) As List(Of String) Implements IPolicySource.GetValueNames
        Using regKey = RootKey.OpenSubKey(Key)
            If regKey Is Nothing Then Return New List(Of String) Else Return regKey.GetValueNames.ToList
        End Using
    End Function
    Public Function WillDeleteValue(Key As String, Value As String) As Boolean Implements IPolicySource.WillDeleteValue
        Return False
    End Function
    Public Shared Function IsPolicyKey(KeyPath As String) As Boolean
        Return PolicyKeys.Any(Function(pk) KeyPath.StartsWith(pk & "\", StringComparison.InvariantCultureIgnoreCase) Or
                                  KeyPath.Equals(pk, StringComparison.InvariantCultureIgnoreCase))
    End Function
    Public Sub ClearKey(Key As String) Implements IPolicySource.ClearKey
        For Each value In GetValueNames(Key)
            ForgetValue(Key, value)
        Next
    End Sub
    Public Sub ForgetKeyClearance(Key As String) Implements IPolicySource.ForgetKeyClearance
        ' Does nothing
    End Sub
    Public ReadOnly Property EncapsulatedRegistry As RegistryKey
        Get
            Return RootKey
        End Get
    End Property
    Public Shared ReadOnly Property PolicyKeys As IEnumerable(Of String)
        Get
            ' Values outside these branches are not tracked by PolFile.ApplyDifference
            Return {"software\policies", "software\microsoft\windows\currentversion\policies", "system\currentcontrolset\policies"}
        End Get
    End Property
End Class