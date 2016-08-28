Public Class DetailCategory
    Dim SelectedCategory As PolicyPlusCategory
    Public Sub PresentDialog(Category As PolicyPlusCategory)
        PrepareDialog(Category)
        ShowDialog()
    End Sub
    Private Sub PrepareDialog(Category As PolicyPlusCategory)
        SelectedCategory = Category
        NameTextbox.Text = Category.DisplayName
        IdTextbox.Text = Category.UniqueID
        DefinedTextbox.Text = Category.RawCategory.DefinedIn.SourceFile
        DisplayCodeTextbox.Text = Category.RawCategory.DisplayCode
        InfoCodeTextbox.Text = Category.RawCategory.ExplainCode
        ParentButton.Enabled = Category.Parent IsNot Nothing
        If Category.Parent IsNot Nothing Then
            ParentTextbox.Text = Category.Parent.DisplayName
        ElseIf Category.RawCategory.ParentID <> "" Then
            ParentTextbox.Text = "<orphaned from " & Category.RawCategory.ParentID & ">"
        Else
            ParentTextbox.Text = ""
        End If
    End Sub
    Private Sub ParentButton_Click(sender As Object, e As EventArgs) Handles ParentButton.Click
        PrepareDialog(SelectedCategory.Parent)
    End Sub
End Class