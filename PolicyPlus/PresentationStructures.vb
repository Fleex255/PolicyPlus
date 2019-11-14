' These structures hold information on how the UI for policy elements appears
Public Class Presentation
    Public Name As String
    Public Elements As New List(Of PresentationElement)
End Class
Public MustInherit Class PresentationElement
    Public ID As String ' refId
    Public ElementType As String
End Class
Public Class LabelPresentationElement ' <text>
    Inherits PresentationElement
    Public Text As String ' Inner text
End Class
Public Class NumericBoxPresentationElement ' <decimalTextBox>
    Inherits PresentationElement
    Public DefaultValue As UInteger ' defaultValue
    Public HasSpinner As Boolean = True ' spin
    Public SpinnerIncrement As UInteger ' spinStep
    Public Label As String ' Inner text
End Class
Public Class TextBoxPresentationElement ' <textBox>
    Inherits PresentationElement
    Public Label As String ' <label>
    Public DefaultValue As String ' <defaultValue>
End Class
Public Class CheckBoxPresentationElement ' <checkBox>
    Inherits PresentationElement
    Public DefaultState As Boolean ' defaultChecked
    Public Text As String ' Inner text
End Class
Public Class ComboBoxPresentationElement ' <comboBox>
    Inherits PresentationElement
    Public NoSort As Boolean ' noSort
    Public Label As String ' <label>
    Public DefaultText As String ' <default>
    Public Suggestions As New List(Of String) ' <suggestion>s
End Class
Public Class DropDownPresentationElement ' <dropdownList>
    Inherits PresentationElement
    Public NoSort As Boolean ' noSort
    Public DefaultItemID As Integer? ' defaultItem
    Public Label As String ' Inner text
End Class
Public Class ListPresentationElement ' <listBox>
    Inherits PresentationElement
    Public Label As String ' Inner text
End Class
Public Class MultiTextPresentationElement ' <multiTextBox>
    Inherits PresentationElement
    Public Label As String ' Inner text
    ' Undocumented, but never appears to have any other parameters
End Class