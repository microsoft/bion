using BSOA.Column;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  ListSlice exposes a List&lt;T&gt; using an IColumn&lt;T&gt; with the values and a
    ///  VariableLengthColumn&lt;int&gt; to track which values each list has.
    /// </summary>
    /// <remarks>
    ///  MutableSlice is a struct to avoid allocations when reading from a VariableLengthColumn.
    ///  It therefore must update the ArraySlice in the VariableLengthColumn whenever the Count changes.
    ///  It must retrieve the ArraySlice from the VariableLengthColumn on access to ensure it reflects changes another instance has made.
    /// </remarks>
    public struct ListSlice<T> : IList<T>, IReadOnlyList<T>
    {
        private const int MinimumSize = 16;

        // Store a reference to the column and index containing the real ArraySlice value.
        private int _indexInIndices;
        private VariableLengthColumn<int> _indices;
        private IColumn<T> _values;

        public static ListSlice<T> Empty = new ListSlice<T>();

        public ListSlice(int index, VariableLengthColumn<int> indices, IColumn<T> values)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
            _indexInIndices = index;
            _indices = indices;
            _values = values;
        }

        public ArraySlice<int> Slice => _indices?[_indexInIndices] ?? ArraySlice<int>.Empty;

        // Retrieve values from the current ArraySlice; allow updating values in-place (if the array size doesn't change, updates can be made inline)
        public T this[int index]
        {
            get => _values[Slice[index]];
            set
            {
                ArraySlice<int> slice = Slice;
                int valueIndex = slice.Array[slice.Index + index];
                _values[valueIndex] = value;
            }
        }

        public int Count => Slice.Count;
        public bool IsReadOnly => false;

        // Take another ArraySlice directly
        public void SetTo(ArraySlice<int> other)
        {
            _indices[_indexInIndices] = other;
        }

        public void Add(T item)
        {
            ArraySlice<int> slice = Slice;
            int nextIndex = slice.Index + slice.Count;

            int newValueIndex = _values.Count;
            _values[newValueIndex] = item;

            if (slice.IsExpandable && nextIndex < slice.Array.Length)
            {
                // If array can be added to and isn't full, append in place
                slice.Array[nextIndex] = newValueIndex;

                // Record new length
                _indices[_indexInIndices] = new ArraySlice<int>(slice.Array, slice.Index, slice.Count + 1, slice.IsExpandable);
            }
            else
            {
                // Otherwise, allocate a new array and copy items
                int newSize = Math.Max(MinimumSize, slice.Count + slice.Count / 2);
                int[] newArray = new int[newSize];

                if (slice.Count > 0)
                {
                    Array.Copy(slice.Array, slice.Index, newArray, 0, slice.Count);
                }

                // Add new item
                newArray[slice.Count] = newValueIndex;

                // Record new expandable slice with new array and length
                _indices[_indexInIndices] = new ArraySlice<int>(newArray, 0, slice.Count + 1, isExpandable: true);
            }
        }

        public void Clear()
        {
            _indices[_indexInIndices] = ArraySlice<int>.Empty;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (arrayIndex + Count > array.Length) { throw new ArgumentException(nameof(arrayIndex)); }

            for (int i = 0; i < Count; ++i)
            {
                array[i + arrayIndex] = this[i];
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (this[i].Equals(item)) { return i; }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            ArraySlice<int> slice = Slice;
            if (index < 0 || index >= slice.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            // Use add to resize array (inserting to-be-overwritten value)
            Add(item);
            slice = Slice;

            // Shift items from index forward one
            int[] array = slice.Array;
            int realIndex = slice.Index + index;
            int countFromIndex = slice.Count - index;
            Array.Copy(array, realIndex, array, realIndex + 1, countFromIndex);

            // Insert item at desired index
            array[realIndex] = (_values.Count - 1);

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
            ArraySlice<int> slice = Slice;
            if (index < 0 || index >= slice.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            int[] array = slice.Array;
            int realIndex = slice.Index + index;
            int countAfterIndex = slice.Count - 1 - index;

            // Shift items after index back one
            Array.Copy(slice.Array, realIndex + 1, slice.Array, realIndex, countAfterIndex);

            // Record shortened length
            _indices[_indexInIndices] = new ArraySlice<int>(slice.Array, slice.Index, slice.Count - 1, slice.IsExpandable);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }
    }
}
