Public Class PolicyProcessing
    Public Shared Function GetPolicyState(PolicySource As IPolicySource, Policy As PolicyPlusPolicy) As PolicyState
        ' Determine the basic state of a policy
        Dim enabledEvidence As Decimal = 0
        Dim disabledEvidence As Decimal = 0
        Dim rawpol = Policy.RawPolicy
        Dim checkOneVal = Sub(Value As PolicyRegistryValue, Key As String, ValueName As String, ByRef EvidenceVar As Decimal)
                              If Value Is Nothing Then Exit Sub
                              If ValuePresent(Value, PolicySource, Key, ValueName) Then EvidenceVar += 1
                          End Sub
        Dim checkValList = Sub(ValList As PolicyRegistrySingleList, DefaultKey As String, ByRef EvidenceVar As Decimal)
                               If ValList Is Nothing Then Exit Sub
                               Dim listKey = If(ValList.DefaultRegistryKey = "", DefaultKey, ValList.DefaultRegistryKey)
                               For Each regVal In ValList.AffectedValues
                                   Dim entryKey = If(regVal.RegistryKey = "", listKey, regVal.RegistryKey)
                                   checkOneVal(regVal.Value, entryKey, regVal.RegistryValue, EvidenceVar)
                               Next
                           End Sub
        ' Check the policy's standard Registry values
        If rawpol.RegistryValue <> "" Then
            If rawpol.AffectedValues.OnValue Is Nothing Then
                checkOneVal(New PolicyRegistryValue With {.NumberValue = 1UI, .RegistryType = PolicyRegistryValueType.Numeric}, rawpol.RegistryKey, rawpol.RegistryValue, enabledEvidence)
            Else
                checkOneVal(rawpol.AffectedValues.OnValue, rawpol.RegistryKey, rawpol.RegistryValue, enabledEvidence)
            End If
            If rawpol.AffectedValues.OffValue Is Nothing Then
                checkOneVal(New PolicyRegistryValue With {.RegistryType = PolicyRegistryValueType.Delete}, rawpol.RegistryKey, rawpol.RegistryValue, disabledEvidence)
            Else
                checkOneVal(rawpol.AffectedValues.OffValue, rawpol.RegistryKey, rawpol.RegistryValue, disabledEvidence)
            End If
        End If
        checkValList(rawpol.AffectedValues.OnValueList, rawpol.RegistryKey, enabledEvidence)
        checkValList(rawpol.AffectedValues.OffValueList, rawpol.RegistryKey, disabledEvidence)
        ' Check the policy's elements
        If rawpol.Elements IsNot Nothing Then
            Dim deletedElements As Decimal = 0
            Dim presentElements As Decimal = 0
            For Each elem In rawpol.Elements
                Dim elemKey = If(elem.RegistryKey = "", rawpol.RegistryKey, elem.RegistryKey)
                If elem.ElementType = "list" Then
                    Dim neededValues = 0
                    If PolicySource.WillDeleteValue(elemKey, "") Then
                        deletedElements += 1
                        neededValues = 1
                    End If
                    If PolicySource.GetValueNames(elemKey).Count > 0 Then
                        deletedElements -= neededValues
                        presentElements += 1
                    End If
                ElseIf elem.ElementType = "boolean" Then
                    Dim booleanElem As BooleanPolicyElement = elem
                    If PolicySource.WillDeleteValue(elemKey, elem.RegistryValue) Then
                        deletedElements += 1 ' Implicit checkboxes are deleted when the policy is disabled
                    Else
                        Dim checkboxDisabled As Decimal = 0
                        checkOneVal(booleanElem.AffectedRegistry.OffValue, elemKey, elem.RegistryValue, checkboxDisabled)
                        checkValList(booleanElem.AffectedRegistry.OffValueList, elemKey, checkboxDisabled)
                        deletedElements += checkboxDisabled * 0.1D ' Checkboxes in the off state are weak evidence for the policy being disabled
                        checkOneVal(booleanElem.AffectedRegistry.OnValue, elemKey, elem.RegistryValue, presentElements)
                        checkValList(booleanElem.AffectedRegistry.OnValueList, elemKey, presentElements)
                    End If
                Else
                    If PolicySource.WillDeleteValue(elemKey, elem.RegistryValue) Then
                        deletedElements += 1
                    ElseIf PolicySource.ContainsValue(elemKey, elem.RegistryValue) Then
                        presentElements += 1
                    End If
                End If
            Next
            If presentElements > 0 Then
                enabledEvidence += presentElements
            ElseIf deletedElements > 0 Then
                disabledEvidence += deletedElements
            End If
        End If
        ' Judge the evidence collected
        If enabledEvidence > disabledEvidence Then
            Return PolicyState.Enabled
        ElseIf disabledEvidence > enabledEvidence Then
            Return PolicyState.Disabled
        ElseIf enabledEvidence = 0 Then ' No evidence for either side
            Return PolicyState.NotConfigured
        Else
            Return PolicyState.Unknown
        End If
    End Function
    Private Shared Function ValuePresent(Value As PolicyRegistryValue, Source As IPolicySource, Key As String, ValueName As String) As Boolean
        ' Determine whether the given value is found in the Registry
        Select Case Value.RegistryType
            Case PolicyRegistryValueType.Delete
                Return Source.WillDeleteValue(Key, ValueName)
            Case PolicyRegistryValueType.Numeric
                If Not Source.ContainsValue(Key, ValueName) Then Return False
                Dim sourceVal = Source.GetValue(Key, ValueName)
                If TypeOf sourceVal IsNot UInteger And TypeOf sourceVal IsNot Integer Then Return False
                Return CLng(sourceVal) = Value.NumberValue
            Case PolicyRegistryValueType.Text
                If Not Source.ContainsValue(Key, ValueName) Then Return False
                Dim sourceVal = Source.GetValue(Key, ValueName)
                If TypeOf sourceVal IsNot String Then Return False
                Return CStr(sourceVal) = Value.StringValue
            Case Else
                Throw New InvalidOperationException("Illegal value type")
        End Select
    End Function
    Private Shared Function ValueListPresent(ValueList As PolicyRegistrySingleList, Source As IPolicySource, Key As String, ValueName As String) As Boolean
        ' Determine whether all the values in a value list are in the Registry data
        Dim sublistKey = If(ValueList.DefaultRegistryKey = "", Key, ValueList.DefaultRegistryKey)
        Return ValueList.AffectedValues.All(Function(e)
                                                Dim entryKey = If(e.RegistryKey = "", sublistKey, e.RegistryKey)
                                                Return ValuePresent(e.Value, Source, entryKey, e.RegistryValue)
                                            End Function)
    End Function
    Public Shared Function DeduplicatePolicies(Workspace As AdmxBundle) As Integer
        ' Merge otherwise-identical pairs of user and computer policies into both-section policies
        Dim dedupeCount = 0
        For Each cat In Workspace.Policies.GroupBy(Function(c) c.Value.Category)
            For Each namegroup In cat.GroupBy(Function(p) p.Value.DisplayName).Select(Function(x) x.ToList).ToList
                If namegroup.Count <> 2 Then Continue For
                Dim a = namegroup(0).Value
                Dim b = namegroup(1).Value
                If a.RawPolicy.Section + b.RawPolicy.Section <> AdmxPolicySection.Both Then Continue For
                If a.DisplayExplanation <> b.DisplayExplanation Then Continue For
                If a.RawPolicy.RegistryKey <> b.RawPolicy.RegistryKey Then Continue For
                a.Category.Policies.Remove(a)
                Workspace.Policies.Remove(a.UniqueID)
                b.RawPolicy.Section = AdmxPolicySection.Both
                dedupeCount += 1
            Next
        Next
        Return dedupeCount
    End Function
    Public Shared Function GetPolicyOptionStates(PolicySource As IPolicySource, Policy As PolicyPlusPolicy) As Dictionary(Of String, Object)
        ' Get the element states of an enabled policy
        Dim state As New Dictionary(Of String, Object)
        If Policy.RawPolicy.Elements Is Nothing Then Return state
        For Each elem In Policy.RawPolicy.Elements
            Dim elemKey = If(elem.RegistryKey = "", Policy.RawPolicy.RegistryKey, elem.RegistryKey)
            Select Case elem.ElementType
                Case "decimal"
                    state.Add(elem.ID, CUInt(PolicySource.GetValue(elemKey, elem.RegistryValue)))
                Case "boolean"
                    Dim booleanElem As BooleanPolicyElement = elem
                    state.Add(elem.ID, GetRegistryListState(PolicySource, booleanElem.AffectedRegistry, elemKey, elem.RegistryValue))
                Case "text"
                    state.Add(elem.ID, PolicySource.GetValue(elemKey, elem.RegistryValue))
                Case "list"
                    Dim listElem As ListPolicyElement = elem
                    If listElem.UserProvidesNames Then ' Keys matter, use a dictionary
                        Dim entries As New Dictionary(Of String, String)
                        For Each value In PolicySource.GetValueNames(elemKey)
                            entries.Add(value, PolicySource.GetValue(elemKey, value))
                        Next
                        state.Add(elem.ID, entries)
                    Else ' Keys don't matter, use a list
                        Dim entries As New List(Of String)
                        If listElem.HasPrefix Then
                            Dim n As Integer = 1
                            Do While PolicySource.ContainsValue(elemKey, elem.RegistryValue & n)
                                entries.Add(PolicySource.GetValue(elemKey, elem.RegistryValue & n))
                                n += 1
                            Loop
                        Else
                            For Each value In PolicySource.GetValueNames(elemKey)
                                entries.Add(value)
                            Next
                        End If
                        state.Add(elem.ID, entries)
                    End If
                Case "enum"
                    ' Determine which option has results that match the Registry
                    Dim enumElem As EnumPolicyElement = elem
                    Dim selectedIndex As Integer = -1
                    For n = 0 To enumElem.Items.Count - 1
                        Dim enumItem = enumElem.Items(n)
                        If ValuePresent(enumItem.Value, PolicySource, elemKey, elem.RegistryValue) Then
                            If enumItem.ValueList Is Nothing OrElse ValueListPresent(enumItem.ValueList, PolicySource, elemKey, elem.RegistryValue) Then
                                selectedIndex = n
                                Exit For
                            End If
                        End If
                    Next
                    state.Add(elem.ID, selectedIndex)
                Case "multiText"
                    state.Add(elem.ID, PolicySource.GetValue(elemKey, elem.RegistryValue))
            End Select
        Next
        Return state
    End Function
    Private Shared Function GetRegistryListState(PolicySource As IPolicySource, RegList As PolicyRegistryList, DefaultKey As String, DefaultValueName As String) As Boolean
        ' Whether a list of Registry values is present
        Dim isListAllPresent = Function(l As PolicyRegistrySingleList) ValueListPresent(l, PolicySource, DefaultKey, DefaultValueName)
        If RegList.OnValue IsNot Nothing Then
            If ValuePresent(RegList.OnValue, PolicySource, DefaultKey, DefaultValueName) Then Return True
        ElseIf RegList.OnValueList IsNot Nothing Then
            If isListAllPresent(RegList.OnValueList) Then Return True
        Else
            If CUInt(PolicySource.GetValue(DefaultKey, DefaultValueName)) = 1UI Then Return True
        End If
        If RegList.OffValue IsNot Nothing Then
            If ValuePresent(RegList.OffValue, PolicySource, DefaultKey, DefaultValueName) Then Return False
        ElseIf RegList.OffValueList IsNot Nothing Then
            If isListAllPresent(RegList.OffValueList) Then Return False
        End If
        Return False
    End Function
    Public Shared Function GetReferencedRegistryValues(Policy As PolicyPlusPolicy) As List(Of RegistryKeyValuePair)
        Return WalkPolicyRegistry(Nothing, Policy, False)
    End Function
    Public Shared Sub ForgetPolicy(PolicySource As IPolicySource, Policy As PolicyPlusPolicy)
        WalkPolicyRegistry(PolicySource, Policy, True)
    End Sub
    Private Shared Function WalkPolicyRegistry(PolicySource As IPolicySource, Policy As PolicyPlusPolicy, Forget As Boolean) As List(Of RegistryKeyValuePair)
        ' This function handles both GetReferencedRegistryValues and ForgetPolicy because they require searching through the same things
        Dim entries As New List(Of RegistryKeyValuePair)
        Dim addReg = Sub(Key As String, Value As String)
                         Dim rkvp As New RegistryKeyValuePair With {.Key = Key, .Value = Value}
                         If Not entries.Contains(rkvp) Then entries.Add(rkvp)
                     End Sub
        ' Get all Registry values affected by this policy
        Dim rawpol = Policy.RawPolicy
        If rawpol.RegistryValue <> "" Then addReg(rawpol.RegistryKey, rawpol.RegistryValue)
        Dim addSingleList = Sub(SingleList As PolicyRegistrySingleList, OverrideKey As String)
                                If SingleList Is Nothing Then Exit Sub
                                Dim defaultKey = If(OverrideKey = "", rawpol.RegistryKey, OverrideKey)
                                Dim listKey = If(SingleList.DefaultRegistryKey = "", defaultKey, SingleList.DefaultRegistryKey)
                                For Each e In SingleList.AffectedValues
                                    Dim entryKey = If(e.RegistryKey = "", listKey, e.RegistryKey)
                                    addReg(entryKey, e.RegistryValue)
                                Next
                            End Sub
        addSingleList(rawpol.AffectedValues.OnValueList, "")
        addSingleList(rawpol.AffectedValues.OffValueList, "")
        If rawpol.Elements IsNot Nothing Then
            For Each elem In rawpol.Elements
                Dim elemKey As String = If(elem.RegistryKey = "", rawpol.RegistryKey, elem.RegistryKey)
                If elem.ElementType <> "list" Then addReg(elemKey, elem.RegistryValue)
                Select Case elem.ElementType
                    Case "boolean"
                        Dim booleanElem As BooleanPolicyElement = elem
                        addSingleList(booleanElem.AffectedRegistry.OnValueList, elemKey)
                        addSingleList(booleanElem.AffectedRegistry.OffValueList, elemKey)
                    Case "enum"
                        Dim enumElem As EnumPolicyElement = elem
                        For Each e In enumElem.Items
                            addSingleList(e.ValueList, elemKey)
                        Next
                    Case "list"
                        If Forget Then
                            PolicySource.ClearKey(elemKey) ' Delete all the values
                            PolicySource.ForgetKeyClearance(elemKey)
                        Else
                            addReg(elemKey, "")
                        End If
                End Select
            Next
        End If
        If Forget Then ' Remove them if forgetting
            For Each e In entries
                PolicySource.ForgetValue(e.Key, e.Value)
            Next
        End If
        Return entries
    End Function
    Public Shared Sub SetPolicyState(PolicySource As IPolicySource, Policy As PolicyPlusPolicy, State As PolicyState, Options As Dictionary(Of String, Object))
        ' Write a full policy state to the policy source
        Dim setValue = Sub(Key As String, ValueName As String, Value As PolicyRegistryValue)
                           If Value Is Nothing Then Exit Sub
                           Select Case Value.RegistryType
                               Case PolicyRegistryValueType.Delete
                                   PolicySource.DeleteValue(Key, ValueName)
                               Case PolicyRegistryValueType.Numeric
                                   PolicySource.SetValue(Key, ValueName, Value.NumberValue, Microsoft.Win32.RegistryValueKind.DWord)
                               Case PolicyRegistryValueType.Text
                                   PolicySource.SetValue(Key, ValueName, Value.StringValue, Microsoft.Win32.RegistryValueKind.String)
                           End Select
                       End Sub
        Dim setSingleList = Sub(SingleList As PolicyRegistrySingleList, DefaultKey As String)
                                If SingleList Is Nothing Then Exit Sub
                                Dim listKey As String = If(SingleList.DefaultRegistryKey = "", DefaultKey, SingleList.DefaultRegistryKey)
                                For Each e In SingleList.AffectedValues
                                    Dim itemKey As String = If(e.RegistryKey = "", listKey, e.RegistryKey)
                                    setValue(itemKey, e.RegistryValue, e.Value)
                                Next
                            End Sub
        Dim setList = Sub(List As PolicyRegistryList, DefaultKey As String, DefaultValue As String, IsOn As Boolean)
                          If List Is Nothing Then Exit Sub
                          If IsOn Then
                              setValue(DefaultKey, DefaultValue, List.OnValue)
                              setSingleList(List.OnValueList, DefaultKey)
                          Else
                              setValue(DefaultKey, DefaultValue, List.OffValue)
                              setSingleList(List.OffValueList, DefaultKey)
                          End If
                      End Sub
        Dim rawpol = Policy.RawPolicy
        Select Case State
            Case PolicyState.Enabled
                If rawpol.AffectedValues.OnValue Is Nothing And rawpol.RegistryValue <> "" Then PolicySource.SetValue(rawpol.RegistryKey, rawpol.RegistryValue, 1UI, Microsoft.Win32.RegistryValueKind.DWord)
                setList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, True)
                If rawpol.Elements IsNot Nothing Then ' Write the elements' states
                    For Each elem In rawpol.Elements
                        Dim elemKey = If(elem.RegistryKey = "", rawpol.RegistryKey, elem.RegistryKey)
                        If Not Options.ContainsKey(elem.ID) Then Continue For
                        Dim optionData = Options(elem.ID)
                        Select Case elem.ElementType
                            Case "decimal"
                                Dim decimalElem As DecimalPolicyElement = elem
                                If decimalElem.StoreAsText Then
                                    PolicySource.SetValue(elemKey, elem.RegistryValue, CStr(optionData), Microsoft.Win32.RegistryValueKind.String)
                                Else
                                    PolicySource.SetValue(elemKey, elem.RegistryValue, CUInt(optionData), Microsoft.Win32.RegistryValueKind.DWord)
                                End If
                            Case "boolean"
                                Dim booleanElem As BooleanPolicyElement = elem
                                Dim checkState As Boolean = optionData
                                If booleanElem.AffectedRegistry.OnValue Is Nothing And checkState Then
                                    PolicySource.SetValue(elemKey, elem.RegistryValue, 1UI, Microsoft.Win32.RegistryValueKind.DWord)
                                End If
                                If booleanElem.AffectedRegistry.OffValue Is Nothing And Not checkState Then
                                    PolicySource.DeleteValue(elemKey, elem.RegistryValue)
                                End If
                                setList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, checkState)
                            Case "text"
                                Dim textElem As TextPolicyElement = elem
                                Dim regType = If(textElem.RegExpandSz, Microsoft.Win32.RegistryValueKind.ExpandString, Microsoft.Win32.RegistryValueKind.String)
                                PolicySource.SetValue(elemKey, elem.RegistryValue, optionData, regType)
                            Case "list"
                                Dim listElem As ListPolicyElement = elem
                                If Not listElem.NoPurgeOthers Then PolicySource.ClearKey(elemKey)
                                If optionData Is Nothing Then Continue For
                                Dim regType = If(listElem.RegExpandSz, Microsoft.Win32.RegistryValueKind.ExpandString, Microsoft.Win32.RegistryValueKind.String)
                                If listElem.UserProvidesNames Then
                                    Dim items As Dictionary(Of String, String) = optionData
                                    For Each i In items
                                        PolicySource.SetValue(elemKey, i.Key, i.Value, regType)
                                    Next
                                Else
                                    Dim items As List(Of String) = optionData
                                    Dim n As Integer = 1
                                    Do While n <= items.Count
                                        Dim valueName As String = If(listElem.HasPrefix, listElem.RegistryValue & n, items(n - 1))
                                        PolicySource.SetValue(elemKey, valueName, items(n - 1), regType)
                                        n += 1
                                    Loop
                                End If
                            Case "enum"
                                Dim enumElem As EnumPolicyElement = elem
                                Dim selItem = enumElem.Items(optionData)
                                setValue(elemKey, elem.RegistryValue, selItem.Value)
                                setSingleList(selItem.ValueList, elemKey)
                            Case "multiText"
                                PolicySource.SetValue(elemKey, elem.RegistryValue, optionData, Microsoft.Win32.RegistryValueKind.MultiString)
                        End Select
                    Next
                End If
            Case PolicyState.Disabled
                If rawpol.AffectedValues.OffValue Is Nothing And rawpol.RegistryValue <> "" Then PolicySource.DeleteValue(rawpol.RegistryKey, rawpol.RegistryValue)
                setList(rawpol.AffectedValues, rawpol.RegistryKey, rawpol.RegistryValue, False)
                If rawpol.Elements IsNot Nothing Then ' Mark all the elements deleted
                    For Each elem In rawpol.Elements
                        Dim elemKey = If(elem.RegistryKey = "", rawpol.RegistryKey, elem.RegistryKey)
                        If elem.ElementType = "list" Then
                            PolicySource.ClearKey(elemKey)
                        ElseIf elem.ElementType = "boolean" Then
                            Dim booleanElem As BooleanPolicyElement = elem
                            If booleanElem.AffectedRegistry.OffValue IsNot Nothing Or booleanElem.AffectedRegistry.OffValueList IsNot Nothing Then
                                ' Non-implicit checkboxes get their "off" value set when the policy is disabled
                                setList(booleanElem.AffectedRegistry, elemKey, elem.RegistryValue, False)
                            Else
                                PolicySource.DeleteValue(elemKey, elem.RegistryValue)
                            End If
                        Else
                            PolicySource.DeleteValue(elemKey, elem.RegistryValue)
                        End If
                    Next
                End If
        End Select
    End Sub
    Public Shared Function IsPolicySupported(Policy As PolicyPlusPolicy, Products As List(Of PolicyPlusProduct), AlwaysUseAny As Boolean, ApproveLiterals As Boolean) As Boolean
        ' Whether a policy is supported on a computer with the given products
        If Policy.SupportedOn Is Nothing OrElse Policy.SupportedOn.RawSupport.Logic = AdmxSupportLogicType.Blank Then Return ApproveLiterals
        Dim supEntryMet = Function(SupportEntry As PolicyPlusSupportEntry) As Boolean ' Only for products (not support definitions)
                              If SupportEntry.Product Is Nothing Then Return ApproveLiterals
                              If Products.Contains(SupportEntry.Product) And Not SupportEntry.RawSupportEntry.IsRange Then Return True
                              If SupportEntry.Product.Children Is Nothing OrElse SupportEntry.Product.Children.Count = 0 Then Return False ' Ranges only apply to parent products
                              Dim rangeMin = If(SupportEntry.RawSupportEntry.MinVersion, 0)
                              Dim rangeMax = If(SupportEntry.RawSupportEntry.MaxVersion, SupportEntry.Product.Children.Max(Function(p) p.RawProduct.Version))
                              For v = rangeMin To rangeMax
                                  Dim version = v ' To suppress compiler warnings about iteration variable in lambdas
                                  Dim subproduct = SupportEntry.Product.Children.FirstOrDefault(Function(p) p.RawProduct.Version = version)
                                  If subproduct Is Nothing Then Continue For
                                  If Products.Contains(subproduct) Then Return True
                                  If subproduct.Children IsNot Nothing AndAlso subproduct.Children.Any(Function(p) Products.Contains(p)) Then Return True
                              Next
                              Return False
                          End Function
        Dim entriesSeen As New List(Of PolicyPlusSupport)
        Dim supDefMet As Func(Of PolicyPlusSupport, Boolean)
        supDefMet = Function(Support As PolicyPlusSupport) As Boolean
                        If entriesSeen.Contains(Support) Then Return False ' Cyclic dependencies
                        entriesSeen.Add(Support)
                        Dim requireAll = If(AlwaysUseAny, Support.RawSupport.Logic = AdmxSupportLogicType.AllOf, False)
                        ' It's much faster to check for plain products, so do that first
                        For Each supElem In Support.Elements.Where(Function(e) e.SupportDefinition Is Nothing)
                            Dim isMet = supEntryMet(supElem)
                            If requireAll Then
                                If Not isMet Then Return False
                            Else
                                If isMet Then Return True
                            End If
                        Next
                        For Each subDef In Support.Elements.Where(Function(e) e.SupportDefinition IsNot Nothing)
                            Dim isMet = supDefMet(subDef.SupportDefinition)
                            If requireAll Then
                                If Not isMet Then Return False
                            Else
                                If isMet Then Return True
                            End If
                        Next
                        Return requireAll ' If all were required and this function hasn't exited yet, all are matched
                    End Function
        Return supDefMet(Policy.SupportedOn)
    End Function
End Class
Public Enum PolicyState
    NotConfigured = 0
    Disabled = 1
    Enabled = 2
    Unknown = 3
End Enum
Public Class RegistryKeyValuePair
    Implements IEquatable(Of RegistryKeyValuePair)
    Public Key As String
    Public Value As String
    Public Function EqualsRKVP(other As RegistryKeyValuePair) As Boolean Implements IEquatable(Of RegistryKeyValuePair).Equals
        Return other.Key.Equals(Key, StringComparison.InvariantCultureIgnoreCase) And other.Value.Equals(Value, StringComparison.InvariantCultureIgnoreCase)
    End Function
    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj IsNot RegistryKeyValuePair Then Return False
        Return EqualsRKVP(obj)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return Key.ToLowerInvariant.GetHashCode Xor Value.ToLowerInvariant.GetHashCode
    End Function
End Class