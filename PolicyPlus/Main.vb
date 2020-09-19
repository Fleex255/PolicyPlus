Imports System.ComponentModel
Imports System.Text
Imports Microsoft.Win32
Public Class Main
    Dim Configuration As ConfigurationStorage
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
        ' Create the configuration manager (for the Registry)
        Configuration = New ConfigurationStorage(RegistryHive.CurrentUser, "Software\Policy Plus")
        ' Restore the last ADMX source and policy loaders
        OpenLastAdmxSource()
        Dim compLoaderType As PolicyLoaderSource = Configuration.GetValue("CompSourceType", 0)
        Dim compLoaderData = Configuration.GetValue("CompSourceData", "")
        Dim userLoaderType As PolicyLoaderSource = Configuration.GetValue("UserSourceType", 0)
        Dim userLoaderData = Configuration.GetValue("UserSourceData", "")
        Try
            OpenPolicyLoaders(New PolicyLoader(userLoaderType, userLoaderData, True), New PolicyLoader(compLoaderType, compLoaderData, False), True)
        Catch ex As Exception
            MsgBox("The previous policy sources are not accessible. The defaults will be loaded.", MsgBoxStyle.Exclamation)
            Configuration.SetValue("CompSourceType", CInt(PolicyLoaderSource.LocalGpo))
            Configuration.SetValue("UserSourceType", CInt(PolicyLoaderSource.LocalGpo))
            OpenPolicyLoaders(New PolicyLoader(PolicyLoaderSource.LocalGpo, "", True), New PolicyLoader(PolicyLoaderSource.LocalGpo, "", False), True)
        End Try
        OpenPol.SetLastSources(compLoaderType, compLoaderData, userLoaderType, userLoaderData)
        ' Set up the UI
        ComboAppliesTo.Text = ComboAppliesTo.Items(0)
        CategoriesTree.Height -= InfoStrip.ClientSize.Height
        SettingInfoPanel.Height -= InfoStrip.ClientSize.Height
        PoliciesList.Height -= InfoStrip.ClientSize.Height
        InfoStrip.Items.Insert(2, New ToolStripSeparator)
        PopulateAdmxUi()
    End Sub
    Private Sub Main_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Check whether ADMX files probably need to be downloaded
        If CInt(Configuration.GetValue("CheckedPolicyDefinitions", 0)) = 0 Then
            Configuration.SetValue("CheckedPolicyDefinitions", 1)
            If (Not HasGroupPolicyInfrastructure()) AndAlso AdmxWorkspace.Categories.Values.Where(Function(c) IsOrphanCategory(c) And Not IsEmptyCategory(c)).Count() > 2 Then
                If MsgBox($"Welcome to Policy Plus!{vbCrLf}{vbCrLf}Home editions do not come with the full set of policy definitions. Would you like to download them now? " +
                       "This can also be done later with Help | Acquire ADMX Files.", MsgBoxStyle.Information Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    AcquireADMXFilesToolStripMenuItem_Click(Nothing, Nothing)
                End If
            End If
        End If
    End Sub
    Sub OpenLastAdmxSource()
        Dim defaultAdmxSource = Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions")
        Dim admxSource As String = Configuration.GetValue("AdmxSource", defaultAdmxSource)
        Try
            Dim fails = AdmxWorkspace.LoadFolder(admxSource, GetPreferredLanguageCode())
            If DisplayAdmxLoadErrorReport(fails, True) = MsgBoxResult.No Then Throw New Exception("You decided to not use the problematic ADMX bundle.")
        Catch ex As Exception
            AdmxWorkspace = New AdmxBundle
            Dim loadFailReason As String = ""
            If admxSource <> defaultAdmxSource Then
                If MsgBox("Policy definitions could not be loaded from """ & admxSource & """: " & ex.Message & vbCrLf & vbCrLf &
                          "Load from the default location?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                    Try
                        Configuration.SetValue("AdmxSource", defaultAdmxSource)
                        AdmxWorkspace = New AdmxBundle
                        DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(defaultAdmxSource, GetPreferredLanguageCode()))
                    Catch ex2 As Exception
                        loadFailReason = ex2.Message
                    End Try
                End If
            Else
                loadFailReason = ex.Message
            End If
            If loadFailReason <> "" Then MsgBox("Policy definitions could not be loaded: " & loadFailReason, MsgBoxStyle.Exclamation)
        End Try
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
        ' Update the right pane to include the current category's children
        Dim topItemIndex As Integer?
        If PoliciesList.TopItem IsNot Nothing Then topItemIndex = PoliciesList.TopItem.Index
        Dim inSameCategory As Boolean = False
        PoliciesList.Items.Clear()
        If CurrentCategory IsNot Nothing Then
            If CurrentSetting IsNot Nothing AndAlso CurrentSetting.Category Is CurrentCategory Then inSameCategory = True
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
                If policy Is CurrentSetting Then ' Keep the current policy selected
                    listItem.Selected = True
                    listItem.Focused = True
                    listItem.EnsureVisible()
                End If
            Next
            If topItemIndex.HasValue And inSameCategory Then ' Minimize the list view's jumping around when refreshing
                If PoliciesList.Items.Count > topItemIndex.Value Then PoliciesList.TopItem = PoliciesList.Items(topItemIndex.Value)
            End If
            If CategoriesTree.SelectedNode Is Nothing OrElse CategoriesTree.SelectedNode.Tag IsNot CurrentCategory Then ' Update the tree view
                CategoriesTree.SelectedNode = CategoryNodes(CurrentCategory)
            End If
        End If
    End Sub
    Sub UpdatePolicyInfo()
        ' Update the middle pane with the selected object's information
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
            PolicyDescLabel.Text = PrettifyDescription(CurrentSetting.DisplayExplanation)
            PolicyIsPrefTable.Visible = IsPreference(CurrentSetting)
        ElseIf HighlightCategory IsNot Nothing Or CurrentCategory IsNot Nothing Then
            Dim shownCategory = If(HighlightCategory, CurrentCategory)
            PolicyTitleLabel.Text = shownCategory.DisplayName
            PolicySupportedLabel.Text = If(HighlightCategory Is Nothing, "This", "The selected") & " category contains " & shownCategory.Policies.Count & " policies and " & shownCategory.Children.Count & " subcategories."
            PolicyDescLabel.Text = PrettifyDescription(shownCategory.DisplayExplanation)
            PolicyIsPrefTable.Visible = False
        Else
            PolicyDescLabel.Text = "Select an item to see its description."
            PolicyIsPrefTable.Visible = False
        End If
        SettingInfoPanel_ClientSizeChanged(Nothing, Nothing)
    End Sub
    Function IsOrphanCategory(Category As PolicyPlusCategory) As Boolean
        Return Category.Parent Is Nothing And Category.RawCategory.ParentID <> ""
    End Function
    Function IsEmptyCategory(Category As PolicyPlusCategory) As Boolean
        Return Category.Children.Count = 0 And Category.Policies.Count = 0
    End Function
    Function GetImageIndexForCategory(Category As PolicyPlusCategory) As Integer
        If IsOrphanCategory(Category) Then
            Return 1 ' Orphaned
        ElseIf IsEmptyCategory(Category) Then
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
        ' Should this category be shown considering the current filter?
        If ViewEmptyCategories Then
            Return True
        Else ' Only if it has visible children
            Return Category.Policies.Any(AddressOf ShouldShowPolicy) OrElse Category.Children.Any(AddressOf ShouldShowCategory)
        End If
    End Function
    Function ShouldShowPolicy(Policy As PolicyPlusPolicy) As Boolean
        ' Should this policy be shown considering the current filter and active sections?
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
        ' Move up in the categories tree until a visible one is found
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
        ' Get a human-readable string describing the status of a policy, considering all active sections
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
        ' Get the human-readable status of a policy considering only one section
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
        ' Get the comment text to show in the Comment column, considering all active sections
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
        ' Get a policy's comment in one section
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
        ' Show the Edit Policy Setting dialog for a policy and reload if changes were made
        If EditSetting.PresentDialog(Policy, Section, AdmxWorkspace, CompPolicySource, UserPolicySource, CompPolicyLoader, UserPolicyLoader, CompComments, UserComments) = DialogResult.OK Then
            ' Keep the selection where it is if possible
            If CurrentCategory Is Nothing OrElse ShouldShowCategory(CurrentCategory) Then UpdateCategoryListing() Else MoveToVisibleCategoryAndReload()
        End If
    End Sub
    Sub ClearSelections()
        CurrentSetting = Nothing
        HighlightCategory = Nothing
    End Sub
    Sub OpenPolicyLoaders(User As PolicyLoader, Computer As PolicyLoader, Quiet As Boolean)
        ' Create policy sources from the given loaders
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
        UserSourceLabel.Text = UserPolicyLoader.GetDisplayInfo
        ComputerSourceLabel.Text = CompPolicyLoader.GetDisplayInfo
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
        ' Clean up the policy sources
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
        ' Show the search dialog and make it start a search if appropriate
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
        ' Calculate whether a policy is visible with the current filter
        If CurrentFilter.ManagedPolicy.HasValue Then
            If IsPreference(Policy) = CurrentFilter.ManagedPolicy.Value Then Return False
        End If
        If CurrentFilter.PolicyState.HasValue Then
            Dim policyState = PolicyProcessing.GetPolicyState(If(IsUser, UserPolicySource, CompPolicySource), Policy)
            Select Case CurrentFilter.PolicyState.Value
                Case FilterPolicyState.Configured
                    If policyState = PolicyState.NotConfigured Then Return False
                Case FilterPolicyState.NotConfigured
                    If policyState <> PolicyState.NotConfigured Then Return False
                Case FilterPolicyState.Disabled
                    If policyState <> PolicyState.Disabled Then Return False
                Case FilterPolicyState.Enabled
                    If policyState <> PolicyState.Enabled Then Return False
            End Select
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
        ' Does this policy apply to the given section?
        Return (Policy.RawPolicy.Section And Section) > 0
    End Function
    Function GetOrCreatePolFromPolicySource(Source As IPolicySource) As PolFile
        If TypeOf Source Is PolFile Then
            ' If it's already a POL, just save it
            Return Source
        ElseIf TypeOf Source Is RegistryPolicyProxy Then
            ' Recurse through the Registry branch and create a POL
            Dim regRoot = CType(Source, RegistryPolicyProxy).EncapsulatedRegistry
            Dim pol As New PolFile
            Dim addSubtree As Action(Of String, RegistryKey)
            addSubtree = Sub(PathRoot As String, Key As RegistryKey)
                             For Each valName In Key.GetValueNames
                                 Dim valData = Key.GetValue(valName, Nothing, RegistryValueOptions.DoNotExpandEnvironmentNames)
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
            Return pol
        Else
            Throw New InvalidOperationException("Policy source type not supported")
        End If
    End Function
    Function DisplayAdmxLoadErrorReport(Failures As IEnumerable(Of AdmxLoadFailure), Optional AskContinue As Boolean = False) As MsgBoxResult
        If Failures.Count = 0 Then Return MsgBoxResult.Ok
        Dim boxStyle = If(AskContinue, MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, MsgBoxStyle.Exclamation)
        Dim header = "Errors were encountered while adding administrative templates to the workspace."
        Return MsgBox(header & If(AskContinue, " Continue trying to use this workspace?", "") & vbCrLf & vbCrLf &
               String.Join(vbCrLf & vbCrLf, Failures.Select(Function(f) f.ToString)), boxStyle)
    End Function
    Function GetPreferredLanguageCode() As String
        Return Configuration.GetValue("LanguageCode", Globalization.CultureInfo.CurrentCulture.Name)
    End Function
    Private Sub CategoriesTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles CategoriesTree.AfterSelect
        ' When the user selects a new category in the left pane
        CurrentCategory = e.Node.Tag
        UpdateCategoryListing()
        ClearSelections()
        UpdatePolicyInfo()
    End Sub
    Private Sub ResizePolicyNameColumn(sender As Object, e As EventArgs) Handles Me.SizeChanged, PoliciesList.SizeChanged
        ' Fit the policy name column to the window size
        If IsHandleCreated Then BeginInvoke(Sub() PoliciesList.Columns(0).Width = PoliciesList.ClientSize.Width - (PoliciesList.Columns(1).Width + PoliciesList.Columns(2).Width))
    End Sub
    Private Sub PoliciesList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PoliciesList.SelectedIndexChanged
        ' When the user highlights an item in the right pane
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
        ' Show the Open ADMX Folder dialog and load the policy definitions
        If OpenAdmxFolder.ShowDialog = DialogResult.OK Then
            Try
                If OpenAdmxFolder.ClearWorkspace Then ClearAdmxWorkspace()
                DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(OpenAdmxFolder.SelectedFolder, GetPreferredLanguageCode()))
                ' Only update the last source when successfully opening a complete source
                If OpenAdmxFolder.ClearWorkspace Then Configuration.SetValue("AdmxSource", OpenAdmxFolder.SelectedFolder)
            Catch ex As Exception
                MsgBox("The folder could not be fully added to the workspace. " & ex.Message, MsgBoxStyle.Exclamation)
            End Try
            PopulateAdmxUi()
        End If
    End Sub
    Private Sub OpenADMXFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenADMXFileToolStripMenuItem.Click
        ' Open a single ADMX file
        Using ofd As New OpenFileDialog
            ofd.Filter = "Policy definitions files|*.admx"
            ofd.Title = "Open ADMX file"
            If ofd.ShowDialog <> DialogResult.OK Then Exit Sub
            Try
                DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFile(ofd.FileName, GetPreferredLanguageCode()))
            Catch ex As Exception
                MsgBox("The ADMX file could not be added to the workspace. " & ex.Message, MsgBoxStyle.Exclamation)
            End Try
            PopulateAdmxUi()
        End Using
    End Sub
    Private Sub CloseADMXWorkspaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseADMXWorkspaceToolStripMenuItem.Click
        ' Close all policy definitions and clear the workspace
        ClearAdmxWorkspace()
        PopulateAdmxUi()
    End Sub
    Private Sub EmptyCategoriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmptyCategoriesToolStripMenuItem.Click
        ' Toggle whether empty categories are visible
        ViewEmptyCategories = Not ViewEmptyCategories
        EmptyCategoriesToolStripMenuItem.Checked = ViewEmptyCategories
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub ComboAppliesTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboAppliesTo.SelectedIndexChanged
        ' When the user chooses a different section to work with
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
        ' When the user opens a policy object in the right pane
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
        ' Make otherwise-identical pairs of user and computer policies into single dual-section policies
        ClearSelections()
        Dim deduped = PolicyProcessing.DeduplicatePolicies(AdmxWorkspace)
        MsgBox("Deduplicated " & deduped & " policies.", MsgBoxStyle.Information)
        UpdateCategoryListing()
        UpdatePolicyInfo()
    End Sub
    Private Sub FindByIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByIDToolStripMenuItem.Click
        ' Show the Find By ID window and try to move to the selected object
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
        ' Show the Open Policy Resources dialog and open its loaders
        If OpenPol.ShowDialog = DialogResult.OK Then
            OpenPolicyLoaders(OpenPol.SelectedUser, OpenPol.SelectedComputer, False)
            MoveToVisibleCategoryAndReload()
        End If
    End Sub
    Private Sub SavePoliciesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavePoliciesToolStripMenuItem.Click
        ' Save policy state and comments to disk
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
            Configuration.SetValue("CompSourceType", CInt(CompPolicyLoader.Source))
            Configuration.SetValue("UserSourceType", CInt(UserPolicyLoader.Source))
            Configuration.SetValue("CompSourceData", If(CompPolicyLoader.LoaderData, ""))
            Configuration.SetValue("UserSourceData", If(UserPolicyLoader.LoaderData, ""))
            MsgBox("Success." & vbCrLf & vbCrLf & "User policies: " & userStatus & "." & vbCrLf & vbCrLf & "Computer policies: " & compStatus & ".", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("Saving failed!" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        ' Show author and version information if it was compiled into the program
        Dim about = $"Policy Plus by Ben Nordick.{vbCrLf}{vbCrLf}Available on GitHub: Fleex255/PolicyPlus."
        If Version.Trim() <> "" Then about &= $" Version: {Version.Trim()}."
        MsgBox(about, MsgBoxStyle.Information)
    End Sub
    Private Sub ByTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ByTextToolStripMenuItem.Click
        ' Show the Find By Text window and start the search
        If FindByText.PresentDialog(UserComments, CompComments) = DialogResult.OK Then
            ShowSearchDialog(FindByText.Searcher)
        End If
    End Sub
    Private Sub SearchResultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchResultsToolStripMenuItem.Click
        ' Show the search results window but don't start a search
        ShowSearchDialog(Nothing)
    End Sub
    Private Sub FindNextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindNextToolStripMenuItem.Click
        ' Move to the next policy in the search results
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
        ' Show the Find By Registry window and start the search
        If FindByRegistry.ShowDialog = DialogResult.OK Then ShowSearchDialog(FindByRegistry.Searcher)
    End Sub
    Private Sub SettingInfoPanel_ClientSizeChanged(sender As Object, e As EventArgs) Handles SettingInfoPanel.ClientSizeChanged, SettingInfoPanel.SizeChanged
        ' Finagle the middle pane's UI elements
        SettingInfoPanel.AutoScrollMinSize = SettingInfoPanel.Size
        PolicyTitleLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicySupportedLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyDescLabel.MaximumSize = New Size(PolicyInfoTable.Width, 0)
        PolicyIsPrefLabel.MaximumSize = New Size(PolicyInfoTable.Width - 22, 0) ' Leave room for the exclamation icon
        PolicyInfoTable.MaximumSize = New Size(SettingInfoPanel.Width - If(SettingInfoPanel.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0), 0)
        PolicyInfoTable.Width = PolicyInfoTable.MaximumSize.Width
        If PolicyInfoTable.ColumnCount > 0 Then PolicyInfoTable.ColumnStyles(0).Width = PolicyInfoTable.ClientSize.Width ' Only once everything is initialized
        PolicyInfoTable.PerformLayout() ' Force the table to take up its full desired size
        PInvoke.ShowScrollBar(SettingInfoPanel.Handle, 0, False) ' 0 means horizontal
    End Sub
    Private Sub Main_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ClosePolicySources() ' Make sure everything is cleaned up before quitting
    End Sub
    Private Sub PoliciesList_KeyDown(sender As Object, e As KeyEventArgs) Handles PoliciesList.KeyDown
        ' Activate a right pane item if the user presses Enter on it
        If e.KeyCode = Keys.Enter And PoliciesList.SelectedItems.Count > 0 Then PoliciesList_DoubleClick(sender, e)
    End Sub
    Private Sub FilterOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FilterOptionsToolStripMenuItem.Click
        ' Show the Filter Options dialog and refresh if the filter changes
        If FilterOptions.PresentDialog(CurrentFilter, AdmxWorkspace) = DialogResult.OK Then
            CurrentFilter = FilterOptions.CurrentFilter
            ViewFilteredOnly = True
            OnlyFilteredObjectsToolStripMenuItem.Checked = True
            MoveToVisibleCategoryAndReload()
        End If
    End Sub
    Private Sub OnlyFilteredObjectsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OnlyFilteredObjectsToolStripMenuItem.Click
        ' Toggle whether the filter is used
        ViewFilteredOnly = Not ViewFilteredOnly
        OnlyFilteredObjectsToolStripMenuItem.Checked = ViewFilteredOnly
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub ImportSemanticPolicyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportSemanticPolicyToolStripMenuItem.Click
        ' Open the SPOL import dialog and apply the data
        If ImportSpol.ShowDialog() = DialogResult.OK Then
            Dim spol = ImportSpol.Spol
            Dim fails = spol.ApplyAll(AdmxWorkspace, UserPolicySource, CompPolicySource, UserComments, CompComments)
            MoveToVisibleCategoryAndReload()
            If fails = 0 Then
                MsgBox("Semantic Policy successfully applied.", MsgBoxStyle.Information)
            Else
                MsgBox(fails & " out of " & spol.Policies.Count & " could not be applied.", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    Private Sub ImportPOLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportPOLToolStripMenuItem.Click
        ' Open a POL file and write it to a policy source
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
                If OpenSection.PresentDialog(True, True) = DialogResult.OK Then
                    Dim section = If(OpenSection.SelectedSection = AdmxPolicySection.User, UserPolicySource, CompPolicySource)
                    pol.Apply(section)
                    MoveToVisibleCategoryAndReload()
                    MsgBox("POL import successful.", MsgBoxStyle.Information)
                End If
            End If
        End Using
    End Sub
    Private Sub ExportPOLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportPOLToolStripMenuItem.Click
        ' Create a POL file from a current policy source
        Using sfd As New SaveFileDialog
            sfd.Filter = "POL files|*.pol"
            If sfd.ShowDialog = DialogResult.OK AndAlso OpenSection.PresentDialog(True, True) = DialogResult.OK Then
                Dim section = If(OpenSection.SelectedSection = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource)
                Try
                    GetOrCreatePolFromPolicySource(section).Save(sfd.FileName)
                    MsgBox("POL exported successfully.", MsgBoxStyle.Information)
                Catch ex As Exception
                    MsgBox("The POL file could not be saved.", MsgBoxStyle.Exclamation)
                End Try
            End If
        End Using
    End Sub
    Private Sub AcquireADMXFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcquireADMXFilesToolStripMenuItem.Click
        ' Show the Acquire ADMX Files dialog and load the new ADMX files
        If DownloadAdmx.ShowDialog = DialogResult.OK Then
            If DownloadAdmx.NewPolicySourceFolder <> "" Then
                ClearAdmxWorkspace()
                DisplayAdmxLoadErrorReport(AdmxWorkspace.LoadFolder(DownloadAdmx.NewPolicySourceFolder, GetPreferredLanguageCode()))
                Configuration.SetValue("AdmxSource", DownloadAdmx.NewPolicySourceFolder)
                PopulateAdmxUi()
            End If
        End If
    End Sub
    Private Sub LoadedADMXFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadedADMXFilesToolStripMenuItem.Click
        LoadedAdmx.PresentDialog(AdmxWorkspace)
    End Sub
    Private Sub AllSupportDefinitionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllSupportDefinitionsToolStripMenuItem.Click
        LoadedSupportDefinitions.PresentDialog(AdmxWorkspace)
    End Sub
    Private Sub AllProductsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllProductsToolStripMenuItem.Click
        LoadedProducts.PresentDialog(AdmxWorkspace)
    End Sub
    Private Sub EditRawPOLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditRawPOLToolStripMenuItem.Click
        Dim userIsPol = TypeOf UserPolicySource Is PolFile
        Dim compIsPol = TypeOf CompPolicySource Is PolFile
        If Not (userIsPol Or compIsPol) Then
            MsgBox("Neither loaded source is backed by a POL file.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If CInt(Configuration.GetValue("EditPolDangerAcknowledged", 0)) = 0 Then
            If MsgBox("Caution! This tool is for very advanced users. Improper modifications may result in inconsistencies in policies' states." & vbCrLf & vbCrLf &
                      "Changes operate directly on the policy source, though they will not be committed to disk until you save. Are you sure you want to continue?",
                      MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            Configuration.SetValue("EditPolDangerAcknowledged", 1)
        End If
        If OpenSection.PresentDialog(userIsPol, compIsPol) = DialogResult.OK Then
            EditPol.PresentDialog(PolicyIcons, If(OpenSection.SelectedSection = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource),
                                  OpenSection.SelectedSection = AdmxPolicySection.User)
        End If
        MoveToVisibleCategoryAndReload()
    End Sub
    Private Sub ExportREGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportREGToolStripMenuItem.Click
        If OpenSection.PresentDialog(True, True) = DialogResult.OK Then
            Dim source = If(OpenSection.SelectedSection = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource)
            ExportReg.PresentDialog("", GetOrCreatePolFromPolicySource(source), OpenSection.SelectedSection = AdmxPolicySection.User)
        End If
    End Sub
    Private Sub ImportREGToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportREGToolStripMenuItem.Click
        If OpenSection.PresentDialog(True, True) = DialogResult.OK Then
            Dim source = If(OpenSection.SelectedSection = AdmxPolicySection.Machine, CompPolicySource, UserPolicySource)
            If ImportReg.PresentDialog(source) = DialogResult.OK Then MoveToVisibleCategoryAndReload()
        End If
    End Sub
    Private Sub SetADMLLanguageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetADMLLanguageToolStripMenuItem.Click
        If LanguageOptions.PresentDialog(GetPreferredLanguageCode()) = DialogResult.OK Then
            Configuration.SetValue("LanguageCode", LanguageOptions.NewLanguage)
            If MsgBox("Language changes will take effect when ADML files are next loaded. Would you like to reload the workspace now?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                ClearAdmxWorkspace()
                OpenLastAdmxSource()
                PopulateAdmxUi()
            End If
        End If
    End Sub
    Private Sub PolicyObjectContext_Opening(sender As Object, e As CancelEventArgs) Handles PolicyObjectContext.Opening
        ' When the right-click menu is opened
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
        ' Items are tagged in the designer for the objects they apply to
        For Each item In PolicyObjectContext.Items.OfType(Of ToolStripMenuItem)
            Dim ok As Boolean = True
            If CStr(item.Tag) = "P" And showingForCategory Then ok = False
            If CStr(item.Tag) = "C" And Not showingForCategory Then ok = False
            item.Visible = ok
        Next
    End Sub
    Private Sub PolicyObjectContext_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles PolicyObjectContext.ItemClicked
        ' When the user clicks an item in the right-click menu
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
            InspectSpolFragment.PresentDialog(polObject, AdmxWorkspace, CompPolicySource, UserPolicySource, CompComments, UserComments)
        End If
    End Sub
    Private Sub CategoriesTree_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles CategoriesTree.NodeMouseClick
        ' Right-clicking doesn't actually select the node by default
        If e.Button = MouseButtons.Right Then CategoriesTree.SelectedNode = e.Node
    End Sub
    Shared Function PrettifyDescription(Description As String) As String
        ' Remove extra indentation from paragraphs
        Dim sb As New StringBuilder
        For Each line In Description.Split(vbCrLf)
            sb.AppendLine(line.Trim())
        Next
        Return sb.ToString().TrimEnd()
    End Function
End Class