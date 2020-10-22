// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Model
{
    /// <summary>
    ///  CacheItem enables simple lockless caching of the last retrieved row in columns.
    /// </summary>
    /// <remarks>
    ///  Updating a reference variable is atomic.
    ///  Columns can keep a CacheItem variable.
    ///  On read, if the variable has the desired RowIndex, the value must be correct.
    ///  If not, a new CacheItem is created for the requested RowIndex and value.
    ///  When the CacheItem variable is set to the new CacheItem for the new row, that
    ///  replacement is atomic.
    ///  
    ///  See StringColumn for an example.
    /// </remarks>
    /// <typeparam name="T">Type of value being cached</typeparam>
    internal class CacheItem<T>
    {
        public readonly int RowIndex;
        public readonly T Value;

        public CacheItem(int rowIndex, T value)
        {
            RowIndex = rowIndex;
            Value = value;
        }
    }
}
