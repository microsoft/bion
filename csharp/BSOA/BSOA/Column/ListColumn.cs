// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.Collections;
using BSOA.GC;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  ListColumn provides a full IList&lt;T&gt; for each row.
    ///  It wraps a values column which contains the individual values
    ///  and an ArraySliceColumn which identifies which values each row-list has.
    /// </summary>
    public class ListColumn<T> : LimitedList<IList<T>>, IColumn<IList<T>>
    {
        private ColumnList<T> _cached;
        private NumberListColumn<int> _indicesInner;
        internal IColumn<NumberList<int>> _indices;
        internal IColumn<T> _values;

        public override int Count => _indices.Count;

        public ListColumn(IColumn<T> values, Nullability nullability = Nullability.DefaultToNull)
        {
            _indicesInner = new NumberListColumn<int>();
            _indices = NullableColumn<NumberList<int>>.Wrap(_indicesInner, nullability);
            _values = values;
        }

        // ColumnFactory untyped constructor
        public ListColumn(IColumn values, object defaultValue) : this((IColumn<T>)values, (defaultValue == null ? Nullability.DefaultToNull : Nullability.DefaultToEmpty))
        { }

        public override IList<T> this[int index]
        {
            get
            {
                ColumnList<T> value = _cached;
                if (value?._rowIndex != index)
                {
                    value = ColumnList<T>.Get(this, index);
                    _cached = value;
                }

                return value;
            }

            set
            {
                _cached = default;
                ColumnList<T>.Set(this, index, value);
            }
        }

        public override void Swap(int index1, int index2)
        {
            _cached = default;
            _indices.Swap(index1, index2);
        }

        public override void RemoveFromEnd(int count)
        {
            _cached = default;
            _indices.RemoveFromEnd(count);
        }

        public override void Clear()
        {
            _cached = default;
            _indices.Clear();
            _values.Clear();
        }

        public void Trim()
        {
            // Trim indices first to consolidate references before garbage collection
            _indices.Trim();

            // Find any unused values and remove them
            GarbageCollector.Collect<int, T>(_indicesInner, _values);

            // Trim values afterward to clean up any newly unused space
            _values.Trim();
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.Indices, _indices);
            writer.Write(Names.Values, _values);
            writer.WriteEndObject();
        }

        private static Dictionary<string, Setter<ListColumn<T>>> setters = new Dictionary<string, Setter<ListColumn<T>>>()
        {
            [Names.Indices] = (r, me) => me._indices.Read(r),
            [Names.Values] = (r, me) => me._values.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }
    }
}