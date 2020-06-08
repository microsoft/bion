using BSOA.Extensions;
using BSOA.Model;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    public struct IndirectCollection<T> : IReadOnlyList<T>, IReadOnlyCollection<T>, ICollection<T>
    {
        private IColumn<T> _values;
        private ArraySlice<int> _indices;

        public IndirectCollection(IColumn<T> values, ArraySlice<int> indices)
        {
            _values = values;
            _indices = indices;
        }

        public bool IsReadOnly => true;
        public int Count => _indices.Count;

        public T this[int index] => _values[_indices[index]];

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            foreach (T value in this)
            {
                if (object.Equals(item, value)) { return true; }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new IndirectEnumerator<T>(_values, _indices);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new IndirectEnumerator<T>(_values, _indices);
        }
    }
}
