using BSOA.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  NumberList exposes a mutable IList on top of ArraySlices from NumberListColumn.
    /// </summary>
    /// <remarks>
    ///  NumberList is a struct to avoid allocations when reading values.
    ///  NumberList therefore "points" to a row index in the column and retrieves the current slice on use.
    ///  NumberList persists each Count change back to the column immediately.
    /// </remarks>
    public readonly struct NumberList<T> : IList<T>, IReadOnlyList<T> where T : unmanaged
    {
        private const int MinimumSize = 16;

        // Store a reference to the column and index containing the real ArraySlice value.
        private readonly IColumn<ArraySlice<T>> _column;
        private readonly int _index;

        public static NumberList<T> Empty = new NumberList<T>();

        public NumberList(IColumn<ArraySlice<T>> column, int index)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
            _column = column;
            _index = index;
        }

        public ArraySlice<T> Slice => _column?[_index] ?? ArraySlice<T>.Empty;

        // Retrieve values from the current ArraySlice; allow updating values in-place (if the array size doesn't change, updates can be made inline)
        public T this[int index]
        {
            get => Slice[index];
            set
            {
                ArraySlice<T> slice = Slice;
                slice.Array[slice.Index + index] = value;
            }
        }

        public int Count => Slice.Count;
        public bool IsReadOnly => false;

        // Take another ArraySlice directly
        public void SetTo(ArraySlice<T> other)
        {
            _column[_index] = other;
        }

        public void Add(T item)
        {
            ArraySlice<T> slice = Slice;
            int nextIndex = slice.Index + slice.Count;

            if (slice.IsExpandable && nextIndex < slice.Array.Length)
            {
                // If array can be added to and isn't full, append in place
                slice.Array[nextIndex] = item;

                // Record new length
                _column[_index] = new ArraySlice<T>(slice.Array, slice.Index, slice.Count + 1, slice.IsExpandable);
            }
            else
            {
                // Otherwise, allocate a new array and copy items
                int newSize = Math.Max(MinimumSize, slice.Count + slice.Count / 2);
                T[] newArray = new T[newSize];

                if (slice.Count > 0)
                {
                    Array.Copy(slice.Array, slice.Index, newArray, 0, slice.Count);
                }

                // Add new item
                newArray[slice.Count] = item;

                // Record new expandable slice with new array and length
                _column[_index] = new ArraySlice<T>(newArray, 0, slice.Count + 1, isExpandable: true);
            }
        }

        public void Clear()
        {
            _column[_index] = ArraySlice<T>.Empty;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Slice.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            ArraySlice<T> slice = Slice;
            T[] array = slice.Array;
            int end = slice.Index + slice.Count;

            for (int i = slice.Index; i < end; ++i)
            {
                if (array[i].Equals(item)) { return i - slice.Index; }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            ArraySlice<T> slice = Slice;
            if (index < 0 || index >= slice.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            // Use add to resize array (inserting to-be-overwritten value)
            Add(item);
            slice = Slice;

            // Shift items from index forward one
            T[] array = slice.Array;
            int realIndex = slice.Index + index;
            int countFromIndex = slice.Count - index;
            Array.Copy(array, realIndex, array, realIndex + 1, countFromIndex);

            // Insert item at desired index
            array[realIndex] = item;

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
            ArraySlice<T> slice = Slice;
            if (index < 0 || index >= slice.Count) { throw new IndexOutOfRangeException(nameof(index)); }

            T[] array = slice.Array;
            int realIndex = slice.Index + index;
            int countAfterIndex = slice.Count - 1 - index;

            // Shift items after index back one
            Array.Copy(slice.Array, realIndex + 1, slice.Array, realIndex, countAfterIndex);

            // Record shortened length
            _column[_index] = new ArraySlice<T>(slice.Array, slice.Index, slice.Count - 1, slice.IsExpandable);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Slice.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Slice.GetEnumerator();
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            for (int i = 0; i < Count; ++i)
            {
                hashCode = unchecked(hashCode * 17) + this[i].GetHashCode();
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
                if (!other[i].Equals(this[i])) { return false; }
            }

            return true;
        }

        public static bool operator ==(NumberList<T> left, NumberList<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NumberList<T> left, NumberList<T> right)
        {
            return !(left == right);
        }
    }
}
