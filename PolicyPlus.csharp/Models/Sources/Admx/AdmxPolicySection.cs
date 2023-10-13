using System;

namespace PolicyPlus.csharp.Models.Sources.Admx
{
    [Flags]
    public enum AdmxPolicySection
    {
        Machine = 1,
        User = 2,
        Both = 3
    }
}