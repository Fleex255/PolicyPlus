Public Class Main
    Dim AdmxWorkspace As New AdmxBundle
    Dim CurrentCategory As PolicyPlusCategory
    Dim CurrentSetting As PolicyPlusPolicy
    Dim CategoryNodes As New Dictionary(Of PolicyPlusCategory, TreeNode)
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdmxWorkspace.LoadFolder(Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions"), Globalization.CultureInfo.CurrentCulture.Name)
        PopulateAdmxUi()
    End Sub
    Sub PopulateAdmxUi()
        ' Populate the left categories tree
        CategoriesTree.Nodes.Clear()
        CategoryNodes.Clear()
        Dim addCategory As Action(Of IEnumerable(Of PolicyPlusCategory), TreeNodeCollection)
        addCategory = Sub(CategoryList, ParentNode)
                          For Each category In CategoryList
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
            For Each category In CurrentCategory.Children.OrderBy(Function(c) c.DisplayName) ' Add subcategories
                Dim listItem = PoliciesList.Items.Add(category.DisplayName)
                listItem.Tag = category
                listItem.ImageIndex = GetImageIndexForCategory(category)
            Next
            For Each policy In CurrentCategory.Policies.OrderBy(Function(p) p.DisplayName) ' Add policies
                Dim listItem = PoliciesList.Items.Add(policy.DisplayName)
                listItem.Tag = policy
                listItem.ImageIndex = GetImageIndexForSetting(policy)
            Next
            If CategoriesTree.SelectedNode.Tag IsNot CurrentCategory Then ' Update the tree view
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
        Else
            PolicyDescLabel.Text = "Select a setting on the right to see its description."
        End If
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
        If Setting.RawPolicy.Elements Is Nothing OrElse Setting.RawPolicy.Elements.Count = 0 Then
            Return 4 ' Normal
        Else
            Return 5 ' Extra configuration
        End If
    End Function
    Private Sub CategoriesTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles CategoriesTree.AfterSelect
        CurrentCategory = e.Node.Tag
        UpdateCategoryListing()
        CurrentSetting = Nothing
        UpdatePolicyInfo()
    End Sub
    Private Sub ResizePolicyNameColumn(sender As Object, e As EventArgs) Handles Me.SizeChanged, PoliciesList.SizeChanged
        PoliciesList.Columns(0).Width = PoliciesList.Width - (PoliciesList.Columns(1).Width + PoliciesList.Columns(2).Width)
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
    Private Sub PoliciesList_DoubleClick(sender As Object, e As EventArgs) Handles PoliciesList.DoubleClick
        If PoliciesList.SelectedItems.Count = 0 Then Exit Sub
        Dim policyItem = PoliciesList.SelectedItems(0).Tag
        If TypeOf policyItem Is PolicyPlusCategory Then
            CurrentCategory = policyItem
            UpdateCategoryListing()
        End If
    End Sub
End Class