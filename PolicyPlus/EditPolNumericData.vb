Public Class EditPolNumericData
    Public Function PresentDialog(ValueName As String, InitialData As ULong, IsQword As Boolean) As DialogResult
        TextName.Text = ValueName
        NumData.Maximum = If(IsQword, ULong.MaxValue, UInteger.MaxValue)
        NumData.Value = InitialData
        NumData.Select()
        Return ShowDialog()
    End Function
    Private Sub CheckHexadecimal_CheckedChanged(sender As Object, e As EventArgs) Handles CheckHexadecimal.CheckedChanged
        NumData.Hexadecimal = CheckHexadecimal.Checked
    End Sub
    Private Sub EditPolNumericData_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then DialogResult = DialogResult.Cancel
    End Sub
End Class
' The normal NumericUpDown has a bug - it's unable to handle numbers greater than 0x7FFFFFF when in hex mode
' This subclass fixes that bug
' Adapted from https://social.msdn.microsoft.com/Forums/windows/en-US/6eea9c6c-a43c-4ef1-a7a3-de95e17e77a8/numericupdown-hexadecimal-bug?forum=winforms
Friend Class WideRangeNumericUpDown
    Inherits NumericUpDown
    Protected Overrides Sub UpdateEditText()
        If Hexadecimal Then
            If UserEdit Then HexParseEditText()
            If Not String.IsNullOrEmpty(Text) Then
                ChangingText = True
                Text = String.Format("{0:X}", CULng(Value))
            End If
        Else
            MyBase.UpdateEditText()
        End If
    End Sub
    Protected Overrides Sub ValidateEditText()
        If Hexadecimal Then
            HexParseEditText()
            UpdateEditText()
        Else
            MyBase.ValidateEditText()
        End If
    End Sub
    Private Sub HexParseEditText()
        Try
            If Not String.IsNullOrEmpty(Text) Then Value = ULong.Parse(Text, Globalization.NumberStyles.HexNumber)
        Catch ex As ArgumentOutOfRangeException
            Value = Maximum
        Catch ex As OverflowException
            If Not String.IsNullOrEmpty(Text) Then
                If Text.StartsWith("-") Then
                    Value = Minimum
                Else
                    Value = Maximum
                End If
            End If
        Catch ex As Exception
            ' Do nothing
        Finally
            UserEdit = False
        End Try
    End Sub
End Class