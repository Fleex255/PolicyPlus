Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Temporary testing - set a breakpoint at the "End Sub" to examine the objects with Visual Studio
        Dim cmtx = CmtxFile.Load("C:\Windows\System32\GroupPolicy\Machine\comment.cmtx")
        Dim commentTable = cmtx.ToCommentTable
    End Sub
End Class
