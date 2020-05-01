using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  Discouraged: Use StringColumn instead.
    ///  String16Column stores strings as UTF-16 chars, like .NET.
    ///  It uses twice the memory, and is kept to compare conversion speed with the normal UTF-8-based form.
    /// </summary>
    public class String16Column : IColumn<string>
    {
        private BooleanColumn _isNull;
        private NumberListColumn<char> _inner;

        public String16Column()
        {
            // Default is Null
            _isNull = new BooleanColumn(true);
            _inner = new NumberListColumn<char>();
        }

        public int Count => _inner.Count;
        public bool Empty => Count == 0;

        public string this[int index]
        {
            get
            {
                if (_isNull[index]) { return null; }

                ArraySlice<char> value = _inner[index];
                return (value.Count == 0 ? string.Empty : new string(value.Array, value.Index, value.Count));
            }

            set
            {
                if (value == null)
                {
                    _isNull[index] = true;
                    _inner[index] = ArraySlice<char>.Empty;
                }
                else
                {
                    _isNull[index] = false;
                    _inner[index] = new ArraySlice<char>(value.ToCharArray());
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

        private static Dictionary<string, Setter<String16Column>> setters = new Dictionary<string, Setter<String16Column>>()
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
