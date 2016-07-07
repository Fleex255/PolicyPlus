Imports Microsoft.Win32
Imports System.IO
Public Interface IPolicySource
    Function ContainsValue(Key As String, Value As String) As Boolean
    Function GetValue(Key As String, Value As String) As Object
    Function WillDeleteValue(Key As String, Value As String) As Boolean
    Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind)
    Sub ForgetValue(Key As String, Value As String) ' Stop keeping track of a value
    Sub DeleteValue(Key As String, Value As String) ' Mark a value as queued for deletion
End Interface
Public Class PolFile
    Implements IPolicySource
    Private Entries As New Dictionary(Of String, PolEntryData) ' Keys are lowercase Registry keys and values, separated by "\\"
    Private CasePreservation As New Dictionary(Of String, String) ' Keep track of the original cases
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
    Public Sub DeleteValue(Key As String, Value As String) Implements IPolicySource.DeleteValue
        ForgetValue(Key, Value)
        If Not WillDeleteValue(Key, Value) Then
            Dim ped = PolEntryData.FromDword(32) ' It's what Microsoft does
            Entries.Add(GetDictKey(Key, "**del." & Value), Nothing)
        End If
    End Sub
    Public Sub ForgetValue(Key As String, Value As String) Implements IPolicySource.ForgetValue
        Dim dictKey = GetDictKey(Key, Value)
        If Entries.ContainsKey(dictKey) Then Entries.Remove(dictKey)
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
        If Entries.ContainsKey(GetDictKey(Key, "**del." & Value)) Then Return True
        If Entries.ContainsKey(GetDictKey(Key, "**delvals")) Then Return True
        Dim delValuesDict = GetDictKey(Key, "**delvalues")
        If Entries.ContainsKey(delValuesDict) Then
            Dim delEntry = Entries(delValuesDict)
            Dim lowerVal = Value.ToLowerInvariant
            Dim deletedValues = Split(delEntry.AsString, ";")
            If deletedValues.Any(Function(s) s.ToLowerInvariant = lowerVal) Then Return True
        End If
        Return False
    End Function
    Private Class PolEntryData
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
        Public Shared Function FromString(Text As String) As PolEntryData
            Dim ped As New PolEntryData With {.Kind = RegistryValueKind.String}
            Dim data((Text.Length * 2) + 1) As Byte
            For x = 0 To Text.Length - 1
                Dim charCode = AscW(Text(x))
                data(x) = charCode And &HFF
                data(x + 1) = charCode >> 8
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
            data(0) = Dword And &HFF
            data(1) = Dword And &HFF00
            data(2) = Dword And &HFF0000
            data(3) = Dword >> 24
            ped.Data = data
            Return ped
        End Function
        Public Function AsArbitrary() As Object
            Select Case Kind
                Case RegistryValueKind.String
                    Return AsString()
                Case RegistryValueKind.DWord
                    Return AsDword()
                Case Else
                    ' TODO: Other types of entries
            End Select
        End Function
        Public Shared Function FromArbitrary(Data As Object, Kind As RegistryValueKind) As PolEntryData
            Select Case Kind
                Case RegistryValueKind.String
                    Return FromString(Data)
                Case RegistryValueKind.DWord
                    Return FromDword(Data)
                Case Else
                    ' TODO: Other types of entries
            End Select
        End Function
    End Class
End Class
Public Class RegistryPolicyProxy
    Implements IPolicySource
    Public Sub DeleteValue(Key As String, Value As String) Implements IPolicySource.DeleteValue
        Throw New NotImplementedException()
    End Sub
    Public Sub ForgetValue(Key As String, Value As String) Implements IPolicySource.ForgetValue
        Throw New NotImplementedException()
    End Sub
    Public Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind) Implements IPolicySource.SetValue
        Throw New NotImplementedException()
    End Sub
    Public Function ContainsValue(Key As String, Value As String) As Boolean Implements IPolicySource.ContainsValue
        Throw New NotImplementedException()
    End Function
    Public Function GetValue(Key As String, Value As String) As Object Implements IPolicySource.GetValue
        Throw New NotImplementedException()
    End Function
    Public Function WillDeleteValue(Key As String, Value As String) As Boolean Implements IPolicySource.WillDeleteValue
        Return False
    End Function
End Class