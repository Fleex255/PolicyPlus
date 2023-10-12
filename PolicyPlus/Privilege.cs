using System;
using System.Runtime.InteropServices;

namespace PolicyPlus
{
    public class Privilege
    {
        public static void EnablePrivilege(string Name)
        {
            // Enable the given Win32 privilege
            var luid = default(PInvokeLuid);
            PInvokeTokenPrivileges priv;
            IntPtr thisProcToken;
            PInvoke.OpenProcessToken(PInvoke.GetCurrentProcess(), 0x28U, ref thisProcToken); // 0x28 = TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY
            string argSystemName = null;
            PInvoke.LookupPrivilegeValueW(ref argSystemName, ref Name, ref luid);
            priv.Attributes = 2U; // SE_PRIVILEGE_ENABLED
            priv.PrivilegeCount = 1U;
            priv.LUID = luid;
            uint argReturnLength = 0U;
            PInvoke.AdjustTokenPrivileges(thisProcToken, false, ref priv, (uint)Marshal.SizeOf(priv), IntPtr.Zero, ref argReturnLength);
            PInvoke.CloseHandle(thisProcToken);
        }
    }
}