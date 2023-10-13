using System.Runtime.InteropServices;

namespace PolicyPlus.csharp.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PInvokeTokenPrivileges
    {
        public uint PrivilegeCount;
        public PInvokeLuid LUID;
        public uint Attributes;
    }
}