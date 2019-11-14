Imports System.Xml
Public Class AdmlFile
    Public SourceFile As String
    Public Revision As Decimal
    Public DisplayName As String
    Public Description As String
    Public StringTable As New Dictionary(Of String, String)
    Public PresentationTable As New Dictionary(Of String, Presentation)
    Private Sub New()
    End Sub
    Public Shared Function Load(File As String) As AdmlFile
        ' ADML documentation: https://technet.microsoft.com/en-us/library/cc772050(v=ws.10).aspx
        Dim adml As New AdmlFile
        adml.SourceFile = File
        Dim xmlDoc As New XmlDocument
        xmlDoc.Load(File)
        ' Load ADML metadata
        Dim policyDefinitionResources As XmlNode = xmlDoc.GetElementsByTagName("policyDefinitionResources")(0)
        adml.Revision = Decimal.Parse(policyDefinitionResources.Attributes("revision").Value, Globalization.CultureInfo.InvariantCulture)
        For Each child As XmlNode In policyDefinitionResources.ChildNodes
            Select Case child.LocalName
                Case "displayName"
                    adml.DisplayName = child.InnerText
                Case "description"
                    adml.Description = child.InnerText
            End Select
        Next
        ' Load localized strings
        Dim stringTableList = xmlDoc.GetElementsByTagName("stringTable")
        If stringTableList.Count > 0 Then
            Dim stringTable = stringTableList(0)
            For Each stringElement As XmlNode In stringTable.ChildNodes
                If stringElement.LocalName <> "string" Then Continue For
                Dim key = stringElement.Attributes("id").Value
                Dim value = stringElement.InnerText
                adml.StringTable.Add(key, value)
            Next
        End If
        ' Load presentations (UI arrangements)
        Dim presTableList = xmlDoc.GetElementsByTagName("presentationTable")
        If presTableList.Count > 0 Then
            Dim presTable = presTableList(0)
            For Each presElement As XmlNode In presTable.ChildNodes
                If presElement.LocalName <> "presentation" Then Continue For
                Dim presentation As New Presentation
                presentation.Name = presElement.Attributes("id").Value
                For Each uiElement As XmlNode In presElement.ChildNodes
                    Dim presPart As PresentationElement = Nothing
                    Select Case uiElement.LocalName
                        Case "text"
                            Dim textPart As New LabelPresentationElement
                            textPart.Text = uiElement.InnerText
                            presPart = textPart
                        Case "decimalTextBox"
                            Dim decTextPart As New NumericBoxPresentationElement
                            decTextPart.DefaultValue = AttributeOrDefault(uiElement, "defaultValue", 1)
                            decTextPart.HasSpinner = AttributeOrDefault(uiElement, "spin", True)
                            decTextPart.SpinnerIncrement = AttributeOrDefault(uiElement, "spinStep", 1)
                            decTextPart.Label = uiElement.InnerText
                            presPart = decTextPart
                        Case "textBox"
                            Dim textPart As New TextBoxPresentationElement
                            For Each textboxInfo As XmlNode In uiElement.ChildNodes
                                Select Case textboxInfo.LocalName
                                    Case "label"
                                        textPart.Label = textboxInfo.InnerText
                                    Case "defaultValue"
                                        textPart.DefaultValue = textboxInfo.InnerText
                                End Select
                            Next
                            presPart = textPart
                        Case "checkBox"
                            Dim checkPart As New CheckBoxPresentationElement
                            checkPart.DefaultState = AttributeOrDefault(uiElement, "defaultChecked", False)
                            checkPart.Text = uiElement.InnerText
                            presPart = checkPart
                        Case "comboBox"
                            Dim comboPart As New ComboBoxPresentationElement
                            comboPart.NoSort = AttributeOrDefault(uiElement, "noSort", False)
                            For Each comboInfo As XmlNode In uiElement.ChildNodes
                                Select Case comboInfo.LocalName
                                    Case "label"
                                        comboPart.Label = comboInfo.InnerText
                                    Case "default"
                                        comboPart.DefaultText = comboInfo.InnerText
                                    Case "suggestion"
                                        comboPart.Suggestions.Add(comboInfo.InnerText)
                                End Select
                            Next
                            presPart = comboPart
                        Case "dropdownList"
                            Dim dropPart As New DropDownPresentationElement
                            dropPart.NoSort = AttributeOrDefault(uiElement, "noSort", False)
                            dropPart.DefaultItemID = AttributeOrNull(uiElement, "defaultItem")
                            dropPart.Label = uiElement.InnerText
                            presPart = dropPart
                        Case "listBox"
                            Dim listPart As New ListPresentationElement
                            listPart.Label = uiElement.InnerText
                            presPart = listPart
                        Case "multiTextBox"
                            Dim multiTextPart As New MultiTextPresentationElement
                            multiTextPart.Label = uiElement.InnerText
                            presPart = multiTextPart
                    End Select
                    If presPart IsNot Nothing Then
                        If uiElement.Attributes("refId") IsNot Nothing Then presPart.ID = uiElement.Attributes("refId").Value
                        presPart.ElementType = uiElement.LocalName
                        presentation.Elements.Add(presPart)
                    End If
                Next
                adml.PresentationTable.Add(presentation.Name, presentation)
            Next
        End If
        Return adml
    End Function
End Class