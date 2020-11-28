Imports Microsoft.Win32
Public Class EditPol
    Public EditingPol As PolFile
    Dim EditingUserSource As Boolean
    Sub UpdateTree()
        ' Repopulate the main list view, keeping the scroll position in the same place
        Dim topItemIndex As Integer?
        If LsvPol.TopItem IsNot Nothing Then topItemIndex = LsvPol.TopItem.Index
        LsvPol.BeginUpdate()
        LsvPol.Items.Clear()
        Dim addKey As Action(Of String, Integer)
        addKey = Sub(Prefix As String, Depth As Integer)
                     Dim subkeys = EditingPol.GetKeyNames(Prefix)
                     subkeys.Sort(StringComparer.InvariantCultureIgnoreCase)
                     For Each subkey In subkeys
                         Dim keypath = If(Prefix = "", subkey, Prefix & "\" & subkey)
                         Dim lsvi = LsvPol.Items.Add(subkey)
                         lsvi.IndentCount = Depth
                         lsvi.ImageIndex = 0 ' Folder
                         lsvi.Tag = keypath
                         addKey(keypath, Depth + 1)
                     Next
                     Dim values = EditingPol.GetValueNames(Prefix, False)
                     values.Sort(StringComparer.InvariantCultureIgnoreCase)
                     For Each value In values
                         If value = "" Then Continue For
                         Dim data = EditingPol.GetValue(Prefix, value)
                         Dim kind = EditingPol.GetValueKind(Prefix, value)
                         Dim addToLsv = Function(ItemText As String, Icon As Integer, Deletion As Boolean)
                                            Dim lsvItem = LsvPol.Items.Add(ItemText, Icon)
                                            lsvItem.IndentCount = Depth
                                            Dim tag As New PolValueInfo With {.Name = value, .Key = Prefix}
                                            If Deletion Then
                                                tag.IsDeleter = True
                                            Else
                                                tag.Kind = kind
                                                tag.Data = data
                                            End If
                                            lsvItem.Tag = tag
                                            Return lsvItem
                                        End Function
                         If value.Equals("**deletevalues", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete values", 8, True).SubItems.Add(data.ToString())
                         ElseIf value.StartsWith("**del.", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete value", 8, True).SubItems.Add(value.Substring(6))
                         ElseIf value.StartsWith("**delvals", StringComparison.InvariantCultureIgnoreCase) Then
                             addToLsv("Delete all values", 8, True)
                         Else
                             Dim text As String = ""
                             Dim iconIndex As Integer
                             If TypeOf data Is String() Then
                                 text = String.Join(" ", CType(data, String()))
                                 iconIndex = 39 ' Two pages
                             ElseIf TypeOf data Is String Then
                                 text = data
                                 iconIndex = If(kind = RegistryValueKind.ExpandString, 42, 40) ' One page with arrow, or without
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
                             addToLsv(value, iconIndex, False).SubItems.Add(text)
                         End If
                     Next
                 End Sub
        addKey("", 0)
        LsvPol.EndUpdate()
        If topItemIndex.HasValue AndAlso LsvPol.Items.Count > topItemIndex.Value Then LsvPol.TopItem = LsvPol.Items(topItemIndex.Value)
    End Sub
    Public Sub PresentDialog(Images As ImageList, Pol As PolFile, IsUser As Boolean)
        LsvPol.SmallImageList = Images
        EditingPol = Pol
        EditingUserSource = IsUser
        UpdateTree()
        ChItem.Width = LsvPol.ClientSize.Width - ChValue.Width - SystemInformation.VerticalScrollBarWidth
        LsvPol_SelectedIndexChanged(Nothing, Nothing)
        ShowDialog()
    End Sub
    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        DialogResult = DialogResult.OK
    End Sub
    Private Sub EditPol_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Sub SelectKey(KeyPath As String)
        Dim lsvi = LsvPol.Items.OfType(Of ListViewItem).FirstOrDefault(Function(i) TypeOf i.Tag Is String AndAlso KeyPath.Equals(CStr(i.Tag), StringComparison.InvariantCultureIgnoreCase))
        If lsvi Is Nothing Then Exit Sub
        lsvi.Selected = True
        lsvi.EnsureVisible()
    End Sub
    Sub SelectValue(KeyPath As String, ValueName As String)
        Dim lsvi = LsvPol.Items.OfType(Of ListViewItem).FirstOrDefault(
            Function(Item As ListViewItem)
                If TypeOf Item.Tag IsNot PolValueInfo Then Return False
                Dim pvi As PolValueInfo = Item.Tag
                Return pvi.Key.Equals(KeyPath, StringComparison.InvariantCultureIgnoreCase) And pvi.Name.Equals(ValueName, StringComparison.InvariantCultureIgnoreCase)
            End Function)
        If lsvi Is Nothing Then Exit Sub
        lsvi.Selected = True
        lsvi.EnsureVisible()
    End Sub
    Function IsKeyNameValid(Name As String) As Boolean
        Return Not Name.Contains("\")
    End Function
    Function IsKeyNameAvailable(ContainerPath As String, Name As String) As Boolean
        Return Not EditingPol.GetKeyNames(ContainerPath).Any(Function(k) k.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
    End Function
    Private Sub ButtonAddKey_Click(sender As Object, e As EventArgs) Handles ButtonAddKey.Click
        Dim keyName = EditPolKey.PresentDialog("")
        If keyName = "" Then Exit Sub
        If Not IsKeyNameValid(keyName) Then
            MsgBox("The key name is not valid.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim containerKey As String = If(LsvPol.SelectedItems.Count > 0, LsvPol.SelectedItems(0).Tag, "")
        If Not IsKeyNameAvailable(containerKey, keyName) Then
            MsgBox("The key name is already taken.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim newPath = If(containerKey = "", keyName, containerKey & "\" & keyName)
        EditingPol.SetValue(newPath, "", Array.CreateInstance(GetType(Byte), 0), RegistryValueKind.None)
        UpdateTree()
        SelectKey(newPath)
    End Sub
    Function PromptForNewValueData(ValueName As String, CurrentData As Object, Kind As RegistryValueKind) As Object
        If Kind = RegistryValueKind.String Or Kind = RegistryValueKind.ExpandString Then
            If EditPolStringData.PresentDialog(ValueName, CurrentData) = DialogResult.OK Then
                Return EditPolStringData.TextData.Text
            Else
                Return Nothing
            End If
        ElseIf Kind = RegistryValueKind.DWord Or Kind = RegistryValueKind.QWord Then
            If EditPolNumericData.PresentDialog(ValueName, CurrentData, Kind = RegistryValueKind.QWord) = DialogResult.OK Then
                Return EditPolNumericData.NumData.Value
            Else
                Return Nothing
            End If
        ElseIf Kind = RegistryValueKind.MultiString Then
            If EditPolMultiStringData.PresentDialog(ValueName, CurrentData) = DialogResult.OK Then
                Return EditPolMultiStringData.TextData.Lines
            Else
                Return Nothing
            End If
        Else
            MsgBox("This value kind is not supported.", MsgBoxStyle.Exclamation)
            Return Nothing
        End If
    End Function
    Private Sub ButtonAddValue_Click(sender As Object, e As EventArgs) Handles ButtonAddValue.Click
        Dim keyPath As String = LsvPol.SelectedItems(0).Tag
        If EditPolValue.PresentDialog() <> DialogResult.OK Then Exit Sub
        Dim value = EditPolValue.ChosenName
        Dim kind = EditPolValue.SelectedKind
        Dim defaultData As Object
        If kind = RegistryValueKind.String Or kind = RegistryValueKind.ExpandString Then
            defaultData = ""
        ElseIf kind = RegistryValueKind.DWord Or kind = RegistryValueKind.QWord Then
            defaultData = 0
        Else ' Multi-string
            defaultData = Array.CreateInstance(GetType(String), 0)
        End If
        Dim newData = PromptForNewValueData(value, defaultData, kind)
        If newData IsNot Nothing Then
            EditingPol.SetValue(keyPath, value, newData, kind)
            UpdateTree()
            SelectValue(keyPath, value)
        End If
    End Sub
    Private Sub ButtonDeleteValue_Click(sender As Object, e As EventArgs) Handles ButtonDeleteValue.Click
        Dim tag = LsvPol.SelectedItems(0).Tag
        If TypeOf tag Is String Then
            If EditPolDelete.PresentDialog(Split(tag, "\").Last) <> DialogResult.OK Then Exit Sub
            If EditPolDelete.OptPurge.Checked Then
                EditingPol.ClearKey(tag) ' Delete everything
            ElseIf EditPolDelete.OptClearFirst.Checked Then
                EditingPol.ForgetKeyClearance(tag) ' So the clearance is before the values in the POL
                EditingPol.ClearKey(tag)
                ' Add the existing values back
                Dim index = LsvPol.SelectedIndices(0) + 1
                Do
                    If index >= LsvPol.Items.Count Then Exit Do
                    Dim subItem = LsvPol.Items(index)
                    If subItem.IndentCount <= LsvPol.SelectedItems(0).IndentCount Then Exit Do
                    If subItem.IndentCount = LsvPol.SelectedItems(0).IndentCount + 1 And TypeOf subItem.Tag Is PolValueInfo Then
                        Dim valueInfo As PolValueInfo = subItem.Tag
                        If Not valueInfo.IsDeleter Then EditingPol.SetValue(valueInfo.Key, valueInfo.Name, valueInfo.Data, valueInfo.Kind)
                    End If
                    index += 1
                Loop
            Else
                ' Delete only the specified value
                EditingPol.DeleteValue(tag, EditPolDelete.TextValueName.Text)
            End If
            UpdateTree()
            SelectKey(tag)
        Else
            Dim info As PolValueInfo = tag
            EditingPol.DeleteValue(info.Key, info.Name)
            UpdateTree()
            SelectValue(info.Key, "**del." & info.Name)
        End If
    End Sub
    Private Sub ButtonForget_Click(sender As Object, e As EventArgs) Handles ButtonForget.Click
        Dim containerKey As String = ""
        Dim tag = LsvPol.SelectedItems(0).Tag
        If TypeOf tag Is String Then
            If MsgBox("Are you sure you want to remove this key and all its contents?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            Dim keyPath As String = tag
            If keyPath.Contains("\") Then containerKey = keyPath.Remove(keyPath.LastIndexOf("\"c))
            Dim removeKey As Action(Of String)
            removeKey = Sub(Key As String)
                            For Each subkey In EditingPol.GetKeyNames(Key)
                                removeKey(Key & "\" & subkey)
                            Next
                            EditingPol.ClearKey(Key)
                            EditingPol.ForgetKeyClearance(Key)
                        End Sub
            removeKey(keyPath)
        Else
            Dim info As PolValueInfo = tag
            containerKey = info.Key
            EditingPol.ForgetValue(info.Key, info.Name)
        End If
        UpdateTree()
        If containerKey <> "" Then
            Dim pathParts = Split(containerKey, "\")
            For n = 1 To pathParts.Length
                SelectKey(String.Join("\", pathParts.Take(n)))
            Next
        Else
            LsvPol_SelectedIndexChanged(Nothing, Nothing) ' Make sure the buttons don't stay enabled
        End If
    End Sub
    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        Dim info As PolValueInfo = LsvPol.SelectedItems(0).Tag
        Dim newData = PromptForNewValueData(info.Name, info.Data, info.Kind)
        If newData IsNot Nothing Then
            EditingPol.SetValue(info.Key, info.Name, newData, info.Kind)
            UpdateTree()
            SelectValue(info.Key, info.Name)
        End If
    End Sub
    Private Sub LsvPol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LsvPol.SelectedIndexChanged
        ' Update the enabled status of all the buttons
        If LsvPol.SelectedItems.Count = 0 Then
            ButtonAddKey.Enabled = True
            ButtonAddValue.Enabled = False
            ButtonDeleteValue.Enabled = False
            ButtonEdit.Enabled = False
            ButtonForget.Enabled = False
            ButtonExport.Enabled = True
        Else
            Dim tag = LsvPol.SelectedItems(0).Tag
            ButtonForget.Enabled = True
            If TypeOf tag Is String Then ' It's a key
                ButtonAddKey.Enabled = True
                ButtonAddValue.Enabled = True
                ButtonEdit.Enabled = False
                ButtonDeleteValue.Enabled = True
                ButtonExport.Enabled = True
            Else ' It's a value
                ButtonAddKey.Enabled = False
                ButtonAddValue.Enabled = False
                Dim delete = CType(tag, PolValueInfo).IsDeleter
                ButtonEdit.Enabled = Not delete
                ButtonDeleteValue.Enabled = Not delete
                ButtonExport.Enabled = False
            End If
        End If
    End Sub
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        If ImportReg.PresentDialog(EditingPol) = DialogResult.OK Then UpdateTree()
    End Sub
    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        Dim branch As String = ""
        If LsvPol.SelectedItems.Count > 0 Then branch = LsvPol.SelectedItems(0).Tag
        ExportReg.PresentDialog(branch, EditingPol, EditingUserSource)
    End Sub
    Private Class PolValueInfo
        Public Key As String
        Public Name As String
        Public Kind As RegistryValueKind
        Public Data As Object
        Public IsDeleter As Boolean
    End Class
End Class