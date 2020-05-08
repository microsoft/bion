using BSOA.Column;
using BSOA.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  List exposes a mutable IList on top of ListColumn.
    /// </summary>
    /// <remarks>
    ///  List is a struct to avoid allocations when reading values.
    ///  List therefore "points" to a row index in the column and retrieves the current slice on use.
    ///  List persists each Count change back to the column immediately.
    /// </remarks>
    public readonly struct ColumnList<T> : IList<T>, IReadOnlyList<T>
    {
        private const int MinimumSize = 16;
        private readonly ListColumn<T> _column;
        private readonly int _indexOfList;

        public static ColumnList<T> Empty = new ColumnList<T>();

        public ColumnList(ListColumn<T> column, int index)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
            _column = column;
            _indexOfList = index;
        }

        internal ArraySlice<int> Indices => _column?._indices[_indexOfList] ?? ArraySlice<int>.Empty;
        internal IColumn<T> Values => _column?._values;

        public T this[int indexWithinList]
        {
            get => Values[Indices[indexWithinList]];
            set => Values[Indices[indexWithinList]] = value;
        }

        public int Count => Indices.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            // Add the new value itself
            int newValueIndex = Values.Count;
            Values[Values.Count] = item;

            // Add a new index to the list of indices pointing to this value
            ArraySlice<int> indices = Indices;
            int nextIndex = indices.Index + indices.Count;

            if (indices.IsExpandable && nextIndex < indices.Array.Length)
            {
                // If array can be added to and isn't full, append in place
                indices.Array[nextIndex] = newValueIndex;

                // Record new length
                _column._indices[_indexOfList] = new ArraySlice<int>(indices.Array, indices.Index, indices.Count + 1, indices.IsExpandable);
            }
            else
            {
                // Otherwise, allocate a new array and copy items
                int newSize = Math.Max(MinimumSize, indices.Count + indices.Count / 2);
                int[] newArray = new int[newSize];

                if (indices.Count > 0)
                {
                    Array.Copy(indices.Array, indices.Index, newArray, 0, indices.Count);
                }

                // Add new item
                newArray[indices.Count] = newValueIndex;

                // Record new expandable slice with new array and length
                _column._indices[_indexOfList] = new ArraySlice<int>(newArray, 0, indices.Count + 1, isExpandable: true);
            }
        }

        public void Clear()
        {
            // Clear values (still need to reclaim space later)
            foreach (int index in Indices)
            {
                Values[index] = default(T);
            }

            // Clear indices
            _column._indices[_indexOfList] = ArraySlice<int>.Empty;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            IColumn<T> values = Values;
            ArraySlice<int> indices = Indices;

            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex + indices.Count > array.Length) { throw new ArgumentException(nameof(arrayIndex)); }

            for (int i = 0; i < indices.Count; ++i)
            {
                array[arrayIndex + i] = values[indices[i]];
            }
        }

        public int IndexOf(T item)
        {
            IColumn<T> values = Values;

            ArraySlice<int> indices = Indices;
            int[] indicesArray = indices.Array;
            int end = indices.Index + indices.Count;

            for (int i = indices.Index; i < end; ++i)
            {
                int indexOfValue = indicesArray[i];
                if (values[indexOfValue].Equals(item)) { return i - indices.Index; }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            ArraySlice<int> indices = Indices;
            if (index < 0 || index >= indices.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            // Use add to resize array (inserting to-be-overwritten value)
            Add(item);
            indices = Indices;
            int newValueIndex = indices[indices.Count - 1];

            // Shift items from index forward one
            int[] array = indices.Array;
            int realIndex = indices.Index + index;
            int countFromIndex = indices.Count - index;
            Array.Copy(array, realIndex, array, realIndex + 1, countFromIndex);

            // Insert item at desired index
            array[realIndex] = newValueIndex;

            // New slice length already recorded by Add()
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            ArraySlice<int> indices = Indices;
            if (index < 0 || index >= indices.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            int[] array = indices.Array;
            int realIndex = indices.Index + index;
            int countAfterIndex = indices.Count - 1 - index;

            // Remove item
            Values[indices[index]] = default(T);

            // Shift items after index back one
            Array.Copy(indices.Array, realIndex + 1, indices.Array, realIndex, countAfterIndex);

            // Record shortened length
            _column._indices[_indexOfList] = new ArraySlice<int>(indices.Array, indices.Index, indices.Count - 1, indices.IsExpandable);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            for (int i = 0; i < Count; ++i)
            {
                hashCode = unchecked(hashCode * 17) + this[i]?.GetHashCode() ?? 0;
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            IReadOnlyList<T> other = obj as IReadOnlyList<T>;
            if (other == null) { return false; }

            if (other.Count != this.Count) { return false; }
            for (int i = 0; i < this.Count; ++i)
            {
                if (!object.Equals(other[i], this[i])) { return false; }
            }

            return true;
        }

        public static bool operator ==(ColumnList<T> left, ColumnList<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColumnList<T> left, ColumnList<T> right)
        {
            return !(left == right);
        }
    }
}
