Public Class InspectPolicyElements
    Dim SelectedPolicy As PolicyPlusPolicy
    Public Sub PresentDialog(Policy As PolicyPlusPolicy, Images As ImageList, AdmxWorkspace As AdmxBundle)
        SelectedPolicy = Policy
        PolicyNameTextbox.Text = Policy.DisplayName
        InfoTreeview.Nodes.Clear()
        InfoTreeview.ImageList = Images
        InfoTreeview.Nodes.Add("Registry key: " & Policy.RawPolicy.RegistryKey).ImageIndex = 0 ' Folder
        If Policy.RawPolicy.RegistryValue <> "" Then InfoTreeview.Nodes.Add("Registry value: " & Policy.RawPolicy.RegistryValue).ImageIndex = 13 ' Gear
        If Policy.RawPolicy.ClientExtension <> "" Then InfoTreeview.Nodes.Add("Client extension: " & Policy.RawPolicy.ClientExtension).ImageIndex = 19 ' DOS window
        ' Methods for adding info on a policy Registry information object
        Dim addValueData = Sub(RegVal As PolicyRegistryValue, Node As TreeNode)
                               Select Case RegVal.RegistryType
                                   Case PolicyRegistryValueType.Delete
                                       Node.Nodes.Add("Delete value").ImageIndex = 18 ' Delete
                                   Case PolicyRegistryValueType.Numeric
                                       Node.Nodes.Add("Numeric value: " & RegVal.NumberValue).ImageIndex = 15 ' Calculator
                                   Case PolicyRegistryValueType.Text
                                       Node.Nodes.Add("Text value: """ & RegVal.StringValue & """").ImageIndex = 14 ' Text
                               End Select
                           End Sub
        Dim addListEntry = Sub(RegVal As PolicyRegistryListEntry, Node As TreeNode)
                               Dim entryNode = Node.Nodes.Add("Set a value")
                               entryNode.ImageIndex = 16 ' Gear with pencil
                               If RegVal.RegistryKey <> "" Then entryNode.Nodes.Add("Registry key: " & RegVal.RegistryKey).ImageIndex = 0
                               entryNode.Nodes.Add("Registry value: " & RegVal.RegistryValue).ImageIndex = 13
                               addValueData(RegVal.Value, entryNode)
                           End Sub
        Dim addSingleListContents = Sub(SingleList As PolicyRegistrySingleList, Node As TreeNode)
                                        If SingleList.DefaultRegistryKey <> "" Then Node.Nodes.Add("Registry key: " & SingleList.DefaultRegistryKey).ImageIndex = 0
                                        For Each entry In SingleList.AffectedValues
                                            addListEntry(entry, Node)
                                        Next
                                    End Sub
        Dim addList = Sub(RegList As PolicyRegistryList, Nodes As TreeNodeCollection, HasValue As Boolean)
                          Dim listNode = Nodes.Add("Affected Registry settings")
                          listNode.ImageIndex = 12 ' Database
                          If RegList.OnValue IsNot Nothing Then
                              Dim onNode = listNode.Nodes.Add("Set when enabled")
                              onNode.ImageIndex = 17 ' Checkmark
                              addValueData(RegList.OnValue, onNode)
                          End If
                          If RegList.OnValueList IsNot Nothing Then
                              Dim onListNode = listNode.Nodes.Add("Set list when enabled")
                              onListNode.ImageIndex = 17
                              addSingleListContents(RegList.OnValueList, onListNode)
                          End If
                          If RegList.OffValue IsNot Nothing Then
                              Dim offNode = listNode.Nodes.Add("Set when disabled")
                              offNode.ImageIndex = 8 ' Minus
                              addValueData(RegList.OffValue, offNode)
                          End If
                          If RegList.OffValueList IsNot Nothing Then
                              Dim offListNode = listNode.Nodes.Add("Set list when disabled")
                              offListNode.ImageIndex = 8
                              addSingleListContents(RegList.OffValueList, offListNode)
                          End If
                          If listNode.Nodes.Count = 0 Then listNode.Nodes.Add(If(HasValue, "Left implicit", "Left to elements")).ImageIndex = 37
                      End Sub
        ' Add the policy's basic Registry info
        addList(Policy.RawPolicy.AffectedValues, InfoTreeview.Nodes, Policy.RawPolicy.RegistryValue <> "")
        ' Add all the info on the policy's elements
        If Policy.Presentation IsNot Nothing And Policy.RawPolicy.Elements IsNot Nothing Then
            Dim presNode = InfoTreeview.Nodes.Add("Presentation: " & Policy.Presentation.Name)
            presNode.ImageIndex = 20 ' Form
            For Each presElem In Policy.Presentation.Elements
                Dim presPartNode = presNode.Nodes.Add("Presentation element (type: " & presElem.ElementType & ")" & If(presElem.ID <> "", ", ID: " & presElem.ID & "", ""))
                Select Case presElem.ElementType
                    Case "text"
                        Dim labelPres As LabelPresentationElement = presElem
                        presPartNode.ImageIndex = 21 ' Text rows
                        presPartNode.Nodes.Add("Text: """ & labelPres.Text & """").ImageIndex = 14
                    Case "decimalTextBox"
                        Dim decTextPres As NumericBoxPresentationElement = presElem
                        presPartNode.ImageIndex = 22 ' Calculator with pencil
                        If decTextPres.Label <> "" Then presPartNode.Nodes.Add("Label: """ & decTextPres.Label & """").ImageIndex = 14
                        presPartNode.Nodes.Add("Default: " & decTextPres.DefaultValue).ImageIndex = 23 ' Wrench
                        presPartNode.Nodes.Add(If(decTextPres.HasSpinner, "Spinner increment: " & decTextPres.SpinnerIncrement, "No spinner")).ImageIndex = 6
                    Case "textBox"
                        Dim textPres As TextBoxPresentationElement = presElem
                        presPartNode.ImageIndex = 24 ' Text field
                        If textPres.Label <> "" Then presPartNode.Nodes.Add("Label: """ & textPres.Label & """").ImageIndex = 14
                        presPartNode.Nodes.Add("Default: """ & textPres.DefaultValue & """").ImageIndex = 23
                    Case "checkBox"
                        Dim checkPres As CheckBoxPresentationElement = presElem
                        presPartNode.ImageIndex = 25 ' Tickmark
                        presPartNode.Nodes.Add("Text: """ & checkPres.Text & """").ImageIndex = 14
                        presPartNode.Nodes.Add("Default: " & If(checkPres.DefaultState, "checked", "unchecked")).ImageIndex = 23
                    Case "comboBox"
                        Dim comboPres As ComboBoxPresentationElement = presElem
                        presPartNode.ImageIndex = 26 ' Bar with text
                        If comboPres.Label <> "" Then presPartNode.Nodes.Add("Label: """ & comboPres.Label & """").ImageIndex = 14
                        presPartNode.Nodes.Add("Default: """ & comboPres.DefaultText & """").ImageIndex = 23
                        presPartNode.Nodes.Add("Sorting: " & If(comboPres.NoSort, "from ADMX", "alphabetical")).ImageIndex = 28 ' Sorted table
                        If comboPres.Suggestions IsNot Nothing Then
                            Dim sugNode = presPartNode.Nodes.Add(comboPres.Suggestions.Count & " suggestions")
                            sugNode.ImageIndex = 29 ' Letter
                            For Each sug In comboPres.Suggestions
                                sugNode.Nodes.Add("""" & sug & """").ImageIndex = 14
                            Next
                        End If
                    Case "dropdownList"
                        Dim dropdownPres As DropDownPresentationElement = presElem
                        presPartNode.ImageIndex = 30 ' List view
                        If dropdownPres.Label <> "" Then presPartNode.Nodes.Add("Label: """ & dropdownPres.Label & """").ImageIndex = 14
                        If dropdownPres.DefaultItemID.HasValue Then presPartNode.Nodes.Add("Default: #" & dropdownPres.DefaultItemID.Value).ImageIndex = 23
                        presPartNode.Nodes.Add("Sorting: " & If(dropdownPres.NoSort, "from ADMX", "alphabetical")).ImageIndex = 28
                    Case "listBox"
                        Dim listPres As ListPresentationElement = presElem
                        presPartNode.ImageIndex = 27 ' Table window
                        presPartNode.Nodes.Add("Label: """ & listPres.Label & """").ImageIndex = 14
                    Case "multiTextBox"
                        Dim multiTextPres As MultiTextPresentationElement = presElem
                        presPartNode.ImageIndex = 38 ' Cascading boxes
                        presPartNode.Nodes.Add("Label: """ & multiTextPres.Label & """").ImageIndex = 14
                End Select
                If presElem.ID = "" Then Continue For
                Dim elem = Policy.RawPolicy.Elements.First(Function(e) e.ID = presElem.ID)
                Dim elemNode = presPartNode.Nodes.Add("Policy element (type: " & elem.ElementType & ")")
                elemNode.ImageIndex = 31 ' Brick
                If elem.ClientExtension <> "" Then elemNode.Nodes.Add("Client extension: " & elem.ClientExtension).ImageIndex = 19
                If elem.RegistryKey <> "" Then elemNode.Nodes.Add("Registry key: " & elem.RegistryKey).ImageIndex = 0
                If elem.ElementType <> "list" Then elemNode.Nodes.Add("Registry value: " & elem.RegistryValue).ImageIndex = 13
                Select Case elem.ElementType
                    Case "decimal"
                        Dim decimalElem As DecimalPolicyElement = elem
                        elemNode.Nodes.Add("Minimum: " & decimalElem.Minimum).ImageIndex = 35 ' Down arrow
                        elemNode.Nodes.Add("Maximum: " & decimalElem.Maximum).ImageIndex = 6
                        If decimalElem.StoreAsText Then elemNode.Nodes.Add("Stored as text").ImageIndex = 33 ' Letters
                        elemNode.Nodes.Add("Required: " & If(decimalElem.Required, "yes", "no")).ImageIndex = 32 ' Exclamation
                        If decimalElem.NoOverwrite Then elemNode.Nodes.Add("Soft").ImageIndex = 34 ' Soft speaker
                    Case "boolean"
                        Dim booleanElem As BooleanPolicyElement = elem
                        addList(booleanElem.AffectedRegistry, elemNode.Nodes, True)
                    Case "text"
                        Dim textElem As TextPolicyElement = elem
                        elemNode.Nodes.Add("Maximum length: " & textElem.MaxLength).ImageIndex = 6
                        If textElem.RegExpandSz Then elemNode.Nodes.Add("Stored as expandable string").ImageIndex = 36 ' Letters with arrow
                        elemNode.Nodes.Add("Required: " & If(textElem.Required, "yes", "no")).ImageIndex = 32
                        If textElem.NoOverwrite Then elemNode.Nodes.Add("Soft").ImageIndex = 34
                    Case "list"
                        Dim listElem As ListPolicyElement = elem
                        If listElem.UserProvidesNames Then
                            elemNode.Nodes.Add("User provides value names").ImageIndex = 13
                        ElseIf listElem.HasPrefix Then
                            elemNode.Nodes.Add("Value prefix: """ & listElem.RegistryValue & """").ImageIndex = 13
                        Else
                            elemNode.Nodes.Add("No prefix (values named for their data)").ImageIndex = 13
                        End If
                        If listElem.RegExpandSz Then elemNode.Nodes.Add("Stored as expandable strings").ImageIndex = 36
                        elemNode.Nodes.Add("Preserve existing values: " & If(listElem.NoPurgeOthers, "yes", "no")).ImageIndex = 34
                    Case "enum"
                        Dim enumElem As EnumPolicyElement = elem
                        elemNode.Nodes.Add("Required: " & If(enumElem.Required, "yes", "no")).ImageIndex = 32
                        Dim itemsNode = elemNode.Nodes.Add(enumElem.Items.Count & " choices")
                        itemsNode.ImageIndex = 26
                        Dim id As Integer = 0
                        For Each item In enumElem.Items
                            Dim itemNode = itemsNode.Nodes.Add("Choice #" & id)
                            itemNode.ImageIndex = 29
                            itemNode.Nodes.Add("Display code: " & item.DisplayCode).ImageIndex = 14
                            itemNode.Nodes.Add("Display name: """ & AdmxWorkspace.ResolveString(item.DisplayCode, Policy.RawPolicy.DefinedIn) & """").ImageIndex = 21
                            addValueData(item.Value, itemNode)
                            If item.ValueList IsNot Nothing Then
                                Dim regNode = itemNode.Nodes.Add("Additional Registry settings modified")
                                regNode.ImageIndex = 12
                                addSingleListContents(item.ValueList, regNode)
                            End If
                            id += 1
                        Next
                    Case "multiText"
                        ' Has no special attributes
                End Select
            Next
        End If
        ' Make SelectedImageIndex always be the same as ImageIndex
        Dim normalizeSelIndex As Action(Of TreeNodeCollection)
        normalizeSelIndex = Sub(Nodes As TreeNodeCollection)
                                For Each node As TreeNode In Nodes
                                    node.SelectedImageIndex = node.ImageIndex
                                    normalizeSelIndex(node.Nodes)
                                Next
                            End Sub
        normalizeSelIndex(InfoTreeview.Nodes)
        ShowDialog()
    End Sub
    Private Sub InfoTreeview_KeyDown(sender As Object, e As KeyEventArgs) Handles InfoTreeview.KeyDown
        If e.KeyCode = Keys.C And e.Modifiers = Keys.Control And InfoTreeview.SelectedNode IsNot Nothing Then
            My.Computer.Clipboard.SetText(InfoTreeview.SelectedNode.Text)
        End If
    End Sub
    Private Sub PolicyDetailsButton_Click(sender As Object, e As EventArgs) Handles PolicyDetailsButton.Click
        DetailPolicy.PresentDialog(SelectedPolicy)
    End Sub
End Class