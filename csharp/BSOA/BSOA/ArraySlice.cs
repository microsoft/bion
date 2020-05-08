using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    public struct ArraySlice<T> : IReadOnlyList<T>, ITreeSerializable where T : unmanaged
    {
        public T[] Array { get; private set; }
        public int Index { get; private set; }
        public int Count { get; set; }
        public bool IsExpandable { get; private set; }

        public T this[int index] => Array[Index + index];

        public static ArraySlice<T> Empty = default;

        public ArraySlice(T[] array, int index = 0, int length = -1, bool isExpandable = false)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (length < 0) { length = array.Length - index; }
            if (index < 0 || index > array.Length) { throw new ArgumentOutOfRangeException("index"); }
            if (index + length > array.Length) { throw new ArgumentOutOfRangeException("length"); }
            
            Array = array;
            Index = index;
            Count = length;
            IsExpandable = isExpandable;
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
            if (Count > 0)
            {
                System.Array.Copy(Array, Index, other, toIndex, Count);
            }
        }

        public void Trim()
        { }

        public void Clear()
        {
            Array = null;
            Index = 0;
            Count = 0;
            IsExpandable = false;
        }

        public void Read(ITreeReader reader)
        {
            Array = reader.ReadBlockArray<T>();
            Index = 0;
            Count = Array.Length;
            IsExpandable = false;
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteBlockArray(Array, Index, Count);
        }
    }
}