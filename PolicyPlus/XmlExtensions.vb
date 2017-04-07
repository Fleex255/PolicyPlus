Imports System.Runtime.CompilerServices
Imports System.Xml
Public Module XmlExtensions
    ' Convenience methods for parsing XML in AdmxFile and AdmlFile
    <Extension> Public Function AttributeOrNull(Node As XmlNode, Attribute As String) As String
        If Node.Attributes(Attribute) Is Nothing Then Return Nothing Else Return Node.Attributes(Attribute).Value
    End Function
    <Extension> Public Function AttributeOrDefault(Node As XmlNode, Attribute As String, DefaultVal As Object) As Object
        If Node.Attributes(Attribute) Is Nothing Then Return DefaultVal Else Return Node.Attributes(Attribute).Value
    End Function
End Module
