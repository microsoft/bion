// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Model
{
    /// <summary>
    ///  Types which represent Rows from a Table must implement IRow.
    /// </summary>
    public interface IRow<T>
    {
        // Provide the Table and Index to allow the Table to identify
        // the source of the row and copy it from other Table instances.
        ITable Table { get; }
        int Index { get; }

        void CopyFrom(T other);
    }
}
