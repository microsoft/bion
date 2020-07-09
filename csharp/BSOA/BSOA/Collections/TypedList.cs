// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Extensions;
using BSOA.Model;

namespace BSOA.Collections
{
    /// <summary>
    ///  TypedList wraps a NumberList and converts it from the internal int (an index of some entity)
    ///  to instances of the entity type for external use.
    /// </summary>
    public class TypedList<TItem> : IList<TItem>, IReadOnlyList<TItem> where TItem: IRow<TItem>
    {
        private readonly NumberList<int> _inner;
        private readonly Func<int, TItem> _toInstance;
        private readonly Func<TItem, int> _toIndex;

        private TypedList(NumberList<int> indices, Func<int, TItem> toInstance, Func<TItem, int> toIndex)
        {
            _inner = indices;
            _toInstance = toInstance;
            _toIndex = toIndex;
        }

        public static TypedList<TItem> Get(Table<TItem> table, IColumn<NumberList<int>> column, int index)
        {
            NumberList<int> indices = column?[index];
            return (indices == null ? null : new TypedList<TItem>(indices, (i) => table.Get(i), (v) => table.LocalIndex(v)));
        }

        public static void Set(Table<TItem> table, IColumn<NumberList<int>> column, int index, ICollection<TItem> toValue)
        {
            if (toValue == null)
            {
                column[index] = null;
            }
            else if (toValue.Count == 0)
            {
                column[index] = NumberList<int>.Empty;
            }
            else
            {
                int[] indices = new int[toValue.Count];
                int i = 0;
                foreach (TItem value in toValue)
                {
                    indices[i++] = table.LocalIndex(value);
                }

                NumberList<int> current = column[index];

                // Setting to empty coerces list creation in correct column
                if (current == null)
                {
                    column[index] = NumberList<int>.Empty;
                    current = column[index];
                }

                current.SetTo(new ArraySlice<int>(indices));
            }
        }

        public TItem this[int index]
        {
            get => _toInstance(_inner[index]);
            set => _inner[index] = _toIndex(value);
        }

        public NumberList<int> Indices => _inner;

        public int Count => _inner?.Count ?? 0;
        public bool IsReadOnly => false;

        public void SetTo(IEnumerable<TItem> list)
        {
            if (list is TypedList<TItem>)
            {
                // Avoid Clear() on SetTo(self)
                if (_inner.Equals(((TypedList<TItem>)list)._inner)) { return; }
            }

            _inner.Clear();

            if (list != null)
            {
                foreach (TItem item in list)
                {
                    _inner.Add(_toIndex(item));
                }
            }
        }

        public void Add(TItem item)
        {
            _inner.Add(_toIndex(item));
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(TItem item)
        {
            return _inner.Contains(_toIndex(item));
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public int IndexOf(TItem item)
        {
            return _inner.IndexOf(_toIndex(item));
        }

        public void Insert(int index, TItem item)
        {
            _inner.Insert(index, _toIndex(item));
        }

        public bool Remove(TItem item)
        {
            return _inner.Remove(_toIndex(item));
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

        public override int GetHashCode()
        {
            return ReadOnlyListExtensions.GetHashCode(this);
        }

        public override bool Equals(object obj)
        {
            return ReadOnlyListExtensions.AreEqual(this, obj as IReadOnlyList<TItem>);
        }

        public static bool operator ==(TypedList<TItem> left, TypedList<TItem> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(TypedList<TItem> left, TypedList<TItem> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
    }
}
