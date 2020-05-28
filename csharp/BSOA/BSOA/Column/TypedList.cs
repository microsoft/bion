using BSOA.Extensions;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  TypedList wraps a NumberList and converts it from the internal int (an index of some entity)
    ///  to instances of the entity type for external use.
    /// </summary>
    public class TypedList<TItem> : IList<TItem>, IReadOnlyList<TItem>
    {
        private NumberList<int> _inner;
        private Func<int, TItem> _toInstance;
        private Func<TItem, int> _toIndex;

        public TypedList(NumberList<int> indices, Func<int, TItem> toInstance, Func<TItem, int> toIndex)
        {
            _inner = indices;
            _toInstance = toInstance;
            _toIndex = toIndex;
        }

        public TItem this[int index]
        {
            get => _toInstance(_inner[index]);
            set => _inner[index] = _toIndex(value);
        }

        public NumberList<int> Indices => _inner;

        public int Count => _inner.Count;
        public bool IsReadOnly => false;

        public void SetTo(IList<TItem> list)
        {
            _inner.Clear();

            if (list?.Count > 0)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    _inner.Add(_toIndex(list[i]));
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
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (arrayIndex + Count > array.Length) { throw new ArgumentException(nameof(arrayIndex)); }

            for (int i = 0; i < Count; ++i)
            {
                array[i + arrayIndex] = _toInstance(_inner[i]);
            }
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
            return (left == null ? right == null : left.Equals(right));
        }

        public static bool operator !=(TypedList<TItem> left, TypedList<TItem> right)
        {
            return (left == null ? right != null : !(left.Equals(right)));
        }
    }
}
