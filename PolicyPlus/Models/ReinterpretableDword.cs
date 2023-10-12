using System.Runtime.InteropServices;

namespace PolicyPlus.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ReinterpretableDword
    {
        [FieldOffset(0)]
        public int Signed;

        [FieldOffset(0)]
        public uint Unsigned;
    }
}