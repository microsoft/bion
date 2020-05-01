using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  NullableColumn is used to track and return null values when the external
    ///  type of the column can be null, but the internally stored type doesn't have
    ///  a separate null representation and so won't round-trip null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableColumn<T> : IColumn<T> where T : class
    {
        private BooleanColumn _isNull;
        private IColumn<T> _inner;

        public NullableColumn(IColumn<T> inner)
        {
            // Default is Null
            _isNull = new BooleanColumn(true);
            _inner = inner;
        }

        public int Count => _inner.Count;
        public bool Empty => Count == 0;

        public T this[int index]
        {
            get
            {
                return (_isNull[index] ? null : _inner[index]);
            }

            set
            {
                _isNull[index] = (value == null);
                _inner[index] = value;
            }
        }

        public void Clear()
        {
            _isNull.Clear();
            _inner.Clear();
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public void Swap(int index1, int index2)
        {
            _isNull.Swap(index1, index2);
            _inner.Swap(index1, index2);
        }

        public void RemoveFromEnd(int count)
        {
            _isNull.RemoveFromEnd(count);
            _inner.RemoveFromEnd(count);
        }

        private const string Inner = nameof(Inner);
        private const string IsNull = nameof(IsNull);

        private static Dictionary<string, Setter<NullableColumn<T>>> setters = new Dictionary<string, Setter<NullableColumn<T>>>()
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

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }
    }
}
