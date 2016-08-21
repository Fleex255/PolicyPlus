Public Class FindByText
    Dim CommentSources() As Dictionary(Of String, String)
    Public Searcher As Func(Of PolicyPlusPolicy, Boolean)
    Public Function PresentDialog(ParamArray CommentDicts() As Dictionary(Of String, String)) As DialogResult
        CommentSources = CommentDicts.Where(Function(d) d IsNot Nothing).ToArray
        Return ShowDialog()
    End Function
    Private Sub FindByText_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim text = StringTextbox.Text
        If text = "" Then
            MsgBox("Please enter search terms.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim checkTitle = TitleCheckbox.Checked
        Dim checkDesc = DescriptionCheckbox.Checked
        Dim checkComment = CommentCheckbox.Checked
        If Not (checkTitle Or checkDesc Or checkComment) Then
            MsgBox("At least one attribute must be searched. Check one of the boxes and try again.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Searcher = Function(Policy As PolicyPlusPolicy) As Boolean
                       Dim cleanupStr = Function(RawText As String) As String
                                            Return New String(Trim(RawText.ToLowerInvariant).Where(Function(c) Not ".,'"";/!(){}[]".Contains(c)).ToArray)
                                        End Function
                       ' Parse the query string for wildcards or quoted strings
                       Dim rawSplitted = Split(text)
                       Dim simpleWords As New List(Of String)
                       Dim wildcards As New List(Of String)
                       Dim quotedStrings As New List(Of String)
                       Dim partialQuotedString As String = ""
                       For n = 0 To rawSplitted.Length - 1
                           Dim curString = rawSplitted(n)
                           If partialQuotedString <> "" Then
                               partialQuotedString &= curString & " "
                               If curString.EndsWith("""") Then
                                   quotedStrings.Add(cleanupStr(partialQuotedString))
                                   partialQuotedString = ""
                               End If
                           ElseIf curString.StartsWith("""") Then
                               partialQuotedString = curString & " "
                           ElseIf curString.Contains("*") Or curString.Contains("?") Then
                               wildcards.Add(cleanupStr(curString))
                           Else
                               simpleWords.Add(cleanupStr(curString))
                           End If
                       Next
                       ' Do the searching
                       Dim isStringAHit = Function(SearchedText As String) As Boolean
                                              Dim cleanText = cleanupStr(SearchedText)
                                              Dim wordsInText = Split(cleanText)
                                              Return simpleWords.All(Function(w) wordsInText.Contains(w)) And ' Plain search terms
                                                wildcards.All(Function(w) wordsInText.Any(Function(wit) wit Like w)) And ' Wildcards
                                                quotedStrings.All(Function(w) cleanText.Contains(" " & w & " ") Or cleanText.StartsWith(w & " ") Or ' Quoted strings
                                                    cleanText.EndsWith(" " & w) Or cleanText = w)
                                          End Function
                       If checkTitle Then
                           If isStringAHit(Policy.DisplayName) Then Return True
                       End If
                       If checkDesc Then
                           If isStringAHit(Policy.DisplayExplanation) Then Return True
                       End If
                       If checkComment Then
                           If CommentSources.Any(Function(Source As Dictionary(Of String, String))
                                                     Return Source.ContainsKey(Policy.UniqueID) AndAlso isStringAHit(Source(Policy.UniqueID))
                                                 End Function) Then Return True
                       End If
                       Return False
                   End Function
        DialogResult = DialogResult.OK
    End Sub
End Class