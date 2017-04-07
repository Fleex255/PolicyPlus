Imports System.Xml
Public Class CmtxFile
    Public Prefixes As New Dictionary(Of String, String)
    Public Comments As New Dictionary(Of String, String)
    Public Strings As New Dictionary(Of String, String)
    Public SourceFile As String
    Public Shared Function Load(File As String) As CmtxFile
        ' CMTX documentation: https://msdn.microsoft.com/en-us/library/dn605929(v=vs.85).aspx
        Dim cmtx As New CmtxFile
        cmtx.SourceFile = File
        Dim xmlDoc As New XmlDocument
        xmlDoc.Load(File)
        Dim policyComments = xmlDoc.GetElementsByTagName("policyComments")(0)
        For Each child As XmlNode In policyComments.ChildNodes
            Select Case child.LocalName
                Case "policyNamespaces" ' ADMX file prefixes
                    For Each usingElement As XmlNode In child.ChildNodes
                        If usingElement.LocalName <> "using" Then Continue For
                        Dim prefix = usingElement.AttributeOrNull("prefix")
                        Dim ns = usingElement.AttributeOrNull("namespace")
                        cmtx.Prefixes.Add(prefix, ns)
                    Next
                Case "comments" ' Policy to comment ID mapping
                    For Each admTemplateElement As XmlNode In child.ChildNodes
                        If admTemplateElement.LocalName <> "admTemplate" Then Continue For
                        For Each commentElement As XmlNode In admTemplateElement.ChildNodes
                            If commentElement.LocalName <> "comment" Then Continue For
                            Dim policy = commentElement.AttributeOrNull("policyRef")
                            Dim text = commentElement.AttributeOrNull("commentText")
                            cmtx.Comments.Add(policy, text)
                        Next
                    Next
                Case "resources" ' The actual comment text
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
    Public Shared Function FromCommentTable(Table As Dictionary(Of String, String)) As CmtxFile
        ' Create CMTX structures from a simple policy-to-comment-text mapping
        Dim cmtx As New CmtxFile
        Dim resNum As Integer = 0 ' A counter to make sure names are unique
        Dim revPrefixes As New Dictionary(Of String, String) ' Opposite-direction prefix lookup
        For Each kv In Table
            Dim idParts = Split(kv.Key, ":", 2)
            If Not revPrefixes.ContainsKey(idParts(0)) Then
                Dim prefixId = idParts(0).Replace("."c, "_"c) & "__" & resNum
                revPrefixes.Add(idParts(0), prefixId)
                cmtx.Prefixes.Add(prefixId, idParts(0))
            End If
            Dim resourceId = idParts(0).Replace("."c, "_"c) & "__" & idParts(1) & "__" & resNum
            cmtx.Strings.Add(resourceId, kv.Value)
            cmtx.Comments.Add(revPrefixes(idParts(0)) & ":" & idParts(1), "$(resource." & resourceId & ")")
            resNum += 1
        Next
        Return cmtx
    End Function
    Public Function ToCommentTable() As Dictionary(Of String, String)
        ' Create a convenient policy-to-comment-text mapping
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
    Public Sub Save()
        Save(SourceFile)
    End Sub
    Public Sub Save(File As String)
        ' Save the CMTX data to a fully compliant XML document
        Dim xml As New XmlDocument
        Dim declaration = xml.CreateXmlDeclaration("1.0", "utf-8", "")
        xml.AppendChild(declaration)
        Dim policyComments = xml.CreateElement("policyComments")
        policyComments.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
        policyComments.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        policyComments.SetAttribute("revision", "1.0")
        policyComments.SetAttribute("schemaVersion", "1.0")
        policyComments.SetAttribute("xmlns", "http://www.microsoft.com/GroupPolicy/CommentDefinitions")
        Dim policyNamespaces = xml.CreateElement("policyNamespaces")
        For Each prefix In Prefixes
            Dim usingElem As XmlElement = xml.CreateElement("using")
            usingElem.SetAttribute("prefix", prefix.Key)
            usingElem.SetAttribute("namespace", prefix.Value)
            policyNamespaces.AppendChild(usingElem)
        Next
        policyComments.AppendChild(policyNamespaces)
        Dim commentsElem = xml.CreateElement("comments")
        Dim admTemplate = xml.CreateElement("admTemplate")
        For Each comment In Comments
            Dim commentElem = xml.CreateElement("comment")
            commentElem.SetAttribute("policyRef", comment.Key)
            commentElem.SetAttribute("commentText", comment.Value)
            admTemplate.AppendChild(commentElem)
        Next
        commentsElem.AppendChild(admTemplate)
        policyComments.AppendChild(commentsElem)
        Dim resources = xml.CreateElement("resources")
        resources.SetAttribute("minRequiredRevision", "1.0")
        Dim stringTable = xml.CreateElement("stringTable")
        For Each textval In Strings
            Dim stringElem = xml.CreateElement("string")
            stringElem.SetAttribute("id", textval.Key)
            stringElem.InnerText = textval.Value
            stringTable.AppendChild(stringElem)
        Next
        resources.AppendChild(stringTable)
        policyComments.AppendChild(resources)
        xml.AppendChild(policyComments)
        xml.Save(File)
    End Sub
End Class
