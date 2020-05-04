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
        private IColumn<T> _values;

        public NullableColumn(IColumn<T> values)
        {
            // Default is Null
            _isNull = new BooleanColumn(true);
            _values = values;
        }

        public int Count => _values.Count;
        public bool Empty => Count == 0;

        public T this[int index]
        {
            get
            {
                return (_isNull[index] ? null : _values[index]);
            }

            set
            {
                _isNull[index] = (value == null);
                _values[index] = value;
            }
        }

        public void Clear()
        {
            _isNull.Clear();
            _values.Clear();
        }

        public void Trim()
        {
            _values.Trim();
        }

        public void Swap(int index1, int index2)
        {
            _isNull.Swap(index1, index2);
            _values.Swap(index1, index2);
        }

        public void RemoveFromEnd(int count)
        {
            _isNull.RemoveFromEnd(count);
            _values.RemoveFromEnd(count);
        }

        private static Dictionary<string, Setter<NullableColumn<T>>> setters = new Dictionary<string, Setter<NullableColumn<T>>>()
        {
            [Names.IsNull] = (r, me) => me._isNull.Read(r),
            [Names.Values] = (r, me) => me._values.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.IsNull, _isNull);
            writer.Write(Names.Values, _values);
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
