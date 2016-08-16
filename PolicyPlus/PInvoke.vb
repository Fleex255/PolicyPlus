Public Class PInvoke
    Declare Function ShowScrollBar Lib "user32.dll" (Handle As IntPtr, Scrollbar As Integer, Show As Boolean) As Boolean
    Declare Function RefreshPolicyEx Lib "userenv.dll" (IsMachine As Boolean, Options As UInteger) As Boolean
End Class
