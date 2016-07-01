Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Temporary testing - set a breakpoint at the "End Sub" to examine the objects with Visual Studio
        Dim bundle As New AdmxBundle
        bundle.LoadFolder("C:\Windows\PolicyDefinitions", "en-us")
    End Sub
End Class
