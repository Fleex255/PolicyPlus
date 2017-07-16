Imports System.Xml
Public Class AdmxFile
    Public SourceFile As String
    Public AdmxNamespace As String
    Public SupersededAdm As String
    Public MinAdmlVersion As Decimal
    Public Prefixes As New Dictionary(Of String, String)
    Public Products As New List(Of AdmxProduct)
    Public SupportedOnDefinitions As New List(Of AdmxSupportDefinition)
    Public Categories As New List(Of AdmxCategory)
    Public Policies As New List(Of AdmxPolicy)
    Private Sub New()
    End Sub
    Public Shared Function Load(File As String) As AdmxFile
        ' ADMX documentation: https://technet.microsoft.com/en-us/library/cc772138(v=ws.10).aspx
        Dim admx As New AdmxFile
        admx.SourceFile = File
        Dim xmlDoc As New XmlDocument
        xmlDoc.Load(File)
        Dim policyDefinitions = xmlDoc.GetElementsByTagName("policyDefinitions")(0)
        For Each child As XmlNode In policyDefinitions.ChildNodes
            Select Case child.LocalName
                Case "policyNamespaces" ' Referenced namespaces and current namespace
                    For Each policyNamespace As XmlNode In child.ChildNodes
                        Dim prefix = policyNamespace.Attributes("prefix").Value
                        Dim fqNamespace = policyNamespace.Attributes("namespace").Value
                        If policyNamespace.LocalName = "target" Then admx.AdmxNamespace = fqNamespace
                        admx.Prefixes.Add(prefix, fqNamespace)
                    Next
                Case "supersededAdm" ' The ADM file that this ADMX supersedes
                    admx.SupersededAdm = child.Attributes("fileName").Value
                Case "resources" ' Minimum required version
                    admx.MinAdmlVersion = Decimal.Parse(child.Attributes("minRequiredRevision").Value, Globalization.CultureInfo.InvariantCulture)
                Case "supportedOn" ' Support definitions
                    For Each supportInfo As XmlNode In child.ChildNodes
                        If supportInfo.LocalName = "definitions" Then
                            For Each supportDef As XmlNode In supportInfo.ChildNodes
                                If supportDef.LocalName <> "definition" Then Continue For
                                Dim definition As New AdmxSupportDefinition
                                definition.ID = supportDef.Attributes("name").Value
                                definition.DisplayCode = supportDef.Attributes("displayName").Value
                                definition.Logic = AdmxSupportLogicType.Blank
                                For Each logicElement As XmlNode In supportDef.ChildNodes
                                    Dim canLoad As Boolean = True
                                    If logicElement.LocalName = "or" Then
                                        definition.Logic = AdmxSupportLogicType.AnyOf
                                    ElseIf logicElement.LocalName = "and" Then
                                        definition.Logic = AdmxSupportLogicType.AllOf
                                    Else
                                        canLoad = False
                                    End If
                                    If canLoad Then
                                        definition.Entries = New List(Of AdmxSupportEntry)
                                        For Each conditionElement As XmlNode In logicElement.ChildNodes
                                            If conditionElement.LocalName = "reference" Then
                                                Dim product = conditionElement.Attributes("ref").Value
                                                definition.Entries.Add(New AdmxSupportEntry With {.ProductID = product, .IsRange = False})
                                            ElseIf conditionElement.LocalName = "range" Then
                                                Dim entry As New AdmxSupportEntry With {.IsRange = True}
                                                entry.ProductID = conditionElement.Attributes("ref").Value
                                                Dim maxVerAttr = conditionElement.Attributes("maxVersionIndex")
                                                If maxVerAttr IsNot Nothing Then entry.MaxVersion = Integer.Parse(maxVerAttr.Value)
                                                Dim minVerAttr = conditionElement.Attributes("minVersionIndex")
                                                If minVerAttr IsNot Nothing Then entry.MinVersion = Integer.Parse(minVerAttr.Value)
                                                definition.Entries.Add(entry)
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                                definition.DefinedIn = admx
                                admx.SupportedOnDefinitions.Add(definition)
                            Next
                        ElseIf supportInfo.LocalName = "products" Then ' Product definitions
                            Dim loadProducts As Action(Of XmlNode, String, AdmxProduct)
                            loadProducts = Sub(Node As XmlNode, ChildTagName As String, Parent As AdmxProduct)
                                               For Each subproductElement As XmlNode In Node.ChildNodes
                                                   If subproductElement.LocalName <> ChildTagName Then Continue For
                                                   Dim product As New AdmxProduct
                                                   product.ID = subproductElement.Attributes("name").Value
                                                   product.DisplayCode = subproductElement.Attributes("displayName").Value
                                                   If Parent IsNot Nothing Then product.Version = subproductElement.Attributes("versionIndex").Value
                                                   product.Parent = Parent
                                                   product.DefinedIn = admx
                                                   admx.Products.Add(product)
                                                   If Parent Is Nothing Then
                                                       product.Type = AdmxProductType.Product
                                                       loadProducts(subproductElement, "majorVersion", product)
                                                   ElseIf Parent.Parent Is Nothing Then
                                                       product.Type = AdmxProductType.MajorRevision
                                                       loadProducts(subproductElement, "minorVersion", product)
                                                   Else
                                                       product.Type = AdmxProductType.MinorRevision
                                                   End If
                                               Next
                                           End Sub
                            loadProducts(supportInfo, "product", Nothing) ' Start the recursive load
                        End If
                    Next
                Case "categories" ' Categories
                    For Each categoryElement As XmlNode In child.ChildNodes
                        If categoryElement.LocalName <> "category" Then Continue For
                        Dim category As New AdmxCategory
                        category.ID = categoryElement.Attributes("name").Value
                        category.DisplayCode = categoryElement.Attributes("displayName").Value
                        category.ExplainCode = categoryElement.AttributeOrNull("explainText")
                        If categoryElement.HasChildNodes Then
                            Dim parentCatElement = categoryElement("parentCategory")
                            category.ParentID = parentCatElement.Attributes("ref").Value
                        End If
                        category.DefinedIn = admx
                        admx.Categories.Add(category)
                    Next
                Case "policies" ' Policy settings
                    Dim loadRegItem = Function(Node As XmlNode) As PolicyRegistryValue
                                          Dim regItem As New PolicyRegistryValue
                                          For Each subElement As XmlNode In Node.ChildNodes
                                              If subElement.LocalName = "delete" Then
                                                  regItem.RegistryType = PolicyRegistryValueType.Delete
                                                  Exit For
                                              ElseIf subElement.LocalName = "decimal" Then
                                                  regItem.RegistryType = PolicyRegistryValueType.Numeric
                                                  regItem.NumberValue = subElement.Attributes("value").Value
                                                  Exit For
                                              ElseIf subElement.LocalName = "string" Then
                                                  regItem.RegistryType = PolicyRegistryValueType.Text
                                                  regItem.StringValue = subElement.InnerText
                                                  Exit For
                                              End If
                                          Next
                                          Return regItem
                                      End Function
                    Dim loadOneRegList = Function(Node As XmlNode) As PolicyRegistrySingleList
                                             Dim singleList As New PolicyRegistrySingleList
                                             singleList.DefaultRegistryKey = AttributeOrNull(Node, "defaultKey")
                                             singleList.AffectedValues = New List(Of PolicyRegistryListEntry)
                                             For Each itemElement As XmlNode In Node.ChildNodes
                                                 If itemElement.LocalName <> "item" Then Continue For
                                                 Dim listEntry As New PolicyRegistryListEntry
                                                 listEntry.RegistryValue = itemElement.Attributes("valueName").Value
                                                 listEntry.RegistryKey = AttributeOrNull(itemElement, "key")
                                                 For Each valElement As XmlNode In itemElement.ChildNodes
                                                     If valElement.LocalName = "value" Then
                                                         listEntry.Value = loadRegItem(valElement)
                                                         Exit For
                                                     End If
                                                 Next
                                                 singleList.AffectedValues.Add(listEntry)
                                             Next
                                             Return singleList
                                         End Function
                    Dim loadOnOffValList = Function(OnValueName As String, OffValueName As String, OnListName As String, OffListName As String, Node As XmlNode) As PolicyRegistryList
                                               Dim regList As New PolicyRegistryList
                                               For Each subElement As XmlNode In Node.ChildNodes
                                                   If subElement.Name = OnValueName Then
                                                       regList.OnValue = loadRegItem(subElement)
                                                   ElseIf subElement.Name = OffValueName Then
                                                       regList.OffValue = loadRegItem(subElement)
                                                   ElseIf subElement.Name = OnListName Then
                                                       regList.OnValueList = loadOneRegList(subElement)
                                                   ElseIf subElement.Name = OffListName Then
                                                       regList.OffValueList = loadOneRegList(subElement)
                                                   End If
                                               Next
                                               Return regList
                                           End Function
                    For Each polElement As XmlNode In child.ChildNodes
                        If polElement.LocalName <> "policy" Then Continue For
                        Dim policy As New AdmxPolicy
                        policy.ID = polElement.Attributes("name").Value
                        policy.DefinedIn = admx
                        policy.DisplayCode = polElement.Attributes("displayName").Value
                        policy.RegistryKey = polElement.Attributes("key").Value
                        Dim polClass = polElement.Attributes("class").Value
                        Select Case polClass
                            Case "Machine"
                                policy.Section = AdmxPolicySection.Machine
                            Case "User"
                                policy.Section = AdmxPolicySection.User
                            Case Else
                                policy.Section = AdmxPolicySection.Both
                        End Select
                        policy.ExplainCode = AttributeOrNull(polElement, "explainText")
                        policy.PresentationID = AttributeOrNull(polElement, "presentation")
                        policy.ClientExtension = AttributeOrNull(polElement, "clientExtension")
                        policy.RegistryValue = AttributeOrNull(polElement, "valueName")
                        policy.AffectedValues = loadOnOffValList("enabledValue", "disabledValue", "enabledList", "disabledList", polElement)
                        For Each polInfo As XmlNode In polElement.ChildNodes
                            Select Case polInfo.LocalName
                                Case "parentCategory"
                                    policy.CategoryID = polInfo.Attributes("ref").Value
                                Case "supportedOn"
                                    policy.SupportedCode = polInfo.Attributes("ref").Value
                                Case "elements"
                                    policy.Elements = New List(Of PolicyElement)
                                    For Each uiElement As XmlNode In polInfo.ChildNodes
                                        Dim entry As PolicyElement = Nothing
                                        Select Case uiElement.LocalName
                                            Case "decimal"
                                                Dim decimalEntry As New DecimalPolicyElement
                                                decimalEntry.Minimum = AttributeOrDefault(uiElement, "minValue", 0)
                                                decimalEntry.Maximum = AttributeOrDefault(uiElement, "maxValue", UInteger.MaxValue)
                                                decimalEntry.NoOverwrite = AttributeOrDefault(uiElement, "soft", False)
                                                decimalEntry.StoreAsText = AttributeOrDefault(uiElement, "storeAsText", False)
                                                entry = decimalEntry
                                            Case "boolean"
                                                Dim boolEntry As New BooleanPolicyElement
                                                boolEntry.AffectedRegistry = loadOnOffValList("trueValue", "falseValue", "trueList", "falseList", uiElement)
                                                entry = boolEntry
                                            Case "text"
                                                Dim textEntry As New TextPolicyElement
                                                textEntry.MaxLength = AttributeOrDefault(uiElement, "maxLength", 255)
                                                textEntry.Required = AttributeOrDefault(uiElement, "required", False)
                                                textEntry.RegExpandSz = AttributeOrDefault(uiElement, "expandable", False)
                                                textEntry.NoOverwrite = AttributeOrDefault(uiElement, "soft", False)
                                                entry = textEntry
                                            Case "list"
                                                Dim listEntry As New ListPolicyElement
                                                listEntry.NoPurgeOthers = AttributeOrDefault(uiElement, "additive", False)
                                                listEntry.RegExpandSz = AttributeOrDefault(uiElement, "expandable", False)
                                                listEntry.UserProvidesNames = AttributeOrDefault(uiElement, "explicitValue", False)
                                                listEntry.HasPrefix = (uiElement.Attributes("valuePrefix") IsNot Nothing)
                                                listEntry.RegistryValue = AttributeOrNull(uiElement, "valuePrefix")
                                                entry = listEntry
                                            Case "enum"
                                                Dim enumEntry As New EnumPolicyElement
                                                enumEntry.Required = AttributeOrDefault(uiElement, "required", False)
                                                enumEntry.Items = New List(Of EnumPolicyElementItem)
                                                For Each itemElement As XmlNode In uiElement.ChildNodes
                                                    If itemElement.LocalName = "item" Then
                                                        Dim enumItem As New EnumPolicyElementItem
                                                        enumItem.DisplayCode = itemElement.Attributes("displayName").Value
                                                        For Each valElement As XmlNode In itemElement.ChildNodes
                                                            If valElement.LocalName = "value" Then
                                                                enumItem.Value = loadRegItem(valElement)
                                                            ElseIf valElement.LocalName = "valueList" Then
                                                                enumItem.ValueList = loadOneRegList(valElement)
                                                            End If
                                                        Next
                                                        enumEntry.Items.Add(enumItem)
                                                    End If
                                                Next
                                                entry = enumEntry
                                            Case "multiText"
                                                entry = New MultiTextPolicyElement
                                        End Select
                                        If entry IsNot Nothing Then
                                            entry.ClientExtension = AttributeOrNull(uiElement, "clientExtension")
                                            entry.RegistryKey = AttributeOrNull(uiElement, "key")
                                            If entry.RegistryValue = "" Then entry.RegistryValue = AttributeOrNull(uiElement, "valueName")
                                            entry.ID = uiElement.Attributes("id").Value
                                            entry.ElementType = uiElement.LocalName
                                            policy.Elements.Add(entry)
                                        End If
                                    Next
                            End Select
                        Next
                        admx.Policies.Add(policy)
                    Next
            End Select
        Next
        Return admx
    End Function
End Class