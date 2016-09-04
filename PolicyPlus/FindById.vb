Public Class FindById
    Public AdmxWorkspace As AdmxBundle
    Public SelectedPolicy As PolicyPlusPolicy
    Public SelectedCategory As PolicyPlusCategory
    Public SelectedProduct As PolicyPlusProduct
    Public SelectedSupport As PolicyPlusSupport
    Public SelectedSection As AdmxPolicySection ' Specifies the section for policies
    Dim CategoryImage As Image
    Dim PolicyImage As Image
    Dim ProductImage As Image
    Dim SupportImage As Image
    Dim NotFoundImage As Image
    Dim BlankImage As Image
    Private Sub FindById_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CategoryImage = Main.PolicyIcons.Images(0)
        PolicyImage = Main.PolicyIcons.Images(4)
        ProductImage = Main.PolicyIcons.Images(10)
        SupportImage = Main.PolicyIcons.Images(11)
        NotFoundImage = Main.PolicyIcons.Images(8)
        BlankImage = Main.PolicyIcons.Images(9)
    End Sub
    Private Sub IdTextbox_TextChanged(sender As Object, e As EventArgs) Handles IdTextbox.TextChanged
        If AdmxWorkspace Is Nothing Then Exit Sub ' Wait until actually shown
        SelectedPolicy = Nothing
        SelectedCategory = Nothing
        SelectedProduct = Nothing
        SelectedSupport = Nothing
        Dim id = Trim(IdTextbox.Text)
        If AdmxWorkspace.FlatCategories.ContainsKey(id) Then
            StatusImage.Image = CategoryImage
            SelectedCategory = AdmxWorkspace.FlatCategories(id)
        ElseIf AdmxWorkspace.FlatProducts.ContainsKey(id) Then
            StatusImage.Image = ProductImage
            SelectedProduct = AdmxWorkspace.FlatProducts(id)
        ElseIf AdmxWorkspace.SupportDefinitions.ContainsKey(id) Then
            StatusImage.Image = SupportImage
            SelectedSupport = AdmxWorkspace.SupportDefinitions(id)
        Else ' Check for a policy
            Dim policyAndSection = Split(id, "@", 2)
            Dim policyId = policyAndSection(0) ' Cut off the section override
            If AdmxWorkspace.Policies.ContainsKey(policyId) Then
                StatusImage.Image = PolicyImage
                SelectedPolicy = AdmxWorkspace.Policies(policyId)
                If policyAndSection.Length = 2 AndAlso policyAndSection(1).Length = 1 AndAlso "UC".Contains(policyAndSection(1)) Then
                    SelectedSection = If(policyAndSection(1) = "U", AdmxPolicySection.User, AdmxPolicySection.Machine)
                Else
                    SelectedSection = AdmxPolicySection.Both
                End If
            Else
                StatusImage.Image = If(id = "", BlankImage, NotFoundImage)
            End If
        End If
    End Sub
    Private Sub FindById_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If IdTextbox.Text = " " Then IdTextbox.Text = "" ' It's set to a single space in the designer
        IdTextbox.Focus()
        IdTextbox.SelectAll()
    End Sub
    Private Sub GoButton_Click(sender As Object, e As EventArgs) Handles GoButton.Click
        DialogResult = DialogResult.OK ' Close
    End Sub
    Private Sub FindById_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class