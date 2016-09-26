Public Class FilterOptions
    Public CurrentFilter As FilterConfiguration
    Dim ProductNodes As Dictionary(Of PolicyPlusProduct, TreeNode)
    Public Function PresentDialog(Configuration As FilterConfiguration, Workspace As AdmxBundle) As DialogResult
        CurrentFilter = Nothing
        ProductNodes = New Dictionary(Of PolicyPlusProduct, TreeNode)
        AllowedProductsTreeview.Nodes.Clear()
        Dim addProductsToNodeCollection As Action(Of IEnumerable(Of PolicyPlusProduct), TreeNodeCollection)
        addProductsToNodeCollection = Sub(Products As IEnumerable(Of PolicyPlusProduct), Nodes As TreeNodeCollection)
                                          For Each product In Products
                                              Dim node = Nodes.Add(product.DisplayName)
                                              ProductNodes.Add(product, node)
                                              node.Tag = product
                                              If product.Children IsNot Nothing Then addProductsToNodeCollection(product.Children, node.Nodes)
                                          Next
                                      End Sub
        addProductsToNodeCollection(Workspace.Products.Values, AllowedProductsTreeview.Nodes)
        PrepareDialog(Configuration)
        Return ShowDialog()
    End Function
    Sub PrepareDialog(Configuration As FilterConfiguration)
        If Configuration.ManagedPolicy.HasValue Then
            PolicyTypeCombobox.SelectedIndex = If(Configuration.ManagedPolicy.Value, 1, 2)
        Else
            PolicyTypeCombobox.SelectedIndex = 0
        End If
        If Configuration.PolicyState.HasValue Then
            Select Case Configuration.PolicyState.Value
                Case PolicyState.Enabled
                    PolicyStateCombobox.SelectedIndex = 2
                Case PolicyState.Disabled
                    PolicyStateCombobox.SelectedIndex = 3
                Case Else
                    PolicyStateCombobox.SelectedIndex = 1
            End Select
        Else
            PolicyStateCombobox.SelectedIndex = 0
        End If
        If Configuration.Commented.HasValue Then
            CommentedCombobox.SelectedIndex = If(Configuration.Commented.Value, 1, 2)
        Else
            CommentedCombobox.SelectedIndex = 0
        End If
        For Each node In ProductNodes.Values
            node.Checked = False
        Next
        If Configuration.AllowedProducts Is Nothing Then
            SupportedCheckbox.Checked = False
            AlwaysMatchAnyCheckbox.Checked = True
            MatchBlankSupportCheckbox.Checked = True
        Else
            SupportedCheckbox.Checked = True
            For Each product In Configuration.AllowedProducts
                ProductNodes(product).Checked = True
            Next
            AlwaysMatchAnyCheckbox.Checked = Configuration.AlwaysMatchAny
            MatchBlankSupportCheckbox.Checked = Configuration.MatchBlankSupport
        End If
    End Sub
    Private Sub SupportedCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles SupportedCheckbox.CheckedChanged
        RequirementsBox.Enabled = SupportedCheckbox.Checked
    End Sub
    Private Sub FilterOptions_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        PrepareDialog(New FilterConfiguration)
    End Sub
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        Dim newConf As New FilterConfiguration
        Select Case PolicyTypeCombobox.SelectedIndex
            Case 1
                newConf.ManagedPolicy = True
            Case 2
                newConf.ManagedPolicy = False
        End Select
        Select Case PolicyStateCombobox.SelectedIndex
            Case 1
                newConf.PolicyState = PolicyState.NotConfigured
            Case 2
                newConf.PolicyState = PolicyState.Enabled
            Case 3
                newConf.PolicyState = PolicyState.Disabled
        End Select
        Select Case CommentedCombobox.SelectedIndex
            Case 1
                newConf.Commented = True
            Case 2
                newConf.Commented = False
        End Select
        If SupportedCheckbox.Checked Then
            newConf.AlwaysMatchAny = AlwaysMatchAnyCheckbox.Checked
            newConf.MatchBlankSupport = MatchBlankSupportCheckbox.Checked
            newConf.AllowedProducts = New List(Of PolicyPlusProduct)
            For Each kv In ProductNodes
                If kv.Value.Checked Then newConf.AllowedProducts.Add(kv.Key)
            Next
        End If
        CurrentFilter = newConf
        DialogResult = DialogResult.OK
    End Sub
    Private Sub AllowedProductsTreeview_EnabledChanged(sender As Object, e As EventArgs) Handles AllowedProductsTreeview.EnabledChanged
        ' Without this, the tree view looks really bad when disabled
        AllowedProductsTreeview.BackColor = If(AllowedProductsTreeview.Enabled, SystemColors.Window, SystemColors.Control)
    End Sub
    Private Sub AllowedProductsTreeview_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles AllowedProductsTreeview.AfterCheck
        If e.Node.Checked Then
            For Each subnode As TreeNode In e.Node.Nodes
                subnode.Checked = True
            Next
        End If
    End Sub
End Class
Public Class FilterConfiguration
    Public ManagedPolicy As Boolean?
    Public PolicyState As PolicyState?
    Public Commented As Boolean?
    Public AllowedProducts As List(Of PolicyPlusProduct)
    Public AlwaysMatchAny As Boolean
    Public MatchBlankSupport As Boolean
End Class