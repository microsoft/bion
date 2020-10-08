// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    public struct ListEnumerator<T> : IEnumerator<T>
    {
        private IReadOnlyList<T> _list;
        private int _index;
        private int _count;

        public ListEnumerator(IReadOnlyList<T> column)
        {
            _list = column;
            _index = -1;
            _count = _list.Count;
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
            return _index < _count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
