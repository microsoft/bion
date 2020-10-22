// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Model
{
    /// <summary>
    ///  Nullability is used to specify the desired nullability of a column.
    ///  'NullsDisallowed' is fastest (no null bit checked), and means the column cannot store nulls and defaults to empty.
    ///  'DefaultToNull' is nulls allowed and null is the default.
    ///  'DefaultToEmpty' is nulls allowed but empty is the default.
    ///  Use with NullableColumn.Wrap() to construct columns according to the desired nullability.
    /// </summary>
    public enum Nullability : byte
    {
        DefaultToNull = 0,
        DefaultToEmpty = 1,
        NullsDisallowed = 2
    }
}
