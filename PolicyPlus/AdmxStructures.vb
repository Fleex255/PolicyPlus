' Raw data loaded from ADMX files
Public Class AdmxProduct
    Public ID As String
    Public DisplayCode As String
    Public Type As AdmxProductType
    Public Version As Integer
    Public Parent As AdmxProduct
    Public DefinedIn As AdmxFile
End Class
Public Enum AdmxProductType
    Product
    MajorRevision
    MinorRevision
End Enum
Public Class AdmxSupportDefinition
    Public ID As String
    Public DisplayCode As String
    Public Logic As AdmxSupportLogicType
    Public Entries As List(Of AdmxSupportEntry)
    Public DefinedIn As AdmxFile
End Class
Public Enum AdmxSupportLogicType
    Blank
    AllOf
    AnyOf
End Enum
Public Class AdmxSupportEntry
    Public ProductID As String
    Public IsRange As Boolean
    Public MinVersion As Integer?
    Public MaxVersion As Integer?
End Class
Public Class AdmxCategory
    Public ID As String
    Public DisplayCode As String
    Public ExplainCode As String
    Public ParentID As String
    Public DefinedIn As AdmxFile
End Class
Public Class AdmxPolicy
    Public ID As String
    Public Section As AdmxPolicySection
    Public CategoryID As String
    Public DisplayCode As String
    Public ExplainCode As String
    Public SupportedCode As String
    Public PresentationID As String
    Public ClientExtension As String
    Public RegistryKey As String
    Public RegistryValue As String
    Public AffectedValues As PolicyRegistryList
    Public Elements As List(Of PolicyElement)
    Public DefinedIn As AdmxFile
End Class
Public Enum AdmxPolicySection
    Machine = 1
    User = 2
    Both = 3
End Enum