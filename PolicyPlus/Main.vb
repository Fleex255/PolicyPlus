Public Class Main
    Dim AdmxWorkspace As New AdmxBundle
    Dim UserPolicySource, CompPolicySource As IPolicySource
    Dim UserPolicyLoader, CompPolicyLoader As PolicyLoader
    Dim UserComments, CompComments As Dictionary(Of String, String)
    Dim CurrentCategory As PolicyPlusCategory
    Dim CurrentSetting As PolicyPlusPolicy
    Dim HighlightCategory As PolicyPlusCategory
    Dim CategoryNodes As New Dictionary(Of PolicyPlusCategory, TreeNode)
    Dim ViewEmptyCategories As Boolean = False
    Dim ViewPolicyTypes As AdmxPolicySection = AdmxPolicySection.Both
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdmxWorkspace.LoadFolder(Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions"), Globalization.CultureInfo.CurrentCulture.Name)
        OpenPolicyLoaders(New PolicyLoader(PolicyLoaderSource.LocalGpo, "", True), New PolicyLoader(PolicyLoaderSource.LocalGpo, "", False), True)
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
        ClearSelections()
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
        Dim hasCurrentSetting = (CurrentSetting IsNot Nothing) Or (HighlightCategory IsNot Nothing)
        PolicyTitleLabel.Visible = hasCurrentSetting
        PolicySupportedLabel.Visible = hasCurrentSetting
        If CurrentSetting IsNot Nothing Then
            PolicyTitleLabel.Text = CurrentSetting.DisplayName
            If CurrentSetting.SupportedOn Is Nothing Then
                PolicySupportedLabel.Text = "(no requirements information)"
            Else
                PolicySupportedLabel.Text = "Requirements:" & vbCrLf & CurrentSetting.SupportedOn.DisplayName
            End If
            PolicyDescLabel.Text = CurrentSetting.DisplayExplanation.Trim()
            PolicyIsPrefLabel.Visible = IsPreference(CurrentSetting)
        ElseIf HighlightCategory IsNot Nothing Then
            PolicyTitleLabel.Text = HighlightCategory.DisplayName
            PolicySupportedLabel.Text = "This category contains " & HighlightCategory.Policies.Count & " policies and " & HighlightCategory.Children.Count & " subcategories."
            PolicyDescLabel.Text = HighlightCategory.DisplayExplanation.Trim()
            PolicyIsPrefLabel.Visible = False
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
        If EditSetting.PresentDialog(Policy, Section, AdmxWorkspace, CompPolicySource, UserPolicySource, CompPolicyLoader, UserPolicyLoader, CompComments, UserComments) = DialogResult.OK Then UpdateCategoryListing()
    End Sub
    Sub ClearSelections()
        CurrentSetting = Nothing
        HighlightCategory = Nothing
    End Sub
    Sub OpenPolicyLoaders(User As PolicyLoader, Computer As PolicyLoader, Quiet As Boolean)
        If CompPolicyLoader IsNot Nothing Or UserPolicyLoader IsNot Nothing Then ClosePolicySources()
        UserPolicyLoader = User
        UserPolicySource = User.OpenSource
        CompPolicyLoader = Computer
        CompPolicySource = Computer.OpenSource
        Dim allOk As Boolean = True
        Dim policyStatus = Function(Loader As PolicyLoader) As String
                               Select Case Loader.GetWritability
                                   Case PolicySourceWritability.Writable
                                       Return "is fully writable"
                                   Case PolicySourceWritability.NoCommit
                                       allOk = False
                                       Return "cannot be saved"
                                   Case Else ' No writing
                                       allOk = False
                                       Return "cannot be modified"
                               End Select
                           End Function
        Dim loadComments = Function(Loader As PolicyLoader) As Dictionary(Of String, String)
                               Dim cmtxPath = Loader.GetCmtxPath
                               If cmtxPath = "" Then
                                   Return Nothing
                               Else
                                   Try
                                       IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(cmtxPath))
                                       If IO.File.Exists(cmtxPath) Then
                                           Return CmtxFile.Load(cmtxPath).ToCommentTable
                                       Else
                                           Return New Dictionary(Of String, String)
                                       End If
                                   Catch ex As Exception
                                       Return Nothing
                                   End Try
                               End If
                           End Function
        Dim userStatus = policyStatus(User)
        Dim compStatus = policyStatus(Computer)
        UserComments = loadComments(User)
        CompComments = loadComments(Computer)
        If allOk Then
            If Not Quiet Then
                MsgBox("Both the user and computer policy sources are loaded and writable.", MsgBoxStyle.Information)
            End If
        Else
            Dim msgText = "Not all policy sources are fully writable." & vbCrLf & vbCrLf &
                "The user source " & userStatus & "." & vbCrLf & vbCrLf & "The computer source " & compStatus & "."
            MsgBox(msgText, MsgBoxStyle.Exclamation)
        End If
    End Sub
    Sub ClosePolicySources()
        Dim allOk As Boolean = True
        If UserPolicyLoader IsNot Nothing Then
            If Not UserPolicyLoader.Close() Then allOk = False
        End If
        If CompPolicyLoader IsNot Nothing Then
            If Not CompPolicyLoader.Close() Then allOk = False
        End If
        If Not allOk Then
            MsgBox("Cleanup did not complete fully because the loaded resources are open in other programs.", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub CategoriesTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles CategoriesTree.AfterSelect
        CurrentCategory = e.Node.Tag
        UpdateCategoryListing()
        ClearSelections()
        UpdatePolicyInfo()
    End Sub
    Private Sub ResizePolicyNameColumn(sender As Object, e As EventArgs) Handles Me.SizeChanged, PoliciesList.SizeChanged
        If IsHandleCreated Then BeginInvoke(Sub() PoliciesList.Columns(0).Width = PoliciesList.ClientSize.Width - (PoliciesList.Columns(1).Width + PoliciesList.Columns(2).Width))
    End Sub
    Private Sub PoliciesList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PoliciesList.SelectedIndexChanged
        If PoliciesList.SelectedItems.Count > 0 Then
            Dim selObject = PoliciesList.SelectedItems(0).Tag
            If TypeOf selObject Is PolicyPlusPolicy Then
                CurrentSetting = selObject
                HighlightCategory = Nothing
            ElseIf TypeOf selObject Is PolicyPlusCategory Then
                HighlightCategory = selObject
                CurrentSetting = Nothing
            End If
        Else
            ClearSelections()
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
        ClearSelections()
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
    Private Sub OpenPolicyResourcesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenPolicyResourcesToolStripMenuItem.Click
        If OpenPol.ShowDialog = DialogResult.OK Then
            OpenPolicyLoaders(OpenPol.SelectedUser, OpenPol.SelectedComputer, False)
            UpdateCategoryListing()
        End If
    End Sub
    Private Sub SavePoliciesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavePoliciesToolStripMenuItem.Click
        Dim saveComments = Sub(Comments As Dictionary(Of String, String), Loader As PolicyLoader)
                               Try
                                   If Comments IsNot Nothing Then CmtxFile.FromCommentTable(Comments).Save(Loader.GetCmtxPath)
                               Catch ex As Exception
                                   ' Doesn't matter, it's just comments
                               End Try
                           End Sub
        saveComments(UserComments, UserPolicyLoader)
        saveComments(CompComments, CompPolicyLoader)
        Try
            Dim compStatus = "not writable"
            Dim userStatus = "not writable"
            If CompPolicyLoader.GetWritability = PolicySourceWritability.Writable Then compStatus = CompPolicyLoader.Save
            If UserPolicyLoader.GetWritability = PolicySourceWritability.Writable Then userStatus = UserPolicyLoader.Save
            MsgBox("Success." & vbCrLf & vbCrLf & "User policies: " & userStatus & "." & vbCrLf & vbCrLf & "Computer policies: " & compStatus & ".", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("Saving failed!" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Policy Plus by Ben Nordick. Available on GitHub: Fleex255/PolicyPlus. Still in early development (no version number).", MsgBoxStyle.Information)
    End Sub
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
        PInvoke.ShowScrollBar(SettingInfoPanel.Handle, 0, False) ' 0 means horizontal
    End Sub
    Private Sub Main_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ClosePolicySources()
    End Sub
End Class