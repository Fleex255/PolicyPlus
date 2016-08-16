Public Class OpenUserRegistry
    Public SelectedFilePath As String
    Private Sub OpenUserRegistry_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SubfoldersListview.Columns(1).Width = SubfoldersListview.ClientSize.Width - SubfoldersListview.Columns(0).Width - SystemInformation.VerticalScrollBarWidth
        SubfoldersListview.Items.Clear()
        For Each folder In IO.Directory.EnumerateDirectories("C:\Users")
            Dim dirInfo As New IO.DirectoryInfo(folder)
            If (dirInfo.Attributes And IO.FileAttributes.ReparsePoint) > 0 Then Continue For
            Dim ntuserPath = folder & "\ntuser.dat"
            Dim access As String = ""
            Try
                Using fNtuser As New IO.FileStream(ntuserPath, IO.FileMode.Open, IO.FileAccess.ReadWrite)
                    access = "Yes"
                End Using
            Catch ex As UnauthorizedAccessException
                access = "No"
            Catch ex As IO.FileNotFoundException
                access = ""
            Catch ex As Exception
                access = "In use"
            End Try
            If access <> "" Then
                Dim lvi = SubfoldersListview.Items.Add(IO.Path.GetFileName(folder))
                lvi.SubItems.Add(access)
            End If
        Next
    End Sub
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        If SubfoldersListview.SelectedItems.Count = 0 Then Exit Sub
        SelectedFilePath = IO.Path.Combine("C:\Users", SubfoldersListview.SelectedItems(0).Text, "ntuser.dat")
        DialogResult = DialogResult.OK
    End Sub
End Class