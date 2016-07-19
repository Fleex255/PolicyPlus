Public Class Main
    Dim AdmxWorkspace As New AdmxBundle
    Dim CurrentCategory As PolicyPlusCategory
    Dim CurrentSetting As PolicyPlusPolicy
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AdmxWorkspace.LoadFolder(Environment.ExpandEnvironmentVariables("%windir%\PolicyDefinitions"), Globalization.CultureInfo.CurrentCulture.Name)
        PopulateAdmxUi()
    End Sub
    Sub PopulateAdmxUi()
        ' Populate the left categories tree
        CategoriesTree.Nodes.Clear()
        Dim addCategory As Action(Of IEnumerable(Of PolicyPlusCategory), TreeNodeCollection)
        addCategory = Sub(CategoryList, ParentNode)
                          For Each category In CategoryList
                              Dim newNode = ParentNode.Add(category.UniqueID, category.DisplayName, GetImageIndexForCategory(category))
                              newNode.SelectedImageIndex = 3 ' "Go" arrow
                              newNode.Tag = category
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
            For Each category In CurrentCategory.Children.OrderBy(Function(c) c.DisplayName)
                Dim listItem = PoliciesList.Items.Add(category.DisplayName)
                listItem.Tag = category
                listItem.ImageIndex = GetImageIndexForCategory(category)
            Next
            For Each policy In CurrentCategory.Policies.OrderBy(Function(p) p.DisplayName)
                Dim listItem = PoliciesList.Items.Add(policy.DisplayName)
                listItem.Tag = policy
                listItem.ImageIndex = GetImageIndexForSetting(policy)
            Next
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
End Class