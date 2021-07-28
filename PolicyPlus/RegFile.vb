Imports System.IO
Imports Microsoft.Win32
Public Class RegFile
    ' This class implements just enough of IPolicySource to work with PolFile.ApplyDifference
    ' It is not a valid policy source for any policy loader
    Implements IPolicySource
    Const RegSignature = "Windows Registry Editor Version 5.00"
    Dim Prefix As String ' REG files require fully-rooted key paths, while other policy sources disallow them
    Dim SourceSubtree As String ' Accept only writes under this policy path (not needed if only going to use Apply)
    Dim Keys As New List(Of RegFileKey)
    Private Shared Function EscapeValue(Text As String) As String
        ' Escape quotes and slashes for a value name or string data
        Dim sb As New Text.StringBuilder
        For n = 0 To Text.Length - 1
            Dim character = Text(n)
            If character = """"c Or character = "\"c Then sb.Append("\"c)
            sb.Append(character)
        Next
        Return sb.ToString
    End Function
    Private Shared Function UnescapeValue(Text As String) As String
        ' The reverse of EscapeValue
        Dim sb As New Text.StringBuilder
        Dim escaping As Boolean = False
        For n = 0 To Text.Length - 1
            If escaping Then
                sb.Append(Text(n))
                escaping = False
            ElseIf Text(n) = "\"c Then
                escaping = True
            Else
                sb.Append(Text(n))
            End If
        Next
        Return sb.ToString
    End Function
    Private Shared Function ReadNonCommentingLine(Reader As StreamReader, Optional StopAt As Char? = Nothing) As String
        Do
            If Reader.EndOfStream Then Return Nothing
            If StopAt.HasValue AndAlso Reader.Peek = Asc(StopAt.Value) Then Return Nothing
            Dim line = Reader.ReadLine
            If String.IsNullOrWhiteSpace(line) OrElse Trim(line).StartsWith(";") Then Continue Do
            Return line
        Loop
    End Function
    Public Shared Function Load(Reader As StreamReader, Prefix As String) As RegFile
        If Reader.ReadLine() <> RegSignature Then Throw New InvalidDataException("Incorrect REG signature")
        Dim reg As New RegFile
        reg.SetPrefix(Prefix)
        Do ' Read all the keys
            Dim keyHeader = ReadNonCommentingLine(Reader)
            If keyHeader Is Nothing Then Exit Do
            Dim keyName = keyHeader.Substring(1, keyHeader.Length - 2) ' Remove the brackets
            If keyName.StartsWith("-") Then
                ' It's a deleter
                Dim deleterKey As New RegFileKey With {.Name = keyName.Substring(1), .IsDeleter = True}
                reg.Keys.Add(deleterKey)
            Else
                Dim key As New RegFileKey With {.Name = keyName}
                Do ' Read all the values
                    Dim valueLine = ReadNonCommentingLine(Reader, "["c)
                    If valueLine Is Nothing Then Exit Do
                    Dim valueName As String = ""
                    Dim data As String
                    If valueLine.StartsWith("@") Then
                        data = valueLine.Substring(2)
                    Else
                        Dim parts = Split(valueLine, """=", 2)
                        valueName = UnescapeValue(parts(0).Substring(1))
                        data = parts(1)
                    End If
                    Dim value As New RegFileValue With {.Name = valueName}
                    If data = "-" Then
                        value.IsDeleter = True
                    ElseIf data.StartsWith("""") Then
                        value.Kind = RegistryValueKind.String
                        value.Data = UnescapeValue(data.Substring(1, data.Length - 2))
                    ElseIf data.StartsWith("dword:") Then
                        value.Kind = RegistryValueKind.DWord
                        value.Data = UInteger.Parse(data.Substring(6), Globalization.NumberStyles.HexNumber)
                    ElseIf data.StartsWith("hex") Then
                        Dim indexOfClosingParen = data.IndexOf(")"c)
                        Dim curHexLine As String
                        If indexOfClosingParen <> -1 Then
                            value.Kind = Integer.Parse(data.Substring(4, indexOfClosingParen - 4), Globalization.NumberStyles.HexNumber)
                            curHexLine = data.Substring(indexOfClosingParen + 2)
                        Else
                            value.Kind = RegistryValueKind.Binary
                            curHexLine = data.Substring(4)
                        End If
                        Dim allDehexedBytes As New List(Of Byte)
                        Do ' Read all the hex lines
                            Dim hexBytes = Split(Trim(curHexLine).TrimEnd("\"c, ","c), ",").Where(Function(s) s <> "")
                            For Each b In hexBytes
                                allDehexedBytes.Add(Byte.Parse(b, Globalization.NumberStyles.HexNumber))
                            Next
                            If curHexLine.EndsWith("\") Then
                                curHexLine = Reader.ReadLine
                            Else
                                Exit Do
                            End If
                        Loop
                        value.Data = PolFile.BytesToObject(allDehexedBytes.ToArray, value.Kind)
                    End If
                    key.Values.Add(value)
                Loop
                reg.Keys.Add(key)
            End If
        Loop
        Return reg
    End Function
    Public Shared Function Load(File As String, Prefix As String) As RegFile
        Using reader As New StreamReader(File)
            Return Load(reader, Prefix)
        End Using
    End Function
    Public Sub Save(Writer As StreamWriter)
        Writer.WriteLine(RegSignature)
        Writer.WriteLine()
        For Each key In Keys
            If key.IsDeleter Then
                Writer.WriteLine("[-" & key.Name & "]")
            Else
                Writer.WriteLine("[" & key.Name & "]")
                For Each value In key.Values
                    Dim posInRow = 0 ' To split hex across lines
                    If value.Name = "" Then
                        Writer.Write("@")
                        posInRow = 1
                    Else
                        Dim quotedName = """" & EscapeValue(value.Name) & """"
                        Writer.Write(quotedName)
                        posInRow = quotedName.Length
                    End If
                    Writer.Write("=")
                    posInRow += 1
                    If value.IsDeleter Then
                        Writer.WriteLine("-")
                    Else
                        Select Case value.Kind
                            Case RegistryValueKind.String
                                Writer.Write("""")
                                Writer.Write(EscapeValue(value.Data))
                                Writer.WriteLine("""")
                            Case RegistryValueKind.DWord
                                Writer.Write("dword:")
                                Writer.WriteLine(Convert.ToString(CUInt(value.Data), 16).PadLeft(8, "0"c))
                            Case Else
                                Writer.Write("hex")
                                posInRow += 3
                                If value.Kind <> RegistryValueKind.Binary Then
                                    Writer.Write("(")
                                    Writer.Write(Convert.ToString(value.Kind, 16))
                                    Writer.Write(")")
                                    posInRow += 3
                                End If
                                Writer.Write(":")
                                posInRow += 1
                                Dim bytes = PolFile.ObjectToBytes(value.Data, value.Kind)
                                For n = 0 To bytes.Length - 2
                                    Writer.Write(Convert.ToString(bytes(n), 16).PadLeft(2, "0"c))
                                    Writer.Write(",")
                                    posInRow += 3
                                    If posInRow >= 78 Then
                                        Writer.WriteLine("\")
                                        Writer.Write("  ")
                                        posInRow = 2
                                    End If
                                Next
                                If bytes.Length > 0 Then
                                    Writer.WriteLine(Convert.ToString(bytes(bytes.Length - 1), 16).PadLeft(2, "0"c))
                                Else
                                    Writer.WriteLine()
                                End If
                        End Select
                    End If
                Next
            End If
            Writer.WriteLine()
        Next
    End Sub
    Public Sub Save(File As String)
        Using writer As New StreamWriter(File, False)
            Save(writer)
        End Using
    End Sub
    Private Function UnprefixKeyName(Name As String) As String
        If Name.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase) Then Return Name.Substring(Prefix.Length) Else Return Name
    End Function
    Private Function PrefixKeyName(Name As String) As String
        Return Prefix & Name
    End Function
    Private Function GetKey(Name As String) As RegFileKey
        Return Keys.FirstOrDefault(Function(k) k.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
    End Function
    Private Function GetKeyByUnprefixedName(Name As String) As RegFileKey
        Return GetKey(PrefixKeyName(Name))
    End Function
    Private Function GetOrCreateKey(Name As String) As RegFileKey
        Dim key = GetKey(Name)
        If key Is Nothing Then
            key = New RegFileKey With {.Name = Name}
            Keys.Add(key)
        End If
        Return key
    End Function
    Private Function GetNonDeleterKey(Name As String) As RegFileKey
        Return Keys.FirstOrDefault(Function(k) (Not k.IsDeleter) AndAlso k.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
    End Function
    Private Function IsSourceKeyAcceptable(Key As String) As Boolean
        Return (SourceSubtree = "") OrElse Key.Equals(SourceSubtree, StringComparison.InvariantCultureIgnoreCase) OrElse
            Key.StartsWith(SourceSubtree & "\", StringComparison.InvariantCultureIgnoreCase)
    End Function
    Public Function ContainsValue(Key As String, Value As String) As Boolean Implements IPolicySource.ContainsValue
        Throw New NotImplementedException()
    End Function
    Public Function GetValue(Key As String, Value As String) As Object Implements IPolicySource.GetValue
        Throw New NotImplementedException()
    End Function
    Public Function WillDeleteValue(Key As String, Value As String) As Boolean Implements IPolicySource.WillDeleteValue
        Throw New NotImplementedException()
    End Function
    Public Function GetValueNames(Key As String) As List(Of String) Implements IPolicySource.GetValueNames
        Throw New NotImplementedException()
    End Function
    Public Sub SetValue(Key As String, Value As String, Data As Object, DataType As RegistryValueKind) Implements IPolicySource.SetValue
        If Not IsSourceKeyAcceptable(Key) Then Exit Sub
        Dim fullKeyName = PrefixKeyName(Key)
        Dim keyRecord = GetNonDeleterKey(fullKeyName)
        If keyRecord Is Nothing Then
            keyRecord = New RegFileKey With {.Name = fullKeyName}
            Keys.Add(keyRecord)
        End If
        keyRecord.Values.Remove(keyRecord.GetValue(Value))
        keyRecord.Values.Add(New RegFileValue With {.Name = Value, .Kind = DataType, .Data = Data})
    End Sub
    Public Sub ForgetValue(Key As String, Value As String) Implements IPolicySource.ForgetValue
        Throw New NotImplementedException()
    End Sub
    Public Sub DeleteValue(Key As String, Value As String) Implements IPolicySource.DeleteValue
        If Not IsSourceKeyAcceptable(Key) Then Exit Sub
        Dim fullKeyName = PrefixKeyName(Key)
        Dim keyRecord = GetOrCreateKey(fullKeyName)
        If keyRecord.IsDeleter Then Exit Sub
        keyRecord.Values.Remove(keyRecord.GetValue(Value))
        keyRecord.Values.Add(New RegFileValue With {.Name = Value, .IsDeleter = True})
    End Sub
    Public Sub ClearKey(Key As String) Implements IPolicySource.ClearKey
        If Not IsSourceKeyAcceptable(Key) Then Exit Sub
        Dim fullName = PrefixKeyName(Key)
        Keys.Remove(GetKey(fullName))
        Keys.Add(New RegFileKey With {.Name = fullName, .IsDeleter = True})
    End Sub
    Public Sub ForgetKeyClearance(Key As String) Implements IPolicySource.ForgetKeyClearance
        If Not IsSourceKeyAcceptable(Key) Then Exit Sub
        Dim keyRecord = GetKeyByUnprefixedName(Key)
        If keyRecord Is Nothing Then Exit Sub
        If keyRecord.IsDeleter Then Keys.Remove(keyRecord)
    End Sub
    Public Sub Apply(Target As IPolicySource)
        For Each key In Keys
            Dim unprefixedKeyName = UnprefixKeyName(key.Name)
            If key.IsDeleter Then
                Target.ClearKey(unprefixedKeyName)
            Else
                For Each value In key.Values
                    If value.IsDeleter Then
                        Target.DeleteValue(unprefixedKeyName, value.Name)
                    Else
                        Target.SetValue(unprefixedKeyName, value.Name, value.Data, value.Kind)
                    End If
                Next
            End If
        Next
    End Sub
    Public Sub SetPrefix(Prefix As String)
        If Not Prefix.EndsWith("\") Then Prefix &= "\"
        Me.Prefix = Prefix
    End Sub
    Public Sub SetSourceBranch(Branch As String)
        If Branch.EndsWith("\") Then Branch = Branch.TrimEnd("\"c)
        SourceSubtree = Branch
    End Sub
    Public Function GuessPrefix() As String
        ' Try to determine a reasonable prefix from the data present
        If Keys.Count = 0 Then Return "HKEY_LOCAL_MACHINE\" ' Can't do much without any data
        Dim firstKeyName = Keys(0).Name
        If firstKeyName.StartsWith("HKEY_USERS\") Then
            ' The user SID should be part of the prefix
            Dim secondSlashPos = firstKeyName.IndexOf("\", 11)
            Return firstKeyName.Substring(0, secondSlashPos + 1)
        Else
            ' Other hives should be just fine
            Dim firstSlashPos = firstKeyName.IndexOf("\")
            Return firstKeyName.Substring(0, firstSlashPos + 1)
        End If
    End Function
    Public Function HasDefaultValues() As Boolean
        Return Keys.Any(Function(k) k.Values.Any(Function(v) v.Name = ""))
    End Function
    Private Class RegFileKey
        Public Name As String
        Public IsDeleter As Boolean
        Public Values As New List(Of RegFileValue)
        Public Function GetValue(Value As String) As RegFileValue
            Return Values.FirstOrDefault(Function(v) v.Name.Equals(Value, StringComparison.InvariantCultureIgnoreCase))
        End Function
    End Class
    Private Class RegFileValue
        Public Name As String
        Public Data As Object
        Public Kind As RegistryValueKind
        Public IsDeleter As Boolean
    End Class
End Class