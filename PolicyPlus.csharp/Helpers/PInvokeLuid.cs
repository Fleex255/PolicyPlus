using System.Runtime.InteropServices;

namespace PolicyPlus.csharp.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PInvokeLuid
    {
        public uint LowPart;
        public int HighPart;
    }
}