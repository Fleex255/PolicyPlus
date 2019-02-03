Public Class FindByRegistry
    Public Searcher As Func(Of PolicyPlusPolicy, Boolean)
    Private Sub FindByRegistry_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim keyName = KeyTextbox.Text.ToLowerInvariant
        Dim valName = ValueTextbox.Text.ToLowerInvariant
        If keyName = "" And valName = "" Then
            MsgBox("Please enter search terms.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If {"HKLM\", "HKCU\", "HKEY_LOCAL_MACHINE\", "HKEY_CURRENT_USER\"}.Any(Function(bad) keyName.StartsWith(bad, StringComparison.InvariantCultureIgnoreCase)) Then
            MsgBox("Policies' root keys are determined only by their section. Remove the root key from the search terms and try again.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Searcher = Function(Policy As PolicyPlusPolicy) As Boolean
                       Dim affected = PolicyProcessing.GetReferencedRegistryValues(Policy)
                       For Each rkvp In affected
                           If valName <> "" Then
                               If Not rkvp.Value.ToLowerInvariant Like valName Then Continue For
                           End If
                           If keyName <> "" Then
                               If keyName.Contains("*") Or keyName.Contains("?") Then ' Wildcard path
                                   If Not rkvp.Key.ToLowerInvariant Like keyName Then Continue For
                               ElseIf keyName.Contains("\") Then ' Path root
                                   If Not rkvp.Key.StartsWith(keyName, StringComparison.InvariantCultureIgnoreCase) Then Continue For
                               Else ' One path component
                                   If Not Split(rkvp.Key, "\").Any(Function(part) part.Equals(keyName, StringComparison.InvariantCultureIgnoreCase)) Then Continue For
                               End If
                           End If
                           Return True
                       Next
                       Return False
                   End Function
        DialogResult = DialogResult.OK
    End Sub
End Class