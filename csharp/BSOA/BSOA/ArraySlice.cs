using BSOA.Extensions;
using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BSOA
{
    public struct ArraySlice<T> : IReadOnlyList<T>, IBinarySerializable where T : unmanaged
    {
        private T[] _array;
        private int _index;
        private int _length;

        public T this[int index] => _array[_index + index];
        public int Count => _length;

        public static ArraySlice<T> Empty = default;

        public ArraySlice(T[] array, int index = 0, int length = -1)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (length < 0) { length = array.Length - index; }
            if (index < 0 || index >= array.Length) { throw new ArgumentOutOfRangeException("index"); }
            if (index + length > array.Length) { throw new ArgumentOutOfRangeException("length"); }
            
            _array = array;
            _index = index;
            _length = length;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArraySliceEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ArraySliceEnumerator<T>(this);
        }
        
        public void CopyTo(T[] other, int toIndex)
        {
            if (_length > 0)
            {
                Array.Copy(_array, _index, other, toIndex, _length);
            }
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _array = reader.ReadArray<T>(ref buffer);
            _index = 0;
            _length = _array.Length;
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            writer.WriteArray<T>(_array, _index, _length, ref buffer);
        }
    }

    public struct ArraySliceEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        private ArraySlice<T> _slice;
        private int _index;

        public ArraySliceEnumerator(ArraySlice<T> slice)
        {
            _slice = slice;
            _index = -1;
        }

        public T Current => _slice[_index];
        object IEnumerator.Current => _slice[_index];

        public void Dispose()
        {
            // Nothing to Dispose
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _slice.Count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
