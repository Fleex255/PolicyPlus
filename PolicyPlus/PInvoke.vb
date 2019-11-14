Imports System.Runtime.InteropServices
Class PInvoke
    Declare Function ShowScrollBar Lib "user32.dll" (Handle As IntPtr, Scrollbar As Integer, Show As Boolean) As Boolean
    Declare Function RefreshPolicyEx Lib "userenv.dll" (IsMachine As Boolean, Options As UInteger) As Boolean
    Declare Unicode Function RegLoadKeyW Lib "advapi32.dll" (HiveKey As IntPtr, Subkey As String, File As String) As Integer
    Declare Unicode Function RegUnLoadKeyW Lib "advapi32.dll" (HiveKey As IntPtr, Subkey As String) As Integer
    Declare Function GetCurrentProcess Lib "kernel32.dll" () As IntPtr
    Declare Function OpenProcessToken Lib "advapi32.dll" (Process As IntPtr, Access As UInteger, ByRef TokenHandle As IntPtr) As Boolean
    Declare Function AdjustTokenPrivileges Lib "advapi32.dll" (Token As IntPtr, DisableAll As Boolean, ByRef NewState As PInvokeTokenPrivileges, BufferLength As UInteger, Null As IntPtr, ByRef ReturnLength As UInteger) As Boolean
    Declare Unicode Function LookupPrivilegeValueW Lib "advapi32.dll" (SystemName As String, Name As String, ByRef LUID As PInvokeLuid) As Boolean
    Declare Function CloseHandle Lib "kernel32.dll" (Handle As IntPtr) As Boolean
    Declare Function GetProductInfo Lib "kernel32.dll" (MajorVersion As Integer, MinorVersion As Integer, SPMajor As Integer, SPMinor As Integer, ByRef EditionCode As Integer) As Boolean
    Declare Unicode Function SendNotifyMessageW Lib "user32.dll" (Handle As IntPtr, Message As Integer, WParam As UIntPtr, LParam As IntPtr) As Boolean
End Class
<StructLayout(LayoutKind.Sequential)> Structure PInvokeTokenPrivileges
    Public PrivilegeCount As UInteger
    Public LUID As PInvokeLuid
    Public Attributes As UInteger
End Structure
<StructLayout(LayoutKind.Sequential)> Structure PInvokeLuid
    Public LowPart As UInteger
    Public HighPart As Integer
End Structure