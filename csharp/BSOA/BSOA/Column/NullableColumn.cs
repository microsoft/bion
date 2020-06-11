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
        protected BooleanColumn IsNull;
        protected IColumn<T> Values;

        public NullableColumn(IColumn<T> values)
        {
            // Default is Null
            IsNull = new BooleanColumn(true);
            Values = values;
        }

        public override int Count => Values.Count;

        public override T this[int index]
        {
            get
            {
                return (IsNull[index] ? null : Values[index]);
            }

            set
            {
                IsNull[index] = (value == null);
                Values[index] = value;
            }
        }

        public override void Clear()
        {
            IsNull.Clear();
            Values.Clear();
        }

        public override void Swap(int index1, int index2)
        {
            IsNull.Swap(index1, index2);
            Values.Swap(index1, index2);
        }

        public override void RemoveFromEnd(int count)
        {
            IsNull.RemoveFromEnd(count);
            Values.RemoveFromEnd(count);
        }

        public void Trim()
        {
            Values.Trim();
        }

        private static Dictionary<string, Setter<NullableColumn<T>>> setters = new Dictionary<string, Setter<NullableColumn<T>>>()
        {
            [Names.IsNull] = (r, me) => me.IsNull.Read(r),
            [Names.Values] = (r, me) => me.Values.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.IsNull, IsNull);
            writer.Write(Names.Values, Values);
            writer.WriteEndObject();
        }
    }
}
