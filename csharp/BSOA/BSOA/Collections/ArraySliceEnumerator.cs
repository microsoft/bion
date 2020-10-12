// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    /// <summary>
    ///  ArraySliceEnumerator provides optimized enumeration over ArraySlices.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public sealed class ArraySliceEnumerator<T> : IEnumerator<T> where T : unmanaged, IEquatable<T>
    {
        private T[] _array;
        private int _start;
        private int _end;

        private int _current;

        public ArraySliceEnumerator(ArraySlice<T> slice)
        {
            _array = slice.Array;
            _start = slice.Index;
            _end = _start + slice.Count;

            _current = _start - 1;
        }

        public T Current => _array[_current];
        object IEnumerator.Current => _array[_current];

        public bool MoveNext()
        {
            return ++_current < _end;
        }

        public void Reset()
        {
            _current = _start - 1;
        }

        public void Dispose()
        {
            // Nothing to Dispose
        }
    }
}
