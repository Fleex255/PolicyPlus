Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Temporary testing - set a breakpoint at the "End Sub" to examine the objects with Visual Studio
        Dim cmtx = CmtxFile.Load("\Windows\System32\GroupPolicy\Machine\comment.cmtx")
        Dim commentDict = cmtx.ToCommentTable
        commentDict.Add("Microsoft.Policies.BITS:BITS_Job_Timeout", "Timeout for a BITS job")
        cmtx = CmtxFile.FromCommentTable(commentDict)
        cmtx.Save("test.cmtx")
    End Sub
End Class