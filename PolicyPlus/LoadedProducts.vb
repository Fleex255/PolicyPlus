Public Class LoadedProducts
    Public Sub PresentDialog(Workspace As AdmxBundle)
        ' Fill the top-level products list
        LsvTopLevelProducts.SelectedIndices.Clear()
        LsvTopLevelProducts.Items.Clear()
        For Each product In Workspace.Products.Values.OrderBy(Function(p) p.DisplayName)
            Dim lsvi = LsvTopLevelProducts.Items.Add(product.DisplayName)
            lsvi.SubItems.Add(product.Children.Count)
            lsvi.Tag = product
        Next
        ' Clear the other lists
        UpdateMajorList()
        ' Finagle the column widths
        For Each lsv In {LsvTopLevelProducts, LsvMajorVersions, LsvMinorVersions}
            Dim lastColWidths As Integer = 0
            For n = 1 To lsv.Columns.Count - 1
                lastColWidths += lsv.Columns(n).Width
            Next
            lsv.Columns(0).Width = lsv.ClientRectangle.Width - lastColWidths - SystemInformation.VerticalScrollBarWidth
        Next
        ShowDialog()
    End Sub
    Sub UpdateMajorList() Handles LsvTopLevelProducts.SelectedIndexChanged
        ' Show the major versions of the selected top-level product
        LsvMajorVersions.SelectedIndices.Clear()
        LsvMajorVersions.Items.Clear()
        If LsvTopLevelProducts.SelectedItems.Count > 0 Then
            Dim selProduct As PolicyPlusProduct = LsvTopLevelProducts.SelectedItems(0).Tag
            For Each product In selProduct.Children.OrderBy(Function(p) p.RawProduct.Version)
                Dim lsvi = LsvMajorVersions.Items.Add(product.DisplayName)
                lsvi.SubItems.Add(product.RawProduct.Version)
                lsvi.SubItems.Add(product.Children.Count)
                lsvi.Tag = product
            Next
            LabelMajorVersion.Text = "Major versions of """ & selProduct.DisplayName & """"
        Else
            LabelMajorVersion.Text = "Select a product to show its major versions"
        End If
        UpdateMinorList()
    End Sub
    Sub UpdateMinorList() Handles LsvMajorVersions.SelectedIndexChanged
        LsvMinorVersions.SelectedIndices.Clear()
        LsvMinorVersions.Items.Clear()
        ' Show the minor versions of the selected major version
        If LsvMajorVersions.SelectedItems.Count > 0 Then
            Dim selProduct As PolicyPlusProduct = LsvMajorVersions.SelectedItems(0).Tag
            For Each product In selProduct.Children.OrderBy(Function(p) p.RawProduct.Version)
                Dim lsvi = LsvMinorVersions.Items.Add(product.DisplayName)
                lsvi.SubItems.Add(product.RawProduct.Version)
                lsvi.Tag = product
            Next
            LabelMinorVersion.Text = "Minor versions of """ & selProduct.DisplayName & """"
        Else
            LabelMinorVersion.Text = "Select a major version to show its minor versions"
        End If
    End Sub
    Sub OpenProductDetails(sender As Object, e As EventArgs) Handles LsvTopLevelProducts.DoubleClick, LsvMajorVersions.DoubleClick, LsvMinorVersions.DoubleClick
        Dim lsv As ListView = sender
        If lsv.SelectedItems.Count = 0 Then Exit Sub
        Dim product As PolicyPlusProduct = lsv.SelectedItems(0).Tag
        DetailProduct.PresentDialog(product)
    End Sub
    Sub ListKeyPressed(sender As Object, e As KeyEventArgs) Handles LsvTopLevelProducts.KeyDown, LsvMajorVersions.KeyDown, LsvMinorVersions.KeyDown
        If e.KeyCode = Keys.Enter Then
            OpenProductDetails(sender, e)
            e.Handled = True
        End If
    End Sub
End Class