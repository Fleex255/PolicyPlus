Public Class DetailPolicy
    Dim SelectedPolicy As PolicyPlusPolicy
    Public Sub PresentDialog(Policy As PolicyPlusPolicy)
        SelectedPolicy = Policy
        NameTextbox.Text = Policy.DisplayName
        IdTextbox.Text = Policy.UniqueID
        DefinedTextbox.Text = Policy.RawPolicy.DefinedIn.SourceFile
        DisplayCodeTextbox.Text = Policy.RawPolicy.DisplayCode
        InfoCodeTextbox.Text = Policy.RawPolicy.ExplainCode
        PresentCodeTextbox.Text = Policy.RawPolicy.PresentationID
        Select Case Policy.RawPolicy.Section
            Case AdmxPolicySection.Both
                SectionTextbox.Text = "User or computer"
            Case AdmxPolicySection.Machine
                SectionTextbox.Text = "Computer"
            Case AdmxPolicySection.User
                SectionTextbox.Text = "User"
        End Select
        SupportButton.Enabled = Policy.SupportedOn IsNot Nothing
        If Policy.SupportedOn IsNot Nothing Then
            SupportTextbox.Text = Policy.SupportedOn.DisplayName
        ElseIf Policy.RawPolicy.SupportedCode <> "" Then
            SupportTextbox.Text = "<missing: " & Policy.RawPolicy.SupportedCode & ">"
        Else
            SupportTextbox.Text = ""
        End If
        CategoryButton.Enabled = Policy.Category IsNot Nothing
        If Policy.Category IsNot Nothing Then
            CategoryTextbox.Text = Policy.Category.DisplayName
        ElseIf Policy.RawPolicy.CategoryID <> "" Then
            CategoryTextbox.Text = "<orphaned from " & Policy.RawPolicy.CategoryID & ">"
        Else
            CategoryTextbox.Text = "<uncategorized>"
        End If
        ShowDialog()
    End Sub
    Private Sub SupportButton_Click(sender As Object, e As EventArgs) Handles SupportButton.Click
        DetailSupport.PresentDialog(SelectedPolicy.SupportedOn)
    End Sub
    Private Sub CategoryButton_Click(sender As Object, e As EventArgs) Handles CategoryButton.Click
        DetailCategory.PresentDialog(SelectedPolicy.Category)
    End Sub
End Class