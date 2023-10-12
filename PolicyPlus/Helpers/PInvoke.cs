using System;
using System.Runtime.InteropServices;

namespace PolicyPlus.Helpers
{
    internal static class PInvoke
    {
        [DllImport("user32.dll")]
        internal static extern bool ShowScrollBar(IntPtr Handle, int scrollbar, bool show);

        [DllImport("userenv.dll")]
        internal static extern bool RefreshPolicyEx(bool IsMachine, uint options);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern int RegLoadKeyW(IntPtr HiveKey, string subkey, string file);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern int RegUnLoadKeyW(IntPtr HiveKey, string subkey);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll")]
        internal static extern bool OpenProcessToken(IntPtr Process, uint access, ref IntPtr tokenHandle);

        [DllImport("advapi32.dll")]
        internal static extern bool AdjustTokenPrivileges(IntPtr Token, bool disableAll, ref PInvokeTokenPrivileges newState, uint bufferLength, IntPtr @null, ref uint returnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool LookupPrivilegeValueW(string SystemName, string name, ref PInvokeLuid luid);

        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr Handle);

        [DllImport("kernel32.dll")]
        internal static extern bool GetProductInfo(int MajorVersion, int minorVersion, int spMajor, int spMinor, ref int editionCode);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SendNotifyMessageW(IntPtr Handle, int message, UIntPtr wParam, IntPtr lParam);
    }
}