Public Class DetailSupport
    Public Sub PresentDialog(Supported As PolicyPlusSupport)
        PrepareDialog(Supported)
        ShowDialog()
    End Sub
    Sub PrepareDialog(Supported As PolicyPlusSupport)
        NameTextbox.Text = Supported.DisplayName
        IdTextbox.Text = Supported.UniqueID
        DefinedTextbox.Text = Supported.RawSupport.DefinedIn.SourceFile
        DisplayCodeTextbox.Text = Supported.RawSupport.DisplayCode
        Select Case Supported.RawSupport.Logic
            Case AdmxSupportLogicType.AllOf
                LogicTextbox.Text = "Match all the referenced products"
            Case AdmxSupportLogicType.AnyOf
                LogicTextbox.Text = "Match any of the referenced products"
            Case AdmxSupportLogicType.Blank
                LogicTextbox.Text = "Do not match products"
        End Select
        EntriesListview.Items.Clear()
        If Supported.Elements IsNot Nothing Then
            For Each element In Supported.Elements
                Dim lsvi As ListViewItem
                If element.SupportDefinition IsNot Nothing Then
                    lsvi = EntriesListview.Items.Add(element.SupportDefinition.DisplayName)
                ElseIf element.Product IsNot Nothing Then
                    lsvi = EntriesListview.Items.Add(element.Product.DisplayName)
                    If element.RawSupportEntry.IsRange Then
                        If element.RawSupportEntry.MinVersion.HasValue Then lsvi.SubItems.Add(element.RawSupportEntry.MinVersion.Value) Else lsvi.SubItems.Add("")
                        If element.RawSupportEntry.MaxVersion.HasValue Then lsvi.SubItems.Add(element.RawSupportEntry.MaxVersion.Value) Else lsvi.SubItems.Add("")
                    End If
                Else
                    lsvi = EntriesListview.Items.Add("<missing: " & element.RawSupportEntry.ProductID & ">")
                End If
                lsvi.Tag = element
            Next
        End If
    End Sub
    Private Sub EntriesListview_ClientSizeChanged(sender As Object, e As EventArgs) Handles EntriesListview.ClientSizeChanged, Me.Shown
        ChName.Width = EntriesListview.ClientSize.Width - ChMinVer.Width - ChMaxVer.Width
    End Sub
    Private Sub EntriesListview_DoubleClick(sender As Object, e As EventArgs) Handles EntriesListview.DoubleClick
        If EntriesListview.SelectedItems.Count = 0 Then Exit Sub
        Dim supEntry As PolicyPlusSupportEntry = EntriesListview.SelectedItems(0).Tag
        If supEntry.Product IsNot Nothing Then
            DetailProduct.PresentDialog(supEntry.Product)
        ElseIf supEntry.SupportDefinition IsNot Nothing Then
            PrepareDialog(supEntry.SupportDefinition)
        End If
    End Sub
End Class