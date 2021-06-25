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
        addProductsToNodeCollection(Workspace.Products.Values, AllowedProductsTreeview.Nodes) ' Recursively add products
        PrepareDialog(Configuration)
        Return ShowDialog()
    End Function
    Sub PrepareDialog(Configuration As FilterConfiguration)
        ' Set the UI element state from the current filter
        If Configuration.ManagedPolicy.HasValue Then
            PolicyTypeCombobox.SelectedIndex = If(Configuration.ManagedPolicy.Value, 1, 2)
        Else
            PolicyTypeCombobox.SelectedIndex = 0
        End If
        If Configuration.PolicyState.HasValue Then
            Select Case Configuration.PolicyState.Value
                Case FilterPolicyState.NotConfigured
                    PolicyStateCombobox.SelectedIndex = 1
                Case FilterPolicyState.Configured
                    PolicyStateCombobox.SelectedIndex = 2
                Case FilterPolicyState.Enabled
                    PolicyStateCombobox.SelectedIndex = 3
                Case FilterPolicyState.Disabled
                    PolicyStateCombobox.SelectedIndex = 4
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
            ' Expand to show all products with a different check state than their parent
            Dim expandIfNecessary As Action(Of TreeNode)
            expandIfNecessary = Sub(Node As TreeNode)
                                    For Each subnode As TreeNode In Node.Nodes
                                        expandIfNecessary(subnode)
                                        If subnode.IsExpanded Or subnode.Checked <> Node.Checked Then
                                            Node.Expand()
                                        End If
                                    Next
                                End Sub
            For Each node As TreeNode In AllowedProductsTreeview.Nodes
                expandIfNecessary(node)
            Next
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
        ' Create a filter configuration object from the user's settings
        Dim newConf As New FilterConfiguration
        Select Case PolicyTypeCombobox.SelectedIndex
            Case 1
                newConf.ManagedPolicy = True
            Case 2
                newConf.ManagedPolicy = False
        End Select
        Select Case PolicyStateCombobox.SelectedIndex
            Case 1
                newConf.PolicyState = FilterPolicyState.NotConfigured
            Case 2
                newConf.PolicyState = FilterPolicyState.Configured
            Case 3
                newConf.PolicyState = FilterPolicyState.Enabled
            Case 4
                newConf.PolicyState = FilterPolicyState.Disabled
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
        ' Recursively set the check state on child products only in response to the user
        If e.Action = TreeViewAction.Unknown Then Return
        PropogateCheckStateDown(e.Node)
    End Sub
    Sub PropogateCheckStateDown(Node As TreeNode)
        For Each subnode As TreeNode In Node.Nodes
            subnode.Checked = Node.Checked
            PropogateCheckStateDown(subnode)
        Next
    End Sub
End Class
Public Enum FilterPolicyState
    Configured
    NotConfigured
    Enabled
    Disabled
End Enum
Public Class FilterConfiguration
    Public ManagedPolicy As Boolean?
    Public PolicyState As FilterPolicyState?
    Public Commented As Boolean?
    Public AllowedProducts As List(Of PolicyPlusProduct)
    Public AlwaysMatchAny As Boolean
    Public MatchBlankSupport As Boolean
End Class
' The TreeView control has a bug: the displayed check state gets out of sync with the Checked property when the checkbox is double-clicked
' Fix adapted from https://stackoverflow.com/a/3174824
Friend Class DoubleClickIgnoringTreeView
    Inherits TreeView
    Protected Overrides Sub WndProc(ByRef m As Message)
        ' Ignore WM_LBUTTONDBLCLK
        If m.Msg <> &H203 Then MyBase.WndProc(m)
    End Sub
End Class