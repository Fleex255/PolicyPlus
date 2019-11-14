Public Class SpolFile
    Public Policies As New List(Of SpolPolicyState)
    Dim ParserLine As Integer = 0
    Public Shared Function FromText(Text As String) As SpolFile
        Dim spol As New SpolFile
        Try
            spol.LoadFromText(Text)
            Return spol
        Catch ex As Exception
            Throw New Exception(ex.Message & " (Error found on line " & spol.ParserLine & ".)")
        End Try
    End Function
    Private Sub LoadFromText(Text As String)
        ' Load a SPOL script into policy states
        Dim allLines = Split(Text, vbCrLf)
        Dim line As String = ""
        Dim nextLine = Function() As String
                           ParserLine += 1
                           line = allLines(ParserLine - 1) ' For human-readability in errors
                           Return line
                       End Function
        Dim atEnd = Function() As Boolean
                        Return ParserLine >= allLines.Length
                    End Function
        Dim peekLine = Function() As String
                           Return allLines(ParserLine) ' +1 for next, -1 for array
                       End Function
        Dim getAllStrings = Function(Splittable As String, Delimiter As Char) As List(Of String)
                                Dim list As New List(Of String)
                                Dim sb As Text.StringBuilder = Nothing
                                For n = 0 To Splittable.Length - 1
                                    If Splittable(n) = Delimiter Then
                                        If sb Is Nothing Then
                                            sb = New Text.StringBuilder
                                        Else
                                            If n + 1 < Splittable.Length - 1 AndAlso Splittable(n + 1) = Delimiter Then
                                                sb.Append(Delimiter)
                                                n += 1
                                            Else
                                                list.Add(sb.ToString)
                                                sb = Nothing
                                            End If
                                        End If
                                    ElseIf sb IsNot Nothing Then
                                        sb.Append(Splittable(n))
                                    End If
                                Next
                                Return list
                            End Function
        If nextLine() <> "Policy Plus Semantic Policy" Then Throw New Exception("Incorrect signature.")
        Do Until atEnd()
            If Trim(nextLine()) = "" Then Continue Do
            Dim policyHeaderParts = Split(line, " ", 2) ' Section and policy ID
            Dim singlePolicy As New SpolPolicyState With {.UniqueID = policyHeaderParts(1)}
            singlePolicy.Section = If(policyHeaderParts(0) = "U", AdmxPolicySection.User, AdmxPolicySection.Machine)
            Const commentPrefix As String = "comment: "
            If LTrim(peekLine()).ToLowerInvariant().StartsWith(commentPrefix) Then
                Dim escapedCommentText = LTrim(nextLine()).Substring(commentPrefix.Length)
                Dim commentText As New Text.StringBuilder
                For n = 0 To escapedCommentText.Length - 1
                    If escapedCommentText(n) = "\"c Then
                        If n = escapedCommentText.Length - 1 Then Throw New Exception("Escape sequence started at end of comment.")
                        Select Case escapedCommentText(n + 1)
                            Case "\"c
                                commentText.Append("\"c)
                            Case "n"
                                commentText.Append(vbCrLf)
                            Case Else
                                Throw New Exception("Unknown comment escape sequence \" & escapedCommentText(n + 1) & ".")
                        End Select
                        n += 1
                    Else
                        commentText.Append(escapedCommentText(n))
                    End If
                Next
                singlePolicy.Comment = commentText.ToString
            End If
            Select Case Trim(nextLine()).ToLowerInvariant
                Case "not configured"
                    singlePolicy.BasicState = PolicyState.NotConfigured
                Case "enabled"
                    singlePolicy.BasicState = PolicyState.Enabled
                Case "disabled"
                    singlePolicy.BasicState = PolicyState.Disabled
                Case Else
                    Throw New Exception("Unknown policy state.")
            End Select
            If singlePolicy.BasicState = PolicyState.Enabled Then
                Do Until atEnd() OrElse Trim(peekLine()) = ""
                    Dim optionParts = Split(Trim(nextLine()), ": ", 2) ' Name and value
                    Dim valueText = optionParts(1)
                    Dim newObj As Object
                    If valueText.StartsWith("#") Then
                        newObj = CInt(valueText.Substring(1))
                    ElseIf UInteger.TryParse(valueText, 0) Then
                        newObj = CUInt(valueText)
                    ElseIf Boolean.TryParse(valueText, False) Then
                        newObj = CBool(valueText)
                    ElseIf valueText.StartsWith("'") And valueText.EndsWith("'") Then
                        newObj = valueText.Substring(1, valueText.Length - 2)
                    ElseIf valueText.StartsWith("""") And valueText.EndsWith("""") Then
                        newObj = getAllStrings(valueText, """").ToArray
                    ElseIf valueText = "None" Then
                        newObj = Array.CreateInstance(GetType(String), 0)
                    ElseIf valueText = "[" Then
                        Dim entries As New List(Of List(Of String))
                        Do Until Trim(peekLine()) = "]"
                            entries.Add(getAllStrings(nextLine(), """"))
                        Loop
                        nextLine() ' Skip the closing bracket
                        If entries.Count = 0 Then
                            newObj = Nothing ' PolicyProcessing will ignore an empty list element
                        ElseIf entries(0).Count = 1 Then
                            newObj = entries.Select(Function(l) l(0)).ToList
                        Else
                            newObj = entries.ToDictionary(Function(l) l(0), Function(l) l(1))
                        End If
                    Else
                        Throw New Exception("Unknown option data format.")
                    End If
                    singlePolicy.ExtraOptions.Add(optionParts(0), newObj)
                Loop
            End If
            Policies.Add(singlePolicy)
        Loop
    End Sub
    Public Shared Function GetFragment(State As SpolPolicyState) As String
        ' Create a SPOL text fragment from the given policy state
        Dim sb As New Text.StringBuilder
        sb.Append(If(State.Section = AdmxPolicySection.Machine, "C ", "U "))
        sb.AppendLine(State.UniqueID)
        If State.Comment <> "" Then
            ' Escape newlines and backslashes in the comment so it can fit on one SPOL line
            sb.AppendLine(" Comment: " & State.Comment.Replace("\", "\\").Replace(vbCrLf, "\n"))
        End If
        Select Case State.BasicState
            Case PolicyState.NotConfigured
                sb.AppendLine(" Not Configured")
            Case PolicyState.Enabled
                sb.AppendLine(" Enabled")
            Case PolicyState.Disabled
                sb.AppendLine(" Disabled")
        End Select
        Dim doubleQuoteString = Function(Text As String) """" & Text.Replace("""", """""") & """"
        If State.BasicState = PolicyState.Enabled And State.ExtraOptions IsNot Nothing Then
            For Each kv In State.ExtraOptions
                sb.Append("  ")
                sb.Append(kv.Key)
                sb.Append(": ")
                Select Case kv.Value.GetType
                    Case GetType(Integer)
                        sb.Append("#")
                        sb.AppendLine(CInt(kv.Value))
                    Case GetType(UInteger)
                        sb.AppendLine(CUInt(kv.Value))
                    Case GetType(Boolean)
                        sb.AppendLine(CBool(kv.Value))
                    Case GetType(String)
                        sb.Append("'")
                        sb.Append(CStr(kv.Value))
                        sb.AppendLine("'")
                    Case GetType(String())
                        Dim stringArray = CType(kv.Value, String())
                        If stringArray.Length = 0 Then sb.AppendLine("None") Else sb.AppendLine(String.Join(", ", stringArray.Select(doubleQuoteString)))
                    Case Else ' List(Of String) or Dictionary(Of String, String)
                        sb.AppendLine("[")
                        If TypeOf kv.Value Is List(Of String) Then
                            For Each listEntry In CType(kv.Value, List(Of String))
                                sb.Append("   ")
                                sb.AppendLine(doubleQuoteString(listEntry))
                            Next
                        Else
                            For Each listKv In CType(kv.Value, Dictionary(Of String, String))
                                sb.Append("   ")
                                sb.Append(doubleQuoteString(listKv.Key))
                                sb.Append(": ")
                                sb.AppendLine(doubleQuoteString(listKv.Value))
                            Next
                        End If
                        sb.AppendLine("  ]")
                End Select
            Next
        End If
        Return sb.ToString
    End Function
    Public Function ApplyAll(AdmxWorkspace As AdmxBundle, UserSource As IPolicySource, CompSource As IPolicySource, UserComments As Dictionary(Of String, String), CompComments As Dictionary(Of String, String)) As Integer
        ' Write the policy states to the policy sources
        Dim failures As Integer = 0
        For Each policy In Policies
            Try
                If policy.Section = AdmxPolicySection.Machine Then
                    policy.Apply(CompSource, AdmxWorkspace, CompComments)
                Else
                    policy.Apply(UserSource, AdmxWorkspace, UserComments)
                End If
            Catch ex As Exception
                failures += 1
            End Try
        Next
        Return failures
    End Function
End Class
Public Class SpolPolicyState
    Public UniqueID As String
    Public Section As AdmxPolicySection
    Public BasicState As PolicyState
    Public Comment As String
    Public ExtraOptions As New Dictionary(Of String, Object)
    Public Sub Apply(PolicySource As IPolicySource, AdmxWorkspace As AdmxBundle, CommentsMap As Dictionary(Of String, String))
        Dim pol = AdmxWorkspace.Policies(UniqueID)
        If CommentsMap IsNot Nothing And Comment <> "" Then CommentsMap(UniqueID) = Comment
        PolicyProcessing.ForgetPolicy(PolicySource, pol)
        PolicyProcessing.SetPolicyState(PolicySource, pol, BasicState, ExtraOptions)
    End Sub
End Class
