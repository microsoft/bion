using System;
using System.Collections;
using System.Collections.Generic;

namespace Bion.Text
{
    public class String8Set : ICollection<String8>
    {
        private List<int> _indices;
        private byte[] _bytes;
        private int _bytesUsed;

        public String8Set(int stringCountCapacity = 16, int stringBytesCapacity = -1)
        {
            _indices = new List<int>(stringCountCapacity + 1);
            _indices.Add(0);

            if (stringBytesCapacity > 0)
            {
                _bytes = new byte[stringBytesCapacity];
            }

            _bytesUsed = 0;
        }

        public String8 this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) { throw new IndexOutOfRangeException(); }

                int start = _indices[index];
                int end = _indices[index + 1];
                return String8.Reference(_bytes, start, end - start);
            }
        }

        public int Count { get; private set; }
        public int LengthBytes => _bytesUsed;
        public bool IsReadOnly => false;

        public void Add(String8 item)
        {
            if (_bytes == null || _bytes.Length < _bytesUsed + item.Length)
            {
                int newLength = Math.Max(_bytesUsed + item.Length, _bytesUsed + _bytesUsed / 4);

                byte[] newArray = new byte[newLength];
                if (_bytesUsed > 0) { Buffer.BlockCopy(_bytes, 0, newArray, 0, _bytesUsed); }

                _bytes = newArray;
            }

            item.CopyTo(_bytes, _bytesUsed);
            _bytesUsed += item.Length;

            Count++;
            _indices.Add(_bytesUsed);
        }

        public bool Remove(String8 item)
        {
            int index = IndexOf(item);
            if (index == -1) { return false; }

            Remove(index);
            return true;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= Count) { throw new ArgumentOutOfRangeException("index"); }

            int start = _indices[index];
            int end = _indices[index + 1];
            int length = end - start;

            // Shift bytes back
            Buffer.BlockCopy(_bytes, end, _bytes, start, _bytesUsed - end);

            // Shift all indices back and overwrite index of item
            for (int i = index; i < Count; ++i)
            {
                _indices[i] = _indices[i + 1] - length;
            }

            _indices.RemoveAt(Count);
            Count--;
            _bytesUsed -= length;
        }

        public void Clear()
        {
            _bytesUsed = 0;
            Count = 0;
        }

        public int IndexOf(String8 item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (item.Equals(this[i])) { return i; }
            }

            return -1;
        }

        public bool Contains(String8 item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(String8[] array, int arrayIndex)
        {
            if(array == null) { throw new ArgumentNullException("array"); }
            if(arrayIndex < 0 || arrayIndex >= array.Length) { throw new ArgumentOutOfRangeException("arrayIndex"); }

            for(int i = 0; i < Count; ++i)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<String8> GetEnumerator()
        {
            return new String8SetEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new String8SetEnumerator(this);
        }

        private class String8SetEnumerator : IEnumerator<String8>
        {
            private String8Set _set;
            private int _index;
            
            public String8SetEnumerator(String8Set set)
            {
                _set = set;
                _index = -1;
            }

            public String8 Current => _set[_index];
            object IEnumerator.Current => _set[_index];

            public bool MoveNext()
            {
                _index++;
                return _index < _set.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
                // Nothing to do
            }
        }
    }
}
