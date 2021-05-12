Imports System.ComponentModel
Public Class FindResults
    Dim AdmxWorkspace As AdmxBundle
    Dim SearchFunc As Func(Of PolicyPlusPolicy, Boolean)
    Dim CancelingSearch As Boolean = False
    Dim CancelDueToFormClose As Boolean = False
    Dim SearchPending As Boolean = False
    Dim HasSearched As Boolean
    Dim LastSelectedIndex As Integer
    Public SelectedPolicy As PolicyPlusPolicy
    Public Function PresentDialogStartSearch(Workspace As AdmxBundle, Searcher As Func(Of PolicyPlusPolicy, Boolean)) As DialogResult
        ' Start running a search defined by one of the Find By windows
        AdmxWorkspace = Workspace
        SearchFunc = Searcher
        ResultsListview.Items.Clear()
        SearchProgress.Maximum = Workspace.Policies.Count
        SearchProgress.Value = 0
        StopButton.Enabled = True
        CancelingSearch = False
        CancelDueToFormClose = False
        ProgressLabel.Text = "Starting search"
        SearchPending = True
        HasSearched = True
        LastSelectedIndex = -1
        Return ShowDialog()
    End Function
    Public Function PresentDialog() As DialogResult
        ' Open the dialog normally, like from the main form
        If Not HasSearched Then
            MsgBox("No search has been run yet, so there are no results to display.", MsgBoxStyle.Information)
            Return DialogResult.Cancel
        End If
        CancelingSearch = False
        CancelDueToFormClose = False
        SearchPending = False
        Return ShowDialog()
    End Function
    Public Sub ClearSearch()
        HasSearched = False
        ResultsListview.Items.Clear()
    End Sub
    Public Function NextPolicy() As PolicyPlusPolicy
        If LastSelectedIndex >= ResultsListview.Items.Count - 1 Or Not HasSearched Then Return Nothing
        LastSelectedIndex += 1
        Return ResultsListview.Items(LastSelectedIndex).Tag
    End Function
    Sub SearchJob(Workspace As AdmxBundle, Searcher As Func(Of PolicyPlusPolicy, Boolean))
        ' The long-running task that searches all the policies
        Dim searchedSoFar As Integer = 0
        Dim results As Integer = 0
        Dim stoppedByCancel As Boolean = False
        Dim pendingInsertions As New List(Of PolicyPlusPolicy)
        Dim addPendingInsertions = Sub()
                                       ResultsListview.BeginUpdate()
                                       For Each insert In pendingInsertions
                                           Dim lsvi = ResultsListview.Items.Add(insert.DisplayName)
                                           lsvi.Tag = insert
                                           lsvi.SubItems.Add(insert.Category.DisplayName)
                                       Next
                                       ResultsListview.EndUpdate()
                                       pendingInsertions.Clear()
                                   End Sub
        For Each policy In Workspace.Policies
            If Threading.Thread.VolatileRead(CancelingSearch) Then
                stoppedByCancel = True
                Exit For
            End If
            searchedSoFar += 1
            Dim isHit = Searcher(policy.Value) ' The potentially expensive check
            If isHit Then
                results += 1
                pendingInsertions.Add(policy.Value)
            End If
            If searchedSoFar Mod 20 = 0 Then ' UI updating is costly
                Invoke(Sub()
                           addPendingInsertions()
                           SearchProgress.Value = searchedSoFar
                           ProgressLabel.Text = "Searching: checked " & searchedSoFar & " policies so far, found " & results & " hits"
                       End Sub)
            End If
        Next
        If stoppedByCancel AndAlso CBool(Threading.Thread.VolatileRead(CancelDueToFormClose)) Then Exit Sub ' Avoid accessing a disposed object
        Invoke(Sub()
                   addPendingInsertions()
                   Dim status As String = If(stoppedByCancel, "Search canceled", "Finished searching")
                   ProgressLabel.Text = status & ": checked " & searchedSoFar & " policies, found " & results & " hits"
                   SearchProgress.Value = SearchProgress.Maximum
                   StopButton.Enabled = False
               End Sub)
    End Sub
    Sub StopSearch(Force As Boolean)
        Threading.Thread.VolatileWrite(CancelingSearch, True)
        Threading.Thread.VolatileWrite(CancelDueToFormClose, Force)
    End Sub
    Private Sub FindResults_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If SearchPending Then
            Task.Factory.StartNew(Sub() SearchJob(AdmxWorkspace, SearchFunc))
        ElseIf LastSelectedIndex >= 0 And LastSelectedIndex < ResultsListview.Items.Count Then
            ' Restore the last selection
            Dim lastSelected = ResultsListview.Items(LastSelectedIndex)
            lastSelected.Selected = True
            lastSelected.Focused = True
            lastSelected.EnsureVisible()
        End If
    End Sub
    Private Sub StopButton_Click(sender As Object, e As EventArgs) Handles StopButton.Click
        StopSearch(False)
    End Sub
    Private Sub ResultsListview_SizeChanged(sender As Object, e As EventArgs) Handles ResultsListview.SizeChanged
        ChTitle.Width = ResultsListview.ClientSize.Width - ChCategory.Width
    End Sub
    Private Sub FindResults_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        StopSearch(True)
        If SearchProgress.Value <> SearchProgress.Maximum Then
            ProgressLabel.Text = "Search canceled"
            SearchProgress.Maximum = 100
            SearchProgress.Value = SearchProgress.Maximum
        End If
    End Sub
    Private Sub GoClicked(sender As Object, e As EventArgs) Handles GoButton.Click, ResultsListview.DoubleClick
        If ResultsListview.SelectedItems.Count = 0 Then Exit Sub
        SelectedPolicy = ResultsListview.SelectedItems(0).Tag
        LastSelectedIndex = ResultsListview.SelectedIndices(0) ' Remember which item is selected
        DialogResult = DialogResult.OK
    End Sub
    Private Sub FindResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Enable double-buffering for the results view
        Dim doubleBufferProp = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
        doubleBufferProp.SetValue(ResultsListview, True)
    End Sub
End Class