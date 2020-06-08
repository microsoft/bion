using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    public struct ListEnumerator<T> : IEnumerator<T>
    {
        private IReadOnlyList<T> _list;
        private int _index;

        public ListEnumerator(IReadOnlyList<T> column)
        {
            _list = column;
            _index = -1;
        }

        public T Current => _list[_index];
        object IEnumerator.Current => _list[_index];

        public void Dispose()
        {
            // Nothing to Dispose
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _list.Count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
