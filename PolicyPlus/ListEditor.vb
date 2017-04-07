Public Class ListEditor
    Dim UserProvidesNames As Boolean
    Public FinalData As Object
    Public Function PresentDialog(Title As String, Data As Object, TwoColumn As Boolean) As DialogResult
        UserProvidesNames = TwoColumn
        NameColumn.Visible = TwoColumn
        ElementNameLabel.Text = Title
        EntriesDatagrid.Rows.Clear()
        If Data IsNot Nothing Then
            If TwoColumn Then
                Dim dict As Dictionary(Of String, String) = Data
                For Each kv In dict
                    EntriesDatagrid.Rows.Add(kv.Key, kv.Value)
                Next
            Else
                Dim list As List(Of String) = Data
                For Each entry In list
                    EntriesDatagrid.Rows.Add("", entry)
                Next
            End If
        End If
        FinalData = Nothing
        Return ShowDialog()
    End Function
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        If UserProvidesNames Then
            ' Check for duplicate keys
            Dim dict As New Dictionary(Of String, String)
            For Each row As DataGridViewRow In EntriesDatagrid.Rows
                If row.IsNewRow Then Continue For
                Dim key As String = row.Cells(0).Value
                If dict.ContainsKey(key) Then
                    MsgBox("Multiple entries are named """ & key & """! Remove or rename all but one.", MsgBoxStyle.Exclamation)
                    Exit Sub
                Else
                    dict.Add(key, row.Cells(1).Value)
                End If
            Next
            FinalData = dict
        Else
            Dim list As New List(Of String)
            For Each row As DataGridViewRow In EntriesDatagrid.Rows
                If Not row.IsNewRow Then list.Add(row.Cells(1).Value)
            Next
            FinalData = list
        End If
        DialogResult = DialogResult.OK
    End Sub
End Class