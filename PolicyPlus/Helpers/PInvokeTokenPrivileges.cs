using System.Runtime.InteropServices;

namespace PolicyPlus.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PInvokeTokenPrivileges
    {
        public uint PrivilegeCount;
        public PInvokeLuid LUID;
        public uint Attributes;
    }
}