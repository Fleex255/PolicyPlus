Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Temporary testing - set a breakpoint at the "End Sub" to examine the objects with Visual Studio
        Dim bitsAdmx = AdmxFile.Load("C:\Windows\PolicyDefinitions\Bits.admx")
        Dim tpmAdmx = AdmxFile.Load("C:\Windows\PolicyDefinitions\TPM.admx")
        Dim bitsAdml = AdmlFile.Load("C:\Windows\PolicyDefinitions\en-us\Bits.adml")
        Dim tpmAdml = AdmlFile.Load("C:\Windows\PolicyDefinitions\en-us\TPM.adml")
    End Sub
End Class
