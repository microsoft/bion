using BSOA.Model;

using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    public struct IndirectEnumerator<T> : IEnumerator<T>
    {
        private IColumn<T> _values;
        private ArraySlice<int> _indices;
        private int _index;

        public IndirectEnumerator(IColumn<T> values, ArraySlice<int> indices)
        {
            _values = values;
            _indices = indices;
            _index = -1;
        }

        public T Current => _values[_indices[_index]];
        object IEnumerator.Current => _values[_indices[_index]];

        public void Dispose()
        {
            // Nothing to Dispose
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _indices.Count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
