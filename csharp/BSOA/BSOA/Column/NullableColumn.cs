// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.IO;
using BSOA.Model;

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
        internal IColumn<T> Values;

        public NullableColumn(IColumn<T> values, bool nullByDefault)
        {
            IsNull = new BooleanColumn(nullByDefault);
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

            if (IsNull.Count == 0 && Values.Count > 0)
            {
                // Only wrote values means all values are non-null
                IsNull[Values.Count - 1] = false;
                IsNull.SetAll(false);
            }
            else if (IsNull.Count > 0 && Values.Count == 0)
            {
                // Only wrote nulls means all values are null
                Values[IsNull.Count - 1] = default(T);
            }
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            if (Count > 0)
            {
                int nullValueCount = IsNull.CountTrue;

                if (nullValueCount == Count)
                {
                    // If all null, write IsNull only (default is already all null)
                    writer.Write(Names.IsNull, IsNull);
                }
                else if (nullValueCount == 0)
                {
                    // If no nulls, write values only (will infer situation on read)
                    writer.Write(Names.Values, Values);
                }
                else
                {
                    // If there are some nulls and some values, we must write both
                    writer.Write(Names.IsNull, IsNull);
                    writer.Write(Names.Values, Values);
                }
            }

            writer.WriteEndObject();
        }
    }
}
