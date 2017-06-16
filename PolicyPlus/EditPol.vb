Public Class EditPol
    Public Sub PresentDialog(Images As ImageList, Pol As PolFile)
        LsvPol.SmallImageList = Images
        LsvPol.Items.Clear()
        Dim addKey As Action(Of String, Integer)
        addKey = Sub(Prefix As String, Depth As Integer)
                     Dim subkeys = Pol.GetKeyNames(Prefix)
                     subkeys.Sort(StringComparer.InvariantCultureIgnoreCase)
                     For Each subkey In subkeys
                         Dim keypath = If(Prefix = "", subkey, Prefix & "\" & subkey)
                         Dim lsvi = LsvPol.Items.Add(subkey)
                         lsvi.IndentCount = Depth
                         lsvi.ImageIndex = 0 ' Folder
                         addKey(keypath, Depth + 1)
                     Next
                     Dim values = Pol.GetValueNames(Prefix)
                     values.Sort(StringComparer.InvariantCultureIgnoreCase)
                     For Each value In values
                         If value = "" Then Continue For
                         Dim data = Pol.GetValue(Prefix, value)
                         Dim kind = Pol.GetValueKind(Prefix, value)
                         Dim addToLsv = Function(ItemText As String, Icon As Integer)
                                            Dim lsvItem = LsvPol.Items.Add(ItemText, Icon)
                                            lsvItem.IndentCount = Depth
                                            Return lsvItem
                                        End Function
                         If value.Equals("**deletevalues", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete values", 8).SubItems.Add(data)
                         ElseIf value.StartsWith("**del.", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete value", 8).SubItems.Add(value.Substring(6))
                         ElseIf value.StartsWith("**delvals", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete all values", 8)
                         Else
                             Dim text As String = ""
                             Dim iconIndex As Integer
                             If TypeOf data Is String() Then
                                 text = String.Join(" ", CType(data, String()))
                                 iconIndex = 39 ' Two pages
                             ElseIf TypeOf data Is String Then
                                 text = data
                                 iconIndex = If(kind = Microsoft.Win32.RegistryValueKind.ExpandString, 42, 40) ' One page with arrow, or without
                             ElseIf TypeOf data Is UInteger Then
                                 text = data.ToString
                                 iconIndex = 15 ' Calculator
                             ElseIf TypeOf data Is ULong Then
                                 text = data.ToString
                                 iconIndex = 41 ' Calculator+
                             ElseIf TypeOf data Is Byte() Then
                                 text = BitConverter.ToString(data).Replace("-", " ")
                                 iconIndex = 13 ' Gear
                             End If
                             addToLsv(value, iconIndex).SubItems.Add(text)
                         End If
                     Next
                 End Sub
        addKey("", 0)
        ChItem.Width = LsvPol.ClientSize.Width - ChValue.Width - SystemInformation.VerticalScrollBarWidth
        ShowDialog()
    End Sub
    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        DialogResult = DialogResult.OK
    End Sub
    Private Sub EditPol_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class