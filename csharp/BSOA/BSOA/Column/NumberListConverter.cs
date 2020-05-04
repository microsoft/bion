﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  NumberListConverter wraps a NumberList and converts it from the internal int (an index of some entity)
    ///  to instances of the entity type for external use.
    /// </summary>
    public struct NumberListConverter<TItem, TTable> : IList<TItem>, IReadOnlyList<TItem>
    {
        private NumberList<int> _inner;
        private TTable _table;
        private Func<TTable, int, TItem> _toInstance;
        private Func<TTable, TItem, int> _toIndex;

        public NumberListConverter(NumberList<int> indices, TTable table, Func<TTable, int, TItem> toInstance, Func<TTable, TItem, int> toIndex)
        {
            _inner = indices;
            _table = table;
            _toInstance = toInstance;
            _toIndex = toIndex;
        }

        public TItem this[int index]
        {
            get => _toInstance(_table, _inner[index]);
            set => _inner[index] = _toIndex(_table, value);
        }

        public NumberList<int> Indices => _inner;

        public int Count => _inner.Count;
        public bool IsReadOnly => false;

        public void SetTo(IList<TItem> list)
        {
            // TODO: Cross-database copying. Can ITable do it via per-column copy?
            if (list is NumberListConverter<TItem, TTable>)
            {
                NumberListConverter<TItem, TTable> other = (NumberListConverter<TItem, TTable>)list;
                _inner.SetTo(other.Indices.Slice);
            }
            else
            {
                _inner.Clear();

                if (list?.Count > 0)
                {
                    for (int i = 0; i < list.Count; ++i)
                    {
                        _inner.Add(_toIndex(_table, list[i]));
                    }
                }
            }
        }

        public void Add(TItem item)
        {
            _inner.Add(_toIndex(_table, item));
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(TItem item)
        {
            return _inner.Contains(_toIndex(_table, item));
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (arrayIndex + Count > array.Length) { throw new ArgumentException(nameof(arrayIndex)); }

            for (int i = 0; i < Count; ++i)
            {
                array[i + arrayIndex] = _toInstance(_table, _inner[i]);
            }
        }

        public int IndexOf(TItem item)
        {
            return _inner.IndexOf(_toIndex(_table, item));
        }

        public void Insert(int index, TItem item)
        {
            _inner.Insert(index, _toIndex(_table, item));
        }

        public bool Remove(TItem item)
        {
            return _inner.Remove(_toIndex(_table, item));
        }

        public void RemoveAt(int index)
        {
            _inner.RemoveAt(index);
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return new ListEnumerator<TItem>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<TItem>(this);
        }
    }
}