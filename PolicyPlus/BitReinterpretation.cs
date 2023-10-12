using System.Runtime.InteropServices;

namespace PolicyPlus
{

    [StructLayout(LayoutKind.Explicit)]
    public struct ReinterpretableDword
    {
        [FieldOffset(0)]
        public int Signed;
        [FieldOffset(0)]
        public uint Unsigned;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ReinterpretableQword
    {
        [FieldOffset(0)]
        public long Signed;
        [FieldOffset(0)]
        public ulong Unsigned;
    }
}