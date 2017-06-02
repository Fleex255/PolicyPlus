Public Class LoadedAdmx
    Dim Bundle As AdmxBundle
    Public Sub PresentDialog(Workspace As AdmxBundle)
        LsvAdmx.Items.Clear()
        For Each admx In Workspace.Sources.Keys
            Dim lsvi = LsvAdmx.Items.Add(IO.Path.GetFileName(admx.SourceFile))
            lsvi.SubItems.Add(IO.Path.GetDirectoryName(admx.SourceFile))
            lsvi.SubItems.Add(admx.AdmxNamespace)
            lsvi.Tag = admx
        Next
        LoadedAdmx_SizeChanged(Nothing, Nothing)
        ChNamespace.Width -= SystemInformation.VerticalScrollBarWidth ' For some reason, this only needs to be taken into account on the first draw
        Bundle = Workspace
        ShowDialog()
    End Sub
    Private Sub LsvAdmx_DoubleClick(sender As Object, e As EventArgs) Handles LsvAdmx.DoubleClick
        DetailAdmx.PresentDialog(LsvAdmx.SelectedItems(0).Tag, Bundle)
    End Sub
    Private Sub LoadedAdmx_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ChNamespace.Width = Math.Max(30, LsvAdmx.ClientRectangle.Width - ChFolder.Width - ChFileTitle.Width)
    End Sub
    Private Sub LsvAdmx_KeyDown(sender As Object, e As KeyEventArgs) Handles LsvAdmx.KeyDown
        If e.KeyCode = Keys.Enter And LsvAdmx.SelectedItems.Count > 0 Then LsvAdmx_DoubleClick(sender, e)
    End Sub
End Class