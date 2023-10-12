using System;

namespace PolicyPlus.Models
{
    [Flags]
    public enum AdmxPolicySection
    {
        Machine = 1,
        User = 2,
        Both = 3
    }
}