﻿using System.Runtime.InteropServices;

namespace PolicyPlus.csharp.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ReinterpretableQword
    {
        [FieldOffset(0)]
        public long Signed;

        [FieldOffset(0)]
        public ulong Unsigned;
    }
}