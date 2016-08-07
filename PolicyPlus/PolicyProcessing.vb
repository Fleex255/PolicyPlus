Public Class PolicyProcessing
    Public Shared Function GetPolicyState(PolicySource As IPolicySource, Policy As PolicyPlusPolicy) As PolicyState
        Dim enabledEvidence As Integer = 0
        Dim disabledEvidence As Integer = 0
        Dim rawpol = Policy.RawPolicy
        Dim checkOneVal = Sub(Value As PolicyRegistryValue, Key As String, ValueName As String, ByRef EvidenceVar As Integer)
                              If Value Is Nothing Then Exit Sub
                              If ValuePresent(Value, PolicySource, Key, ValueName) Then EvidenceVar += 1
                          End Sub
        Dim checkValList = Sub(ValList As PolicyRegistrySingleList, DefaultKey As String, ByRef EvidenceVar As Integer)
                               If ValList Is Nothing Then Exit Sub
                               Dim listKey = IIf(ValList.DefaultRegistryKey = "", DefaultKey, ValList.DefaultRegistryKey)
                               For Each regVal In ValList.AffectedValues
                                   Dim entryKey = IIf(regVal.RegistryKey = "", listKey, regVal.RegistryKey)
                                   checkOneVal(regVal.Value, entryKey, regVal.RegistryValue, EvidenceVar)
                               Next
                           End Sub
        If rawpol.RegistryValue <> "" Then
            checkOneVal(rawpol.AffectedValues.OnValue, rawpol.RegistryKey, rawpol.RegistryValue, enabledEvidence)
            checkOneVal(rawpol.AffectedValues.OffValue, rawpol.RegistryValue, rawpol.RegistryValue, disabledEvidence)
        End If
        checkValList(rawpol.AffectedValues.OnValueList, rawpol.RegistryKey, enabledEvidence)
        checkValList(rawpol.AffectedValues.OffValueList, rawpol.RegistryKey, disabledEvidence)
        If rawpol.Elements IsNot Nothing Then
            Dim deletedElements As Integer = 0
            Dim presentElements As Integer = 0
            For Each elem In rawpol.Elements
                Dim elemKey = IIf(elem.RegistryKey = "", rawpol.RegistryKey, elem.RegistryKey)
                If PolicySource.WillDeleteValue(elemKey, elem.RegistryValue) Then
                    deletedElements += 1
                ElseIf PolicySource.ContainsValue(elemKey, elem.RegistryValue) Then
                    presentElements += 1
                End If
            Next
            If presentElements > 0 Then
                enabledEvidence += presentElements
            ElseIf deletedElements > 0 Then
                disabledEvidence += deletedElements
            End If
        End If
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
        Select Case Value.RegistryType
            Case PolicyRegistryValueType.Delete
                Return Source.WillDeleteValue(Key, ValueName)
            Case PolicyRegistryValueType.Numeric
                If Not Source.ContainsValue(Key, ValueName) Then Return False
                Dim sourceVal = Source.GetValue(Key, ValueName)
                If TypeOf sourceVal IsNot UInteger Then Return False
                Return sourceVal = Value.NumberValue
            Case PolicyRegistryValueType.Text
                If Not Source.ContainsValue(Key, ValueName) Then Return False
                Dim sourceVal = Source.GetValue(Key, ValueName)
                If TypeOf sourceVal IsNot String Then Return False
                Return sourceVal = Value.StringValue
            Case Else
                Throw New InvalidOperationException("Illegal value type")
        End Select
    End Function
    Public Shared Function DeduplicatePolicies(Workspace As AdmxBundle) As Integer
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
End Class
Public Enum PolicyState
    NotConfigured = 0
    Disabled = 1
    Enabled = 2
    Unknown = 3
End Enum
