// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Column;
using BSOA.Extensions;
using BSOA.Model;

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
    public class ColumnList<T> : IList<T>, IReadOnlyList<T>
    {
        private const int MinimumSize = 16;
        private readonly ListColumn<T> _column;
        internal readonly int _rowIndex;

        private NumberList<int> _indices;
        private IColumn<T> _values;

        public static ColumnList<T> Empty = new ColumnList<T>(null, 0, NumberList<int>.Empty);

        protected ColumnList(ListColumn<T> column, int index, NumberList<int> indices)
        {
            _column = column;
            _rowIndex = index;

            _indices = indices ?? NumberList<int>.Empty;
            _values = _column?._values;
        }

        public static ColumnList<T> Get(ListColumn<T> column, int index)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }

            NumberList<int> indices = column._indices[index];
            return (indices == null ? null : new ColumnList<T>(column, index, indices));
        }

        public static void Set(ListColumn<T> column, int index, IEnumerable<T> value)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }

            if (value == null)
            {
                column._indices[index] = null;
            }
            else
            {
                NumberList<int> indices = column._indices[index];
                new ColumnList<T>(column, index, indices).SetTo(value);
            }
        }

        private void Init()
        {
            // Setting List to empty 'coerces' list creation in correct column
            if (_indices.Count == 0)
            {
                _column._indices[_rowIndex] = NumberList<int>.Empty;
                _indices = _column._indices[_rowIndex];
            }
        }

        public T this[int indexWithinList]
        {
            get => _values[_indices[indexWithinList]];
            set => _values[_indices[indexWithinList]] = value;
        }

        public int Count => _indices.Count;
        public bool IsReadOnly => false;

        public void SetTo(IEnumerable<T> other)
        {
            if (other is ColumnList<T>)
            {
                // Avoid Clear() on SetTo(self)
                ColumnList<T> otherList = (ColumnList<T>)other;
                if (object.ReferenceEquals(this._column, otherList._column) && this._rowIndex == otherList._rowIndex)
                {
                    return;
                }
            }

            Clear();

            if (other != null)
            {
                foreach (T item in other)
                {
                    Add(item);
                }
            }
        }

        public void Add(T item)
        {
            // Add the new value itself
            int newValueIndex = _values.Count;
            _values[newValueIndex] = item;

            // Add a new index to the list of indices pointing to this value
            Init();
            _indices.Add(newValueIndex);
        }

        public void Clear()
        {
            // Clear values (still need to reclaim space later)
            if (Count > 0)
            {
                foreach (int index in _indices)
                {
                    _values[index] = default(T);
                }
            }

            // Clear indices
            Init();
            _indices.Clear();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            ArraySlice<int> indices = _indices.Slice;
            int[] indicesArray = indices.Array;
            int end = indices.Index + indices.Count;

            for (int i = indices.Index; i < end; ++i)
            {
                int indexOfValue = indicesArray[i];
                if (_values[indexOfValue].Equals(item)) { return i - indices.Index; }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            // Add the new value itself
            int newValueIndex = _values.Count;
            _values[_values.Count] = item;

            // Insert the index in the correct position
            Init();
            _indices.Insert(index, newValueIndex);
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
            if (index < 0 || index >= Count) { throw new IndexOutOfRangeException(nameof(index)); }

            // Remove item
            _values[_indices[index]] = default(T);

            // Remove index
            _indices.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorConverter<int, T>(_indices.GetEnumerator(), GetValue);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EnumeratorConverter<int, T>(_indices.GetEnumerator(), GetValue);
        }

        private T GetValue(int index)
        {
            return _values[index];
        }

        public override int GetHashCode()
        {
            return ReadOnlyListExtensions.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            return ReadOnlyListExtensions.AreEqual(this, obj as IReadOnlyList<T>);
        }

        public static bool operator ==(ColumnList<T> left, IReadOnlyList<T> right)
        {
            if (object.ReferenceEquals(left, null)) { return object.ReferenceEquals(right, null); }
            return left.Equals(right);
        }

        public static bool operator !=(ColumnList<T> left, IReadOnlyList<T> right)
        {
            if (object.ReferenceEquals(left, null)) { return object.ReferenceEquals(right, null); }
            return !left.Equals(right);
        }
    }
}
