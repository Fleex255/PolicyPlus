Public Class LoadedSupportDefinitions
    Dim Definitions As IEnumerable(Of PolicyPlusSupport)
    Public Sub PresentDialog(Workspace As AdmxBundle)
        TextFilter.Text = ""
        Definitions = Workspace.SupportDefinitions.Values
        UpdateListing()
        ChName.Width = LsvSupport.ClientRectangle.Width - ChDefinedIn.Width - SystemInformation.VerticalScrollBarWidth
        ShowDialog()
    End Sub
    Sub UpdateListing()
        ' Add all the (matching) support definitions to the list view
        LsvSupport.Items.Clear()
        For Each support In Definitions.OrderBy(Function(s) s.DisplayName.Trim()) ' Some default support definitions have leading spaces
            If Not support.DisplayName.ToLowerInvariant.Contains(TextFilter.Text.ToLowerInvariant) Then Continue For
            Dim lsvi = LsvSupport.Items.Add(support.DisplayName.Trim())
            lsvi.SubItems.Add(IO.Path.GetFileName(support.RawSupport.DefinedIn.SourceFile))
            lsvi.Tag = support
        Next
    End Sub
    Private Sub LsvSupport_DoubleClick(sender As Object, e As EventArgs) Handles LsvSupport.DoubleClick
        DetailSupport.PresentDialog(LsvSupport.SelectedItems(0).Tag)
    End Sub
    Private Sub TextFilter_TextChanged(sender As Object, e As EventArgs) Handles TextFilter.TextChanged
        ' Only repopulate if the form isn't still setting up
        If Visible Then UpdateListing()
    End Sub
    Private Sub LsvSupport_KeyDown(sender As Object, e As KeyEventArgs) Handles LsvSupport.KeyDown
        If e.KeyCode = Keys.Enter And LsvSupport.SelectedItems.Count > 0 Then LsvSupport_DoubleClick(sender, e)
    End Sub
End Class