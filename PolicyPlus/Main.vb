Public Class Main
    Dim AdmxWorkspace As New AdmxBundle
    Dim UserPolicySource, CompPolicySource As IPolicySource
    Dim CurrentCategory As PolicyPlusCategory
    Dim CurrentSetting As PolicyPlusPolicy
    Dim CategoryNodes As New Dictionary(Of PolicyPlusCategory, TreeNode)
    Dim ViewEmptyCategories As Boolean = False
    Dim ViewPolicyTypes As AdmxPolicySection = AdmxPolicySection.Both
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdmxWorkspace.LoadFolder(Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions"), Globalization.CultureInfo.CurrentCulture.Name)
        UserPolicySource = PolFile.Load(Environment.ExpandEnvironmentVariables("%windir%\System32\GroupPolicy\User\Registry.pol"))
        CompPolicySource = PolFile.Load(Environment.ExpandEnvironmentVariables("%windir%\System32\GroupPolicy\Machine\Registry.pol"))
        ComboAppliesTo.Text = ComboAppliesTo.Items(0)
        PopulateAdmxUi()
    End Sub
    Sub PopulateAdmxUi()
        ' Populate the left categories tree
        CategoriesTree.Nodes.Clear()
        CategoryNodes.Clear()
        Dim addCategory As Action(Of IEnumerable(Of PolicyPlusCategory), TreeNodeCollection)
        addCategory = Sub(CategoryList, ParentNode)
                          For Each category In CategoryList.Where(AddressOf ShouldShowCategory)
                              Dim newNode = ParentNode.Add(category.UniqueID, category.DisplayName, GetImageIndexForCategory(category))
                              newNode.SelectedImageIndex = 3 ' "Go" arrow
                              newNode.Tag = category
                              CategoryNodes.Add(category, newNode)
                              addCategory(category.Children, newNode.Nodes)
                          Next
                      End Sub
        addCategory(AdmxWorkspace.Categories.Values, CategoriesTree.Nodes)
        CategoriesTree.Sort()
        CurrentCategory = Nothing
        UpdateCategoryListing()
        CurrentSetting = Nothing
        UpdatePolicyInfo()
    End Sub
    Sub UpdateCategoryListing()
        PoliciesList.Items.Clear()
        If CurrentCategory IsNot Nothing Then
            If CurrentCategory.Parent IsNot Nothing Then ' Add the parent
                Dim listItem = PoliciesList.Items.Add("Up: " & CurrentCategory.Parent.DisplayName)
                listItem.Tag = CurrentCategory.Parent
                listItem.ImageIndex = 6 ' Up arrow
                listItem.SubItems.Add("Parent")
            End If
            For Each category In CurrentCategory.Children.Where(AddressOf ShouldShowCategory).OrderBy(Function(c) c.DisplayName) ' Add subcategories
                Dim listItem = PoliciesList.Items.Add(category.DisplayName)
                listItem.Tag = category
                listItem.ImageIndex = GetImageIndexForCategory(category)
            Next
            For Each policy In CurrentCategory.Policies.Where(AddressOf ShouldShowPolicy).OrderBy(Function(p) p.DisplayName) ' Add policies
                Dim listItem = PoliciesList.Items.Add(policy.DisplayName)
                listItem.Tag = policy
                listItem.ImageIndex = GetImageIndexForSetting(policy)
                listItem.SubItems.Add(GetPolicyState(policy))
            Next
            If CategoriesTree.SelectedNode Is Nothing OrElse CategoriesTree.SelectedNode.Tag IsNot CurrentCategory Then ' Update the tree view
                CategoriesTree.SelectedNode = CategoryNodes(CurrentCategory)
            End If
        End If
    End Sub
    Sub UpdatePolicyInfo()
        Dim hasCurrentSetting = (CurrentSetting IsNot Nothing)
        PolicyTitleLabel.Visible = hasCurrentSetting
        PolicySupportedLabel.Visible = hasCurrentSetting
        If hasCurrentSetting Then
            PolicyTitleLabel.Text = CurrentSetting.DisplayName
            If CurrentSetting.SupportedOn Is Nothing Then
                PolicySupportedLabel.Text = ""
            Else
                PolicySupportedLabel.Text = "Requirements:" & vbCrLf & CurrentSetting.SupportedOn.DisplayName
            End If
            PolicyDescLabel.Text = CurrentSetting.DisplayExplanation.TrimStart(" "c).TrimStart(vbTab(0)).TrimStart(vbCrLf)
            PolicyIsPrefLabel.Visible = IsPreference(CurrentSetting)
        Else
            PolicyDescLabel.Text = "Select a setting on the right to see its description."
            PolicyIsPrefLabel.Visible = False
        End If
        SettingInfoPanel_ClientSizeChanged(Nothing, Nothing)
    End Sub
    Function GetImageIndexForCategory(Category As PolicyPlusCategory) As Integer
        If Category.Parent Is Nothing And Category.RawCategory.ParentID <> "" Then
            Return 1 ' Orphaned
        ElseIf Category.Children.Count = 0 And Category.Policies.Count = 0 Then
            Return 2 ' Empty
        Else
            Return 0 ' Normal
        End If
    End Function
    Function GetImageIndexForSetting(Setting As PolicyPlusPolicy) As Integer
        If IsPreference(Setting) Then
            Return 7 ' Preference, not policy (exclamation mark)
        ElseIf Setting.RawPolicy.Elements Is Nothing OrElse Setting.RawPolicy.Elements.Count = 0 Then
            Return 4 ' Normal
        Else
            Return 5 ' Extra configuration
        End If
    End Function
    Function ShouldShowCategory(Category As PolicyPlusCategory) As Boolean
        If ViewEmptyCategories Then
            Return True
        Else
            Return Category.Policies.Any(AddressOf ShouldShowPolicy) OrElse Category.Children.Any(AddressOf ShouldShowCategory)
        End If
    End Function
    Function ShouldShowPolicy(Policy As PolicyPlusPolicy) As Boolean
        Return (ViewPolicyTypes And Policy.RawPolicy.Section) > 0
    End Function
    Sub MoveToVisibleCategoryAndReload()
        Dim newFocusCategory = CurrentCategory
        Dim newFocusPolicy = CurrentSetting
        Do Until newFocusCategory Is Nothing OrElse ShouldShowCategory(newFocusCategory)
            newFocusCategory = newFocusCategory.Parent
            newFocusPolicy = Nothing
        Loop
        If newFocusPolicy IsNot Nothing AndAlso Not ShouldShowPolicy(newFocusPolicy) Then newFocusPolicy = Nothing
        PopulateAdmxUi()
        CurrentCategory = newFocusCategory
        UpdateCategoryListing()
        CurrentSetting = newFocusPolicy
        UpdatePolicyInfo()
    End Sub
    Function GetPolicyState(Policy As PolicyPlusPolicy) As String
        If ViewPolicyTypes = AdmxPolicySection.Both Then
            Dim userState = GetPolicyState(Policy, AdmxPolicySection.User)
            Dim machState = GetPolicyState(Policy, AdmxPolicySection.Machine)
            Dim section = Policy.RawPolicy.Section
            If section = AdmxPolicySection.Both Then
                If userState = machState Then
                    Return userState & " (2)"
                ElseIf userState = "Not Configured" Then
                    Return machState & " (C)"
                ElseIf machState = "Not Configured" Then
                    Return userState & " (U)"
                Else
                    Return "Mixed"
                End If
            Else
                If section = AdmxPolicySection.Machine Then Return machState & " (C)" Else Return userState & " (U)"
            End If
        Else
            Return GetPolicyState(Policy, ViewPolicyTypes)
        End If
    End Function
    Function GetPolicyState(Policy As PolicyPlusPolicy, Section As AdmxPolicySection) As String
        Select Case PolicyProcessing.GetPolicyState(IIf(Section = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource), Policy)
            Case PolicyState.Disabled
                Return "Disabled"
            Case PolicyState.Enabled
                Return "Enabled"
            Case PolicyState.NotConfigured
                Return "Not Configured"
            Case Else
                Return "Unknown"
        End Select
    End Function
    Function IsPreference(Policy As PolicyPlusPolicy) As Boolean
        Return Policy.RawPolicy.RegistryKey <> "" And Not RegistryPolicyProxy.IsPolicyKey(Policy.RawPolicy.RegistryKey)
    End Function
    Sub ShowSettingEditor(Policy As PolicyPlusPolicy, Section As AdmxPolicySection)
        EditSetting.CurrentSetting = Policy
        EditSetting.CurrentSection = Section
        EditSetting.AdmxWorkspace = AdmxWorkspace
        If EditSetting.ShowDialog() = DialogResult.OK Then UpdateCategoryListing()
    End Sub
    Private Sub CategoriesTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles CategoriesTree.AfterSelect
        CurrentCategory = e.Node.Tag
        UpdateCategoryListing()
        CurrentSetting = Nothing
        UpdatePolicyInfo()
    End Sub
    Private Sub ResizePolicyNameColumn(sender As Object, e As EventArgs) Handles Me.SizeChanged, PoliciesList.SizeChanged
        If IsHandleCreated Then BeginInvoke(Sub() PoliciesList.Columns(0).Width = PoliciesList.ClientSize.Width - (PoliciesList.Columns(1).Width + PoliciesList.Columns(2).Width))
    End Sub
    Private Sub PoliciesList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PoliciesList.SelectedIndexChanged
        If PoliciesList.SelectedItems.Count > 0 AndAlso TypeOf PoliciesList.SelectedItems(0).Tag Is PolicyPlusPolicy Then
            CurrentSetting = PoliciesList.SelectedItems(0).Tag
        Else
            CurrentSetting = Nothing
        End If
        UpdatePolicyInfo()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub
    Private Sub OpenADMXFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenADMXFolderToolStripMenuItem.Click
        If OpenAdmxFolder.ShowDialog = DialogResult.OK Then
            AdmxWorkspace.LoadFolder(OpenAdmxFolder.SelectedFolder, Globalization.CultureInfo.CurrentCulture.Name)
            PopulateAdmxUi()
        End If
    End Sub
    Private Sub OpenADMXFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenADMXFileToolStripMenuItem.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Policy definitions files|*.admx"
            ofd.Title = "Open ADMX file"
            If ofd.ShowDialog <> DialogResult.OK Then Exit Sub
            AdmxWorkspace.LoadFile(ofd.FileName, Globalization.CultureInfo.CurrentCulture.Name)
            PopulateAdmxUi()
        End Using
    End Sub
    Private Sub CloseADMXWorkspaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseADMXWorkspaceToolStripMenuItem.Click
        AdmxWorkspace = New AdmxBundle
        PopulateAdmxUi()
    End Sub
    Private Sub EmptyCategoriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmptyCategoriesToolStripMenuItem.Click
        ViewEmptyCategories = Not ViewEmptyCategories
        EmptyCategoriesToolStripMenuItem.Checked = ViewEmptyCategories
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub ComboAppliesTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboAppliesTo.SelectedIndexChanged
        Select Case ComboAppliesTo.Text
            Case "User"
                ViewPolicyTypes = AdmxPolicySection.User
            Case "Computer"
                ViewPolicyTypes = AdmxPolicySection.Machine
            Case Else
                ViewPolicyTypes = AdmxPolicySection.Both
        End Select
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub PoliciesList_DoubleClick(sender As Object, e As EventArgs) Handles PoliciesList.DoubleClick
        If PoliciesList.SelectedItems.Count = 0 Then Exit Sub
        Dim policyItem = PoliciesList.SelectedItems(0).Tag
        If TypeOf policyItem Is PolicyPlusCategory Then
            CurrentCategory = policyItem
            UpdateCategoryListing()
        ElseIf TypeOf policyItem Is PolicyPlusPolicy Then
            ShowSettingEditor(policyItem, ViewPolicyTypes)
        End If
    End Sub
    Private Sub DeduplicatePoliciesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeduplicatePoliciesToolStripMenuItem.Click
        CurrentSetting = Nothing
        Dim deduped = PolicyProcessing.DeduplicatePolicies(AdmxWorkspace)
        MsgBox("Deduplicated " & deduped & " policies.", MsgBoxStyle.Information)
        UpdateCategoryListing()
        UpdatePolicyInfo()
    End Sub
    Private Sub FindByIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindByIDToolStripMenuItem.Click
        FindById.AdmxWorkspace = AdmxWorkspace
        If FindById.ShowDialog() = DialogResult.OK Then
            Dim selCat = FindById.SelectedCategory
            Dim selPol = FindById.SelectedPolicy
            If selCat IsNot Nothing Then
                If CategoryNodes.ContainsKey(selCat) Then
                    CurrentCategory = selCat
                    UpdateCategoryListing()
                Else
                    MsgBox("The category is not currently visible. Change the view settings and try again.", MsgBoxStyle.Exclamation)
                End If
            ElseIf selPol IsNot Nothing Then
                ShowSettingEditor(selPol, Math.Min(ViewPolicyTypes, FindById.SelectedSection))
            Else
                MsgBox("That object could not be found.", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    Declare Function ShowScrollBar Lib "user32.dll" (Handle As IntPtr, Scrollbar As Integer, Show As Boolean) As Boolean
    Private Sub SettingInfoPanel_ClientSizeChanged(sender As Object, e As EventArgs) Handles SettingInfoPanel.ClientSizeChanged, SettingInfoPanel.SizeChanged
        SettingInfoPanel.AutoScrollMinSize = SettingInfoPanel.Size
        PolicyTitleLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicySupportedLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyDescLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyIsPrefLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyInfoTable.MaximumSize = New Size(SettingInfoPanel.Width - IIf(SettingInfoPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0), 0)
        PolicyInfoTable.Width = PolicyInfoTable.MaximumSize.Width
        If PolicyInfoTable.ColumnCount > 0 Then PolicyInfoTable.ColumnStyles(0).Width = PolicyInfoTable.ClientSize.Width ' Only once everything is initialized
        PolicyInfoTable.PerformLayout() ' Force the table to take up its full desired size
        ShowScrollBar(SettingInfoPanel.Handle, 0, False) ' 0 means horizontal
    End Sub
End Class