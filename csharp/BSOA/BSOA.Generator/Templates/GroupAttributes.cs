using System;

namespace BSOA.Generator.Templates
{
    [Flags]
    public enum GroupAttributes : long
    {
        None = 0,
        Discussion = 1,
        Security = 2,
        Managed = 4,
        AutoExpiring = 8
    }
}
