using BSOA.IO;
using BSOA.Model;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  NullableColumn is used to track and return null values when the external
    ///  type of the column can be null, but the internally stored type doesn't have
    ///  a separate null representation and so won't round-trip null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableColumn<T> : LimitedList<T>, IColumn<T> where T : class
    {
        private BooleanColumn _isNull;
        private IColumn<T> _values;

        public NullableColumn(IColumn<T> values)
        {
            // Default is Null
            _isNull = new BooleanColumn(true);
            _values = values;
        }

        public override int Count => _values.Count;

        public override T this[int index]
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

        public override void Clear()
        {
            _isNull.Clear();
            _values.Clear();
        }

        public override void Swap(int index1, int index2)
        {
            _isNull.Swap(index1, index2);
            _values.Swap(index1, index2);
        }

        public override void RemoveFromEnd(int count)
        {
            _isNull.RemoveFromEnd(count);
            _values.RemoveFromEnd(count);
        }

        public void Trim()
        {
            _values.Trim();
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
    }
}
