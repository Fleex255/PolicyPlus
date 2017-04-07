Imports System.Runtime.InteropServices
Public Class Privilege
    Public Shared Sub EnablePrivilege(Name As String)
        ' Enable the given Win32 privilege
        Dim luid As PInvokeLuid
        Dim priv As PInvokeTokenPrivileges
        Dim thisProcToken As IntPtr
        PInvoke.OpenProcessToken(PInvoke.GetCurrentProcess, &H28, thisProcToken) ' 0x28 = TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY
        PInvoke.LookupPrivilegeValueW(Nothing, Name, luid)
        priv.Attributes = 2 ' SE_PRIVILEGE_ENABLED
        priv.PrivilegeCount = 1
        priv.LUID = luid
        PInvoke.AdjustTokenPrivileges(thisProcToken, False, priv, Marshal.SizeOf(priv), IntPtr.Zero, 0)
        PInvoke.CloseHandle(thisProcToken)
    End Sub
End Class
