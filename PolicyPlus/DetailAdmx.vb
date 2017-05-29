Public Class DetailAdmx
    Public Sub PresentDialog(Admx As AdmxFile, Workspace As AdmxBundle)
        TextPath.Text = Admx.SourceFile
        TextNamespace.Text = Admx.AdmxNamespace
        TextSupersededAdm.Text = Admx.SupersededAdm
        Dim fillListview = Sub(Control As ListView, Collection As IEnumerable, IdSelector As Func(Of Object, String), NameSelector As Func(Of Object, String))
                               Control.Items.Clear()
                               For Each item In Collection
                                   Dim lsvi = Control.Items.Add(IdSelector(item))
                                   lsvi.Tag = item
                                   lsvi.SubItems.Add(NameSelector(item))
                               Next
                               Control.Columns(1).Width = Control.ClientRectangle.Width - Control.Columns(0).Width - SystemInformation.VerticalScrollBarWidth
                           End Sub
        fillListview(LsvPolicies, Workspace.Policies.Values.Where(Function(p) p.RawPolicy.DefinedIn Is Admx), Function(p As PolicyPlusPolicy) p.RawPolicy.ID, Function(p As PolicyPlusPolicy) p.DisplayName)
        fillListview(LsvCategories, Workspace.FlatCategories.Values.Where(Function(c) c.RawCategory.DefinedIn Is Admx), Function(c As PolicyPlusCategory) c.RawCategory.ID, Function(c As PolicyPlusCategory) c.DisplayName)
        fillListview(LsvProducts, Workspace.FlatProducts.Values.Where(Function(p) p.RawProduct.DefinedIn Is Admx), Function(p As PolicyPlusProduct) p.RawProduct.ID, Function(p As PolicyPlusProduct) p.DisplayName)
        fillListview(LsvSupportDefinitions, Workspace.SupportDefinitions.Values.Where(Function(s) s.RawSupport.DefinedIn Is Admx), Function(s As PolicyPlusSupport) s.RawSupport.ID, Function(s As PolicyPlusSupport) s.DisplayName)
        ShowDialog()
    End Sub
    Private Sub LsvPolicies_DoubleClick(sender As Object, e As EventArgs) Handles LsvPolicies.DoubleClick
        DetailPolicy.PresentDialog(LsvPolicies.SelectedItems(0).Tag)
    End Sub
    Private Sub LsvCategories_DoubleClick(sender As Object, e As EventArgs) Handles LsvCategories.DoubleClick
        DetailCategory.PresentDialog(LsvCategories.SelectedItems(0).Tag)
    End Sub
    Private Sub LsvProducts_DoubleClick(sender As Object, e As EventArgs) Handles LsvProducts.DoubleClick
        DetailProduct.PresentDialog(LsvProducts.SelectedItems(0).Tag)
    End Sub
    Private Sub LsvSupportDefinitions_DoubleClick(sender As Object, e As EventArgs) Handles LsvSupportDefinitions.DoubleClick
        DetailSupport.PresentDialog(LsvSupportDefinitions.SelectedItems(0).Tag)
    End Sub
End Class