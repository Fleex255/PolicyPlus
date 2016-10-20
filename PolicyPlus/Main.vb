Imports System.ComponentModel
Public Class Main
    Dim AdmxWorkspace As New AdmxBundle
    Dim UserPolicySource, CompPolicySource As IPolicySource
    Dim UserPolicyLoader, CompPolicyLoader As PolicyLoader
    Dim UserComments, CompComments As Dictionary(Of String, String)
    Dim CurrentCategory As PolicyPlusCategory
    Dim CurrentSetting As PolicyPlusPolicy
    Dim CurrentFilter As New FilterConfiguration
    Dim HighlightCategory As PolicyPlusCategory
    Dim CategoryNodes As New Dictionary(Of PolicyPlusCategory, TreeNode)
    Dim ViewEmptyCategories As Boolean = False
    Dim ViewPolicyTypes As AdmxPolicySection = AdmxPolicySection.Both
    Dim ViewFilteredOnly As Boolean = False
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AdmxWorkspace.LoadFolder(Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions"), Globalization.CultureInfo.CurrentCulture.Name)
        Catch ex As Exception
            MsgBox("Policy definitions could not be loaded from the default folder. " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
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
                listItem.SubItems.Add(GetPolicyCommentText(policy))
            Next
            If CategoriesTree.SelectedNode Is Nothing OrElse CategoriesTree.SelectedNode.Tag IsNot CurrentCategory Then ' Update the tree view
                CategoriesTree.SelectedNode = CategoryNodes(CurrentCategory)
            End If
        End If
    End Sub
    Sub UpdatePolicyInfo()
        Dim hasCurrentSetting = (CurrentSetting IsNot Nothing) Or (HighlightCategory IsNot Nothing) Or (CurrentCategory IsNot Nothing)
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
        ElseIf HighlightCategory IsNot Nothing Or CurrentCategory IsNot Nothing Then
            Dim shownCategory = If(HighlightCategory, CurrentCategory)
            PolicyTitleLabel.Text = shownCategory.DisplayName
            PolicySupportedLabel.Text = If(HighlightCategory Is Nothing, "This", "The selected") & " category contains " & shownCategory.Policies.Count & " policies and " & shownCategory.Children.Count & " subcategories."
            PolicyDescLabel.Text = shownCategory.DisplayExplanation.Trim()
            PolicyIsPrefLabel.Visible = False
        Else
            PolicyDescLabel.Text = "Select an item to see its description."
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
        If Not PolicyVisibleInSection(Policy, ViewPolicyTypes) Then Return False
        If ViewFilteredOnly Then
            Dim visibleAfterFilter As Boolean = False
            If (ViewPolicyTypes And AdmxPolicySection.Machine) > 0 And PolicyVisibleInSection(Policy, AdmxPolicySection.Machine) Then
                If IsPolicyVisibleAfterFilter(Policy, False) Then visibleAfterFilter = True
            ElseIf (ViewPolicyTypes And AdmxPolicySection.User) > 0 And PolicyVisibleInSection(Policy, AdmxPolicySection.User) Then
                If IsPolicyVisibleAfterFilter(Policy, True) Then visibleAfterFilter = True
            End If
            If Not visibleAfterFilter Then Return False
        End If
        Return True
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
        Select Case PolicyProcessing.GetPolicyState(If(Section = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource), Policy)
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
    Function GetPolicyCommentText(Policy As PolicyPlusPolicy) As String
        If ViewPolicyTypes = AdmxPolicySection.Both Then
            Dim userComment = GetPolicyComment(Policy, AdmxPolicySection.User)
            Dim compComment = GetPolicyComment(Policy, AdmxPolicySection.Machine)
            If userComment = "" And compComment = "" Then
                Return ""
            ElseIf userComment <> "" And compComment <> "" Then
                Return "(multiple)"
            ElseIf userComment <> "" Then
                Return userComment
            Else
                Return compComment
            End If
        Else
            Return GetPolicyComment(Policy, ViewPolicyTypes)
        End If
    End Function
    Function GetPolicyComment(Policy As PolicyPlusPolicy, Section As AdmxPolicySection) As String
        Dim commentSource As Dictionary(Of String, String) = If(Section = AdmxPolicySection.Machine, CompComments, UserComments)
        If commentSource Is Nothing Then
            Return ""
        Else
            If commentSource.ContainsKey(Policy.UniqueID) Then Return commentSource(Policy.UniqueID) Else Return ""
        End If
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
    Sub ShowSearchDialog(Searcher As Func(Of PolicyPlusPolicy, Boolean))
        Dim result As DialogResult
        If Searcher Is Nothing Then
            result = FindResults.PresentDialog()
        Else
            result = FindResults.PresentDialogStartSearch(AdmxWorkspace, Searcher)
        End If
        If result = DialogResult.OK Then
            Dim selPol = FindResults.SelectedPolicy
            ShowSettingEditor(selPol, ViewPolicyTypes)
            FocusPolicy(selPol)
        End If
    End Sub
    Sub ClearAdmxWorkspace()
        ' Clear out all the per-workspace bookkeeping
        AdmxWorkspace = New AdmxBundle
        FindResults.ClearSearch()
    End Sub
    Sub FocusPolicy(Policy As PolicyPlusPolicy)
        ' Try to automatically select a policy in the list view
        If CategoryNodes.ContainsKey(Policy.Category) Then
            CurrentCategory = Policy.Category
            UpdateCategoryListing()
            For Each entry As ListViewItem In PoliciesList.Items
                If entry.Tag Is Policy Then
                    entry.Selected = True
                    entry.Focused = True
                    entry.EnsureVisible()
                    Exit For
                End If
            Next
        End If
    End Sub
    Function IsPolicyVisibleAfterFilter(Policy As PolicyPlusPolicy, IsUser As Boolean) As Boolean
        If CurrentFilter.ManagedPolicy.HasValue Then
            If IsPreference(Policy) = CurrentFilter.ManagedPolicy.Value Then Return False
        End If
        If CurrentFilter.PolicyState.HasValue Then
            If PolicyProcessing.GetPolicyState(If(IsUser, UserPolicySource, CompPolicySource), Policy) <> CurrentFilter.PolicyState.Value Then Return False
        End If
        If CurrentFilter.Commented.HasValue Then
            Dim commentDict = If(IsUser, UserComments, CompComments)
            If (commentDict.ContainsKey(Policy.UniqueID) AndAlso commentDict(Policy.UniqueID) <> "") <> CurrentFilter.Commented.Value Then Return False
        End If
        If CurrentFilter.AllowedProducts IsNot Nothing Then
            If Not PolicyProcessing.IsPolicySupported(Policy, CurrentFilter.AllowedProducts, CurrentFilter.AlwaysMatchAny, CurrentFilter.MatchBlankSupport) Then Return False
        End If
        Return True
    End Function
    Function PolicyVisibleInSection(Policy As PolicyPlusPolicy, Section As AdmxPolicySection) As Boolean
        Return (Policy.RawPolicy.Section And Section) > 0
    End Function
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
            Try
                If OpenAdmxFolder.ClearWorkspace Then ClearAdmxWorkspace()
                AdmxWorkspace.LoadFolder(OpenAdmxFolder.SelectedFolder, Globalization.CultureInfo.CurrentCulture.Name)
            Catch ex As Exception
                MsgBox("The folder could not be fully added to the workspace. " & ex.Message, MsgBoxStyle.Exclamation)
            End Try
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
        ClearAdmxWorkspace()
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
    Private Sub FindByIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByIDToolStripMenuItem.Click
        FindById.AdmxWorkspace = AdmxWorkspace
        If FindById.ShowDialog() = DialogResult.OK Then
            Dim selCat = FindById.SelectedCategory
            Dim selPol = FindById.SelectedPolicy
            Dim selPro = FindById.SelectedProduct
            Dim selSup = FindById.SelectedSupport
            If selCat IsNot Nothing Then
                If CategoryNodes.ContainsKey(selCat) Then
                    CurrentCategory = selCat
                    UpdateCategoryListing()
                Else
                    MsgBox("The category is not currently visible. Change the view settings and try again.", MsgBoxStyle.Exclamation)
                End If
            ElseIf selPol IsNot Nothing Then
                ShowSettingEditor(selPol, Math.Min(ViewPolicyTypes, FindById.SelectedSection))
                FocusPolicy(selPol)
            ElseIf selPro IsNot Nothing Then
                DetailProduct.PresentDialog(selPro)
            ElseIf selSup IsNot Nothing Then
                DetailSupport.PresentDialog(selSup)
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
    Private Sub ByTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByTextToolStripMenuItem.Click
        If FindByText.PresentDialog(UserComments, CompComments) = DialogResult.OK Then
            ShowSearchDialog(FindByText.Searcher)
        End If
    End Sub
    Private Sub SearchResultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchResultsToolStripMenuItem.Click
        ShowSearchDialog(Nothing)
    End Sub
    Private Sub FindNextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindNextToolStripMenuItem.Click
        Do
            Dim nextPol = FindResults.NextPolicy
            If nextPol Is Nothing Then
                MsgBox("There are no more results that match the filter.", MsgBoxStyle.Information)
                Exit Do
            ElseIf ShouldShowPolicy(nextPol) Then
                FocusPolicy(nextPol)
                Exit Do
            End If
        Loop
    End Sub
    Private Sub ByRegistryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByRegistryToolStripMenuItem.Click
        If FindByRegistry.ShowDialog = DialogResult.OK Then ShowSearchDialog(FindByRegistry.Searcher)
    End Sub
    Private Sub SettingInfoPanel_ClientSizeChanged(sender As Object, e As EventArgs) Handles SettingInfoPanel.ClientSizeChanged, SettingInfoPanel.SizeChanged
        SettingInfoPanel.AutoScrollMinSize = SettingInfoPanel.Size
        PolicyTitleLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicySupportedLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyDescLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyIsPrefLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyInfoTable.MaximumSize = New Size(SettingInfoPanel.Width - If(SettingInfoPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0), 0)
        PolicyInfoTable.Width = PolicyInfoTable.MaximumSize.Width
        If PolicyInfoTable.ColumnCount > 0 Then PolicyInfoTable.ColumnStyles(0).Width = PolicyInfoTable.ClientSize.Width ' Only once everything is initialized
        PolicyInfoTable.PerformLayout() ' Force the table to take up its full desired size
        PInvoke.ShowScrollBar(SettingInfoPanel.Handle, 0, False) ' 0 means horizontal
    End Sub
    Private Sub Main_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ClosePolicySources()
    End Sub
    Private Sub PoliciesList_KeyDown(sender As Object, e As KeyEventArgs) Handles PoliciesList.KeyDown
        If e.KeyCode = Keys.Enter And PoliciesList.SelectedItems.Count > 0 Then PoliciesList_DoubleClick(sender, e)
    End Sub
    Private Sub FilterOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FilterOptionsToolStripMenuItem.Click
        If FilterOptions.PresentDialog(CurrentFilter, AdmxWorkspace) = DialogResult.OK Then
            CurrentFilter = FilterOptions.CurrentFilter
            ViewFilteredOnly = True
            OnlyFilteredObjectsToolStripMenuItem.Checked = True
            MoveToVisibleCategoryAndReload()
        End If
    End Sub
    Private Sub OnlyFilteredObjectsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnlyFilteredObjectsToolStripMenuItem.Click
        ViewFilteredOnly = Not ViewFilteredOnly
        OnlyFilteredObjectsToolStripMenuItem.Checked = ViewFilteredOnly
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub ImportSemanticPolicyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportSemanticPolicyToolStripMenuItem.Click
        If ImportSpol.ShowDialog() = DialogResult.OK Then
            Dim spol = ImportSpol.Spol
            Dim fails = spol.ApplyAll(AdmxWorkspace, UserPolicySource, CompPolicySource)
            UpdateCategoryListing()
            If fails = 0 Then
                MsgBox("Semantic Policy successfully applied.", MsgBoxStyle.Information)
            Else
                MsgBox(fails & " out of " & spol.Policies.Count & " could not be applied.", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    Private Sub ImportPOLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportPOLToolStripMenuItem.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "POL files|*.pol"
            If ofd.ShowDialog = DialogResult.OK Then
                Dim pol As PolFile = Nothing
                Try
                    pol = PolFile.Load(ofd.FileName)
                Catch ex As Exception
                    MsgBox("The POL file could not be loaded.", MsgBoxStyle.Exclamation)
                    Exit Sub
                End Try
                If OpenSection.ShowDialog = DialogResult.OK Then
                    Dim section = If(OpenSection.SelectedSection = AdmxPolicySection.User, UserPolicySource, CompPolicySource)
                    pol.Apply(section)
                    UpdateCategoryListing()
                    MsgBox("POL import successful.", MsgBoxStyle.Information)
                End If
            End If
        End Using
    End Sub
    Private Sub ExportPOLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportPOLToolStripMenuItem.Click
        Using sfd As New SaveFileDialog
            sfd.Filter = "POL files|*.pol"
            If sfd.ShowDialog = DialogResult.OK AndAlso OpenSection.ShowDialog = DialogResult.OK Then
                Dim section = If(OpenSection.SelectedSection = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource)
                Try
                    If TypeOf section Is PolFile Then
                        CType(section, PolFile).Save(sfd.FileName)
                    ElseIf TypeOf section Is RegistryPolicyProxy Then
                        Dim regRoot = CType(section, RegistryPolicyProxy).EncapsulatedRegistry
                        Dim pol As New PolFile
                        Dim addSubtree As Action(Of String, Microsoft.Win32.RegistryKey)
                        addSubtree = Sub(PathRoot As String, Key As Microsoft.Win32.RegistryKey)
                                         For Each valName In Key.GetValueNames
                                             Dim valData = Key.GetValue(valName, Nothing, Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames)
                                             pol.SetValue(PathRoot, valName, valData, Key.GetValueKind(valName))
                                         Next
                                         For Each subkeyName In Key.GetSubKeyNames
                                             Using subkey = Key.OpenSubKey(subkeyName, False)
                                                 addSubtree(PathRoot & "\" & subkeyName, subkey)
                                             End Using
                                         Next
                                     End Sub
                        For Each policyPath In RegistryPolicyProxy.PolicyKeys
                            Using policyKey = regRoot.OpenSubKey(policyPath, False)
                                addSubtree(policyPath, policyKey)
                            End Using
                        Next
                        pol.Save(sfd.FileName)
                    End If
                    MsgBox("POL exported successfully.", MsgBoxStyle.Information)
                Catch ex As Exception
                    MsgBox("The POL file could not be saved.", MsgBoxStyle.Exclamation)
                End Try
            End If
        End Using
    End Sub
    Private Sub PolicyObjectContext_Opening(sender As Object, e As CancelEventArgs) Handles PolicyObjectContext.Opening
        Dim showingForCategory As Boolean
        If PolicyObjectContext.SourceControl Is CategoriesTree Then
            showingForCategory = True
            PolicyObjectContext.Tag = CategoriesTree.SelectedNode.Tag
        ElseIf PoliciesList.SelectedItems.Count > 0 Then ' Shown from the main view
            Dim selEntryTag = PoliciesList.SelectedItems(0).Tag
            showingForCategory = (TypeOf selEntryTag Is PolicyPlusCategory)
            PolicyObjectContext.Tag = selEntryTag
        Else
            e.Cancel = True
            Exit Sub
        End If
        For Each item In PolicyObjectContext.Items.OfType(Of ToolStripMenuItem)
            Dim ok As Boolean = True
            If CStr(item.Tag) = "P" And showingForCategory Then ok = False
            If CStr(item.Tag) = "C" And Not showingForCategory Then ok = False
            item.Visible = ok
        Next
    End Sub
    Private Sub PolicyObjectContext_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles PolicyObjectContext.ItemClicked
        Dim polObject = PolicyObjectContext.Tag ' The current policy object is in the Tag field
        If e.ClickedItem Is CmeCatOpen Then
            CurrentCategory = polObject
            UpdateCategoryListing()
        ElseIf e.ClickedItem Is CmePolEdit Then
            ShowSettingEditor(polObject, ViewPolicyTypes)
        ElseIf e.ClickedItem Is CmeAllDetails Then
            If TypeOf polObject Is PolicyPlusCategory Then
                DetailCategory.PresentDialog(polObject)
            Else
                DetailPolicy.PresentDialog(polObject)
            End If
        ElseIf e.ClickedItem Is CmePolInspectElements Then
            InspectPolicyElements.PresentDialog(polObject, PolicyIcons, AdmxWorkspace)
        ElseIf e.ClickedItem Is CmePolSpolFragment Then
            InspectSpolFragment.PresentDialog(polObject, AdmxWorkspace, CompPolicySource, UserPolicySource)
        End If
    End Sub
    Private Sub CategoriesTree_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles CategoriesTree.NodeMouseClick
        ' Right-clicking doesn't actually select the node by default
        If e.Button = MouseButtons.Right Then CategoriesTree.SelectedNode = e.Node
    End Sub
End Class