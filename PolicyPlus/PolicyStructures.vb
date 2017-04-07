' These structures hold information on the behavior of policies and policy elements
Public Class PolicyRegistryList
    Public OnValue As PolicyRegistryValue
    Public OnValueList As PolicyRegistrySingleList
    Public OffValue As PolicyRegistryValue
    Public OffValueList As PolicyRegistrySingleList
End Class
Public Class PolicyRegistrySingleList
    Public DefaultRegistryKey As String
    Public AffectedValues As List(Of PolicyRegistryListEntry)
End Class
Public Class PolicyRegistryValue ' <value>
    Public RegistryType As PolicyRegistryValueType
    Public StringValue As String
    Public NumberValue As UInteger
End Class
Public Class PolicyRegistryListEntry ' <item>
    Public RegistryValue As String
    Public RegistryKey As String
    Public Value As PolicyRegistryValue
End Class
Public Enum PolicyRegistryValueType
    Delete
    Numeric
    Text
End Enum
Public MustInherit Class PolicyElement
    Public ID As String
    Public ClientExtension As String
    Public RegistryKey As String
    Public RegistryValue As String
    Public ElementType As String
End Class
Public Class DecimalPolicyElement ' <decimal>
    Inherits PolicyElement
    Public Required As Boolean
    Public Minimum As UInteger
    Public Maximum As UInteger = UInteger.MaxValue
    Public StoreAsText As Boolean
    Public NoOverwrite As Boolean
End Class
Public Class BooleanPolicyElement ' <boolean>
    Inherits PolicyElement
    Public AffectedRegistry As PolicyRegistryList
End Class
Public Class TextPolicyElement ' <text>
    Inherits PolicyElement
    Public Required As Boolean
    Public MaxLength As Integer
    Public RegExpandSz As Boolean
    Public NoOverwrite As Boolean
End Class
Public Class ListPolicyElement ' <list>
    Inherits PolicyElement
    Public HasPrefix As Boolean
    Public NoPurgeOthers As Boolean
    Public RegExpandSz As Boolean
    Public UserProvidesNames As Boolean
End Class
Public Class EnumPolicyElement ' <enum>
    Inherits PolicyElement
    Public Required As Boolean
    Public Items As List(Of EnumPolicyElementItem)
End Class
Public Class EnumPolicyElementItem ' <item>
    Public DisplayCode As String
    Public Value As PolicyRegistryValue
    Public ValueList As PolicyRegistrySingleList ' <valueList>
End Class
Public Class MultiTextPolicyElement ' <multiText>
    Inherits PolicyElement
    ' This is undocumented, so it's unknown whether there can be other options for it
End Class