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
        Dim allLines = Split(Text, vbCrLf)
        Dim line As String = ""
        Dim nextLine = Function() As String
                           ParserLine += 1
                           line = allLines(ParserLine - 1) ' For human-readability
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
            singlePolicy.Section = If(policyHeaderParts(1) = "0", AdmxPolicySection.User, AdmxPolicySection.Machine)
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
                        newObj = getAllStrings(valueText, """")
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
    Public Function ApplyAll(AdmxWorkspace As AdmxBundle, UserSource As IPolicySource, CompSource As IPolicySource) As Integer
        Dim failures As Integer = 0
        For Each policy In Policies
            Try
                policy.Apply(If(policy.Section = AdmxPolicySection.Machine, CompSource, UserSource), AdmxWorkspace)
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
    Public ExtraOptions As New Dictionary(Of String, Object)
    Public Sub Apply(PolicySource As IPolicySource, AdmxWorkspace As AdmxBundle)
        Dim pol = AdmxWorkspace.Policies(UniqueID)
        PolicyProcessing.ForgetPolicy(PolicySource, pol)
        PolicyProcessing.SetPolicyState(PolicySource, pol, BasicState, ExtraOptions)
    End Sub
End Class
