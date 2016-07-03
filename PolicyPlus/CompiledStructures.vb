Public Class PolicyPlusCategory
    Public UniqueID As String
    Public Parent As PolicyPlusCategory
    Public Children As New List(Of PolicyPlusCategory)
    Public DisplayName As String
    Public Policies As New List(Of PolicyPlusPolicy)
    Public RawCategory As AdmxCategory
End Class
Public Class PolicyPlusProduct
    Public UniqueID As String
    Public Parent As PolicyPlusProduct
    Public Children As New List(Of PolicyPlusProduct)
    Public DisplayName As String
    Public RawProduct As AdmxProduct
End Class
Public Class PolicyPlusSupport
    Public UniqueID As String
    Public DisplayName As String
    Public Elements As New List(Of PolicyPlusSupportEntry)
    Public RawSupport As AdmxSupportDefinition
End Class
Public Class PolicyPlusSupportEntry
    Public Product As PolicyPlusProduct
    Public RawSupportEntry As AdmxSupportEntry
End Class
Public Class PolicyPlusPolicy
    Public UniqueID As String
    Public Category As PolicyPlusCategory
    Public DisplayName As String
    Public DisplayExplanation As String
    Public SupportedOn As PolicyPlusSupport
    ' TODO: Presentation
    Public RawPolicy As AdmxPolicy
End Class