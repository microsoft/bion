// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;

using BSOA.IO;

namespace BSOA.Column
{
    public class ListColumn<T> : IColumn<MutableSliceWrapper<T, ListColumn<T>>>
    {
        private VariableLengthColumn<int> _indices;
        private IColumn<T> _values;

        public int Count => _indices.Count;
        public bool Empty => Count == 0;

        public ListColumn(IColumn<T> valuesColumn)
        {
            _indices = new VariableLengthColumn<int>();
            _values = valuesColumn;
        }

        public MutableSliceWrapper<T, ListColumn<T>> this[int index]
        {
            get => new MutableSliceWrapper<T, ListColumn<T>>(new MutableSlice<int>(_indices, index), this, ValueFromIndex, IndexFromValue);
            set => new MutableSliceWrapper<T, ListColumn<T>>(new MutableSlice<int>(_indices, index), this, ValueFromIndex, IndexFromValue).SetTo(value);
        }

        private static T ValueFromIndex(ListColumn<T> column, int valueIndex)
        {
            return column._values[valueIndex];
        }

        private static int IndexFromValue(ListColumn<T> column, T value)
        {
            // If we know the old index, we can overwrite the value there.

            // Issue: We add to the values column every time a set happens
            int newValueIndex = column._values.Count;
            column._values[newValueIndex] = value;
            return newValueIndex;
        }

        public IEnumerator<MutableSliceWrapper<T, ListColumn<T>>> GetEnumerator()
        {
            return new ListEnumerator<MutableSliceWrapper<T, ListColumn<T>>>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<MutableSliceWrapper<T, ListColumn<T>>>(this);
        }

        public void Trim()
        {
            // TODO: Walk for unreferenced values

            _indices.Trim();
            _values.Trim();
        }

        public void Clear()
        {
            _indices.Clear();
            _values.Clear();
        }

        private const string Indices = nameof(Indices);
        private const string Values = nameof(Values);

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Indices, _indices);
            writer.Write(Values, _values);
            writer.WriteEndObject();
        }

        private static Dictionary<string, Setter<ListColumn<T>>> setters = new Dictionary<string, Setter<ListColumn<T>>>()
        {
            [Indices] = (r, me) => me._indices.Read(r),
            [Values] = (r, me) => me._values.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }
    }
}
