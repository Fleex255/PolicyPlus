using System;
using System.Runtime.InteropServices;
using PolicyPlus.csharp.Helpers;

namespace PolicyPlus.csharp.Models
{
    public static class Privilege
    {
        public static void EnablePrivilege(string name)
        {
            // Enable the given Win32 privilege
            var luid = default(PInvokeLuid);
            PInvokeTokenPrivileges priv;
            IntPtr thisProcToken = default;
            _ = PInvoke.OpenProcessToken(PInvoke.GetCurrentProcess(), 0x28U, ref thisProcToken); // 0x28 = TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY
            var argSystemName = string.Empty;
            _ = PInvoke.LookupPrivilegeValueW(argSystemName, name, ref luid);
            priv.Attributes = 2U; // SE_PRIVILEGE_ENABLED
            priv.PrivilegeCount = 1U;
            priv.LUID = luid;
            var argReturnLength = 0U;
            _ = PInvoke.AdjustTokenPrivileges(thisProcToken, false, ref priv, (uint)Marshal.SizeOf(priv), IntPtr.Zero, ref argReturnLength);
            _ = PInvoke.CloseHandle(thisProcToken);
        }
    }
}