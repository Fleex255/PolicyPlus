using System;
using System.Runtime.InteropServices;

namespace PolicyPlus
{
    class PInvoke
    {
        [DllImport("user32.dll")]
        static extern bool ShowScrollBar(IntPtr Handle, int Scrollbar, bool Show);
        [DllImport("userenv.dll")]
        static extern bool RefreshPolicyEx(bool IsMachine, uint Options);
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        static extern int RegLoadKeyW(IntPtr HiveKey, string Subkey, string File);
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        static extern int RegUnLoadKeyW(IntPtr HiveKey, string Subkey);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();
        [DllImport("advapi32.dll")]
        static extern bool OpenProcessToken(IntPtr Process, uint Access, ref IntPtr TokenHandle);
        [DllImport("advapi32.dll")]
        static extern bool AdjustTokenPrivileges(IntPtr Token, bool DisableAll, ref PInvokeTokenPrivileges NewState, uint BufferLength, IntPtr Null, ref uint ReturnLength);
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        static extern bool LookupPrivilegeValueW(string SystemName, string Name, ref PInvokeLuid LUID);
        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr Handle);
        [DllImport("kernel32.dll")]
        static extern bool GetProductInfo(int MajorVersion, int MinorVersion, int SPMajor, int SPMinor, ref int EditionCode);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool SendNotifyMessageW(IntPtr Handle, int Message, UIntPtr WParam, IntPtr LParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PInvokeTokenPrivileges
    {
        public uint PrivilegeCount;
        public PInvokeLuid LUID;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct PInvokeLuid
    {
        public uint LowPart;
        public int HighPart;
    }
}