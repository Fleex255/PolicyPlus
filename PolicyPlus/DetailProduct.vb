Public Class DetailProduct
    Dim SelectedProduct As PolicyPlusProduct
    Public Sub PresentDialog(Product As PolicyPlusProduct)
        PrepareDialog(Product)
        ShowDialog()
    End Sub
    Private Sub PrepareDialog(Product As PolicyPlusProduct)
        SelectedProduct = Product
        NameTextbox.Text = Product.DisplayName
        IdTextbox.Text = Product.UniqueID
        DefinedTextbox.Text = Product.RawProduct.DefinedIn.SourceFile
        DisplayCodeTextbox.Text = Product.RawProduct.DisplayCode
        Select Case Product.RawProduct.Type
            Case AdmxProductType.MajorRevision
                KindTextbox.Text = "Major revision"
            Case AdmxProductType.MinorRevision
                KindTextbox.Text = "Minor revision"
            Case AdmxProductType.Product
                KindTextbox.Text = "Top-level product"
        End Select
        VersionTextbox.Text = If(Product.RawProduct.Type = AdmxProductType.Product, "", CStr(Product.RawProduct.Version))
        If Product.Parent Is Nothing Then
            ParentTextbox.Text = ""
            ParentButton.Enabled = False
        Else
            ParentTextbox.Text = Product.Parent.DisplayName
            ParentButton.Enabled = True
        End If
        ChildrenListview.Items.Clear()
        If Product.Children IsNot Nothing Then
            For Each child In Product.Children
                Dim lsvi = ChildrenListview.Items.Add(child.RawProduct.Version)
                lsvi.SubItems.Add(child.DisplayName)
                lsvi.Tag = child
            Next
        End If
    End Sub
    Private Sub ChildrenListview_ClientSizeChanged(sender As Object, e As EventArgs) Handles ChildrenListview.ClientSizeChanged
        ChName.Width = ChildrenListview.ClientSize.Width - ChVersion.Width
    End Sub
    Private Sub ChildrenListview_DoubleClick(sender As Object, e As EventArgs) Handles ChildrenListview.DoubleClick
        If ChildrenListview.SelectedItems.Count = 0 Then Exit Sub
        PrepareDialog(ChildrenListview.SelectedItems(0).Tag)
    End Sub
    Private Sub ParentButton_Click(sender As Object, e As EventArgs) Handles ParentButton.Click
        PrepareDialog(SelectedProduct.Parent)
    End Sub
End Class