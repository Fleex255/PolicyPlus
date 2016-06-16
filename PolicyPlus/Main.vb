Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim bitsTest = AdmxFile.Load("C:\Windows\PolicyDefinitions\Bits.admx")
        Dim tpmTest = AdmxFile.Load("C:\Windows\PolicyDefinitions\TPM.admx")
    End Sub
End Class
