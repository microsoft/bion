using BSOA.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BSOA.Column
{
    public class StringColumn : IColumn<string>
    {
        private BooleanColumn _isNull;
        private NumberListColumn<byte> _inner;

        public StringColumn()
        {
            // Default is Null
            _isNull = new BooleanColumn(true);
            _inner = new NumberListColumn<byte>();
        }

        public int Count => _inner.Count;
        public bool Empty => Count == 0;

        public string this[int index]
        {
            get
            {
                if (_isNull[index]) { return null; }

                ArraySlice<byte> value = _inner[index];
                return (value.Count == 0 ? string.Empty : Encoding.UTF8.GetString(value.Array, value.Index, value.Count));
            }

            set
            {
                if (value == null)
                {
                    _isNull[index] = true;
                    _inner[index] = ArraySlice<byte>.Empty;
                }
                else
                {
                    _isNull[index] = false;
                    _inner[index] = (value.Length == 0 ? ArraySlice<byte>.Empty : new ArraySlice<byte>(Encoding.UTF8.GetBytes(value)));
                }
            }
        }

        public void Clear()
        {
            _isNull.Clear();
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

        private const string Inner = nameof(Inner);
        private const string IsNull = nameof(IsNull);

        private static Dictionary<string, Setter<StringColumn>> setters = new Dictionary<string, Setter<StringColumn>>()
        {
            [IsNull] = (r, me) => me._isNull.Read(r),
            [Inner] = (r, me) => me._inner.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(IsNull, _isNull);
            writer.Write(Inner, _inner);
            writer.WriteEndObject();
        }
    }
}
