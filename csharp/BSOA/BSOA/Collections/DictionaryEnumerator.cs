// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Column;

namespace BSOA.Collections
{
    public struct DictionaryEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        private DictionaryColumn<TKey, TValue> _column;
        private ArraySlice<int> _indices;
        private int _index;

        public DictionaryEnumerator(DictionaryColumn<TKey, TValue> column, ArraySlice<int> indices)
        {
            _column = column;
            _indices = indices;
            _index = -1;
        }

        public KeyValuePair<TKey, TValue> Current => CurrentValue();
        object IEnumerator.Current => CurrentValue();

        private KeyValuePair<TKey, TValue> CurrentValue()
        {
            int pairIndex = _indices[_index];
            return new KeyValuePair<TKey, TValue>(_column._keys[pairIndex], _column._values[pairIndex]);
        }

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
