// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.IO;

namespace BSOA.Model
{
    /// <summary>
    ///  All Column types in BSOA must implement IColumn&lt;T&gt;.
    ///  Inherit from LimitedList to minimize the number of members which must be directly implemented.
    /// </summary>
    /// <remarks>
    ///  IColumn provides:
    ///   ITreeSerializable, for serialization.
    ///   IReadOnlyList&lt;T&gt;, IReadOnlyCollection&lt;T&gt;, IEnumerable&lt;T&gt; for broad usability when reading.
    ///   ICollection&lt;T&gt; to support collection initializers.
    ///   ILimitedList&lt;T&gt; to support adding and removing at the end of the column only.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface IColumn<T> : ILimitedList<T>, IColumn
    {
        // Resolve ambiguity between ITreeSerializable.Clear, ILimitedList -> ICollection.Clear
        new void Clear();
    }

    public interface IColumn : ILimitedList, ITreeSerializable
    {
        // Resolve ambiguity between ITreeSerializable.Clear, ILimitedList -> ICollection.Clear
        new void Clear();
    }
}
