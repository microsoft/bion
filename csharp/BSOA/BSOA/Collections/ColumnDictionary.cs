using BSOA.Column;
using BSOA.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    public readonly struct ColumnDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private readonly DictionaryColumn<TKey, TValue> _column;
        private readonly int _rowIndex;

        public static ColumnDictionary<TKey, TValue> Empty = new ColumnDictionary<TKey, TValue>();

        public ColumnDictionary(DictionaryColumn<TKey, TValue> column, int index)
        {
            if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
            _column = column;
            _rowIndex = index;
        }

        private NumberList<int> Pairs => _column?._pairs[_rowIndex] ?? NumberList<int>.Empty;
        private TValue Value(int pairIndex) => _column._values[_column._pairs[_rowIndex][pairIndex]];

        public bool IsReadOnly => false;
        public int Count => Pairs.Count;

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
                int index = InternalIndexOfKey(key);
                if (index == -1)
                {
                    Add(key, value);
                }
                else
                {
                    _column._values[index] = value;
                }
            }
        }

        public ICollection<TKey> Keys => new IndirectCollection<TKey>(_column._keys, Pairs.Slice);
        public ICollection<TValue> Values => new IndirectCollection<TValue>(_column._values, Pairs.Slice);

        public void SetTo(IDictionary<TKey, TValue> other)
        {
            Clear();

            foreach (KeyValuePair<TKey, TValue> pair in other)
            {
                this.Add(pair);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (this.ContainsKey(key)) { throw new ArgumentException(nameof(key)); }

            int newPairIndex = _column.Count;
            _column._keys[newPairIndex] = key;
            _column._values[newPairIndex] = value;
            Pairs.Add(newPairIndex);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Pairs.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return InternalIndexOfKey(key) != -1;
        }

        public bool Remove(TKey key)
        {
            int pairIndex = InternalIndexOfKey(key);

            if (pairIndex != -1)
            {
                Pairs.RemoveAt(pairIndex);
                return true;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int pairIndex = InternalIndexOfKey(item.Key);

            if (pairIndex != -1 && object.Equals(Value(pairIndex), item.Value))
            {
                Pairs.RemoveAt(pairIndex);
                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            int pairIndex = InternalIndexOfKey(key);
            if (pairIndex == -1) { return false; }

            value = Value(pairIndex);
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

        private int InternalIndexOfKey(TKey key)
        {
            int pairIndex = 0;

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                if (pair.Key.Equals(key)) { return pairIndex; }
                pairIndex++;
            }

            return -1;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(_column, Pairs.Slice);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(_column, Pairs.Slice);
        }
    }
}
