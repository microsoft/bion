using BSOA.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BSOA
{
    public class StringColumn : IColumn<string>
    {
        private VariableLengthColumn<char> _inner;
        
        public StringColumn()
        {
            _inner = new VariableLengthColumn<char>();
        }

        public int Count => _inner.Count;

        public string this[int index] 
        {
            get
            {
                ArraySlice<char> value = _inner[index];
                return (value.Count == 0 ? string.Empty : new string(value._array, value._index, value.Count));
            }

            set => _inner[index] = new ArraySlice<char>(value.ToCharArray());
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new ListEnumerator<string>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<string>(this);
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _inner.Read(reader, ref buffer);
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            _inner.Write(writer, ref buffer);
        }

        public void Read(ITreeReader reader)
        {
            _inner.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _inner.Write(writer);
        }
    }
}
