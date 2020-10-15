// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Extensions;

namespace BSOA.Collections
{
    /// <summary>
    ///  ColumnDictionary represents a Dictionary in a BSOA object model.
    /// </summary>
    /// <remarks>
    ///  DictionaryColumn stores a Dictionary for every (outer) row.
    ///  It keeps a single list of Key/Value pairs across all Dictionaries in Keys and Values columns.
    ///  It also keeps a List&lt;int&gt; for each row, called 'Pairs',
    ///  which identifies which Key/Value pairs belong to the Dictionary for a given (outer) row.
    ///  
    ///  So, to get the 'keyIndex'th KeyValuePair for row 'outerRow':
    ///  int innerRowIndex = _column._pairs[outerRow][keyIndex];
    ///  new KeyValuePair&lt;TKey, TValue&gt;(_column._keys[innerRowIndex], _column._values[innerRowIndex]);
    /// </remarks>
    /// <typeparam name="TKey">Type of Dictionary keys</typeparam>
    /// <typeparam name="TValue">Type of Dictionary values</typeparam>
    public class ColumnDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
    {
        private readonly DictionaryColumn<TKey, TValue> _column;
        private readonly int _rowIndex;
        private NumberList<int> _pairs;
        private TValue Value(int innerRow) => _column._values[innerRow];

        public static ColumnDictionary<TKey, TValue> Empty = new ColumnDictionary<TKey, TValue>(null, 0, NumberList<int>.Empty);

        protected ColumnDictionary(DictionaryColumn<TKey, TValue> column, int index, NumberList<int> pairs)
        {
            _column = column;
            _rowIndex = index;
            _pairs = pairs;
        }

        public static ColumnDictionary<TKey, TValue> Get(DictionaryColumn<TKey, TValue> column, int index)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }

            NumberList<int> pairs = column._pairs[index];
            return (pairs == null ? null : new ColumnDictionary<TKey, TValue>(column, index, pairs));
        }

        public static void Set(DictionaryColumn<TKey, TValue> column, int index, IDictionary<TKey, TValue> value)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }

            if (value == null)
            {
                column._pairs[index] = null;
            }
            else
            {
                // Setting List to empty 'coerces' list creation in correct column
                NumberList<int> pairs = column._pairs[index];

                if (pairs == null)
                {
                    column._pairs[index] = NumberList<int>.Empty;
                    pairs = column._pairs[index];
                }

                new ColumnDictionary<TKey, TValue>(column, index, pairs).SetTo(value);
            }
        }

        public bool IsReadOnly => false;
        public int Count => _pairs.Count;

        public TValue this[TKey key]
        {
            get
            {
                if (this.TryGetValue(key, out TValue value))
                {
                    return value;
                }

                throw new KeyNotFoundException($"Key {key} not found in Dictionary; Keys: {string.Join(", ", Keys)}");
            }

            set
            {
                AddInternal(key, value, setIfExists: true);
            }
        }

        public ICollection<TKey> Keys => new IndirectCollection<TKey>(_column._keys, _pairs.Slice);
        public ICollection<TValue> Values => new IndirectCollection<TValue>(_column._values, _pairs.Slice);

        public void SetTo(IDictionary<TKey, TValue> other)
        {
            if (other is ColumnDictionary<TKey, TValue>)
            {
                // Avoid Clear() on SetTo(self)
                ColumnDictionary<TKey, TValue> otherDictionary = (ColumnDictionary<TKey, TValue>)other;
                if (object.ReferenceEquals(this._column, otherDictionary._column) && this._rowIndex == otherDictionary._rowIndex)
                {
                    return;
                }
            }

            Clear();

            if (other != null)
            {
                foreach (KeyValuePair<TKey, TValue> pair in other)
                {
                    this.AddInternal(pair.Key, pair.Value);
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            AddInternal(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            AddInternal(item.Key, item.Value);
        }

        private void AddInternal(TKey key, TValue value, bool setIfExists = false)
        {
            // Find insertion point for key
            ArraySlice<int> slice = _pairs.Slice;
            int sliceIndex = ArrayExtensions.IndirectBinarySearch<TKey>(slice.Array, slice.Index, slice.Count, _column._keys, key, _column._keyComparer);

            // Ensure key not already found
            if (sliceIndex >= 0)
            {
                if (setIfExists)
                {
                    _column._values[slice.Array[sliceIndex]] = value;
                    return;
                }
                else
                {
                    throw new ArgumentException(nameof(key));
                }
            }

            // Add a new InnerRow for the new Key/Value pair
            int newInnerRow = _column._values.Count;
            _column._keys[newInnerRow] = key;
            _column._values[newInnerRow] = value;

            // Convert 'insert before' to a non-negative, relative index
            int insertAtIndex = (~sliceIndex) - slice.Index;
            _pairs.Insert(insertAtIndex, newInnerRow);
        }

        public void Clear()
        {
            NumberList<int> pairs = _pairs;

            // Clear Keys and Values to conserve space
            foreach (int innerRow in pairs)
            {
                _column._keys[innerRow] = default;
                _column._values[innerRow] = default;
            }

            // Clear pairs to empty the collection
            pairs.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return InnerRowOfKey(key) != -1;
        }

        public bool Remove(TKey key)
        {
            int innerRow = InnerRowOfKey(key);

            if (innerRow != -1)
            {
                _column._keys[innerRow] = default;
                _column._values[innerRow] = default;
                _pairs.Remove(innerRow);

                return true;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int innerRow = InnerRowOfKey(item.Key);

            if (innerRow != -1 && object.Equals(Value(innerRow), item.Value))
            {
                _column._keys[innerRow] = default;
                _column._values[innerRow] = default;
                _pairs.Remove(innerRow);

                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            int innerRow = InnerRowOfKey(key);
            if (innerRow == -1) { return false; }

            value = Value(innerRow);
            return true;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (TryGetValue(item.Key, out TValue foundValue))
            {
                return object.Equals(foundValue, item.Value);
            }

            return false;
        }

        private int InnerRowOfKey(TKey key)
        {
            ArraySlice<int> slice = _pairs.Slice;
            int keyIndex = ArrayExtensions.IndirectBinarySearch<TKey>(slice.Array, slice.Index, slice.Count, _column._keys, key, _column._keyComparer);
            return (keyIndex < 0 ? -1 : slice.Array[keyIndex]);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(_column, _pairs.Slice);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(_column, _pairs.Slice);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            // Combine Keys with XOR to ensure GetHashCode is order-independent
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                hashCode ^= pair.Key?.GetHashCode() ?? 0;
                hashCode ^= 31 * (pair.Value?.GetHashCode() ?? 0);
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            IDictionary<TKey, TValue> other = obj as IDictionary<TKey, TValue>;
            if (other == null) { return false; }

            // Verify Dictionary counts match
            if (this.Count != other.Count) { return false; }

            // Try to compare keys in order (faster than lookup for each key)
            using (IEnumerator<KeyValuePair<TKey, TValue>> thisEnumerator = GetEnumerator())
            using (IEnumerator<KeyValuePair<TKey, TValue>> otherEnumerator = other.GetEnumerator())
            {
                int countCompared = 0;

                while (thisEnumerator.MoveNext())
                {
                    // If other ran out of items first, definite non-match
                    if (!otherEnumerator.MoveNext()) { return false; }

                    KeyValuePair<TKey, TValue> thisPair = thisEnumerator.Current;
                    KeyValuePair<TKey, TValue> otherPair = otherEnumerator.Current;

                    // If keys don't match, they aren't in the same order; must fall back
                    if (!thisPair.Key.Equals(otherPair.Key)) { break; }

                    // If keys matched but values different, definite non-match
                    if (!object.Equals(thisPair.Value, otherPair.Value)) { return false; }

                    countCompared++;
                }

                // If we got fully through the lists, they match
                if (countCompared == this.Count)
                {
                    return true;
                }
            }

            // Otherwise, retrieve values by key (any order) and compare values)
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                if (!other.Contains(pair)) { return false; }
            }

            return true;
        }

        public static bool operator ==(ColumnDictionary<TKey, TValue> left, ColumnDictionary<TKey, TValue> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ColumnDictionary<TKey, TValue> left, ColumnDictionary<TKey, TValue> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
    }
}
