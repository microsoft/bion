using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    public struct ArraySlice<T> : IReadOnlyList<T>, ITreeSerializable where T : unmanaged
    {
        internal T[] _array;
        internal int _index;
        private int _length;

        public T this[int index] => _array[_index + index];
        public int Count => _length;

        public static ArraySlice<T> Empty = default;

        public ArraySlice(T[] array, int index = 0, int length = -1)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (length < 0) { length = array.Length - index; }
            if (index < 0 || index > array.Length) { throw new ArgumentOutOfRangeException("index"); }
            if (index + length > array.Length) { throw new ArgumentOutOfRangeException("length"); }
            
            _array = array;
            _index = index;
            _length = length;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }
        
        public void CopyTo(T[] other, int toIndex)
        {
            if (_length > 0)
            {
                Array.Copy(_array, _index, other, toIndex, _length);
            }
        }

        public void Read(ITreeReader reader)
        {
            _array = reader.ReadBlockArray<T>();
            _index = 0;
            _length = _array.Length;
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteBlockArray(_array, _index, _length);
        }
    }
}
