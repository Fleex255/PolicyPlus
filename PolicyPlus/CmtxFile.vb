Imports System.Xml
Public Class CmtxFile
    Public Prefixes As New Dictionary(Of String, String)
    Public Comments As New Dictionary(Of String, String)
    Public Strings As New Dictionary(Of String, String)
    Public SourceFile As String
    Public Shared Function Load(File As String) As CmtxFile
        Dim cmtx As New CmtxFile
        cmtx.SourceFile = File
        Dim xmlDoc As New XmlDocument
        Using fFile As New IO.FileStream(File, IO.FileMode.Open, IO.FileAccess.Read)
            fFile.Position = 3 ' Skip unknown header-like thing
            xmlDoc.Load(fFile)
        End Using
        Dim policyComments = xmlDoc.GetElementsByTagName("policyComments")(0)
        For Each child As XmlNode In policyComments.ChildNodes
            Select Case child.LocalName
                Case "policyNamespaces"
                    For Each usingElement As XmlNode In child.ChildNodes
                        Dim prefix = usingElement.AttributeOrNull("prefix")
                        Dim ns = usingElement.AttributeOrNull("namespace")
                        cmtx.Prefixes.Add(prefix, ns)
                    Next
                Case "comments"
                    For Each admTemplateElement As XmlNode In child.ChildNodes
                        If admTemplateElement.LocalName <> "admTemplate" Then Continue For
                        For Each commentElement As XmlNode In admTemplateElement.ChildNodes
                            If commentElement.LocalName <> "comment" Then Continue For
                            Dim policy = commentElement.AttributeOrNull("policyRef")
                            Dim text = commentElement.AttributeOrNull("commentText")
                            cmtx.Comments.Add(policy, text)
                        Next
                    Next
                Case "resources"
                    For Each stringTable As XmlNode In child.ChildNodes
                        If stringTable.LocalName <> "stringTable" Then Continue For
                        For Each stringElement As XmlNode In stringTable.ChildNodes
                            If stringElement.LocalName <> "string" Then Continue For
                            Dim id = stringElement.AttributeOrNull("id")
                            Dim text = stringElement.InnerText
                            cmtx.Strings.Add(id, text)
                        Next
                    Next
            End Select
        Next
        Return cmtx
    End Function
    Public Function ToCommentTable() As Dictionary(Of String, String)
        Dim commentTable As New Dictionary(Of String, String)
        For Each comment In Comments
            Dim refParts = Split(comment.Key, ":", 2)
            Dim polNamespace = Prefixes(refParts(0))
            Dim stringRef = comment.Value
            Dim stringId = stringRef.Substring(11, stringRef.Length - 12) ' "$(resource." is 11 characters long
            commentTable.Add(polNamespace & ":" & refParts(1), Strings(stringId))
        Next
        Return commentTable
    End Function
End Class
