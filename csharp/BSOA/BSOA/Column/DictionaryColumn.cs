// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    public class DictionaryColumn<TKey, TValue> : LimitedList<IDictionary<TKey, TValue>>, IColumn<IDictionary<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        internal IColumn<TKey> _keys;
        internal IColumn<TValue> _values;
        internal NullableColumn<NumberList<int>> _pairs;

        public DictionaryColumn(IColumn<TKey> keys, IColumn<TValue> values, bool nullByDefault = true)
        {
            _keys = keys;
            _values = values;
            _pairs = new NullableColumn<NumberList<int>>(new NumberListColumn<int>(), nullByDefault);
        }

        // ColumnFactory untyped constructor
        public DictionaryColumn(IColumn keys, IColumn values, object defaultValue) : this((IColumn<TKey>)keys, (IColumn<TValue>)values, (defaultValue == null))
        { }

        public override IDictionary<TKey, TValue> this[int index] 
        {
            get => ColumnDictionary<TKey, TValue>.Get(this, index);
            set => ColumnDictionary<TKey, TValue>.Set(this, index, value);
        }

        public override int Count => _pairs.Count;

        public override void Clear()
        {
            _keys.Clear();
            _values.Clear();
            _pairs.Clear();
        }

        public override void Swap(int index1, int index2)
        {
            _pairs.Swap(index1, index2);
        }

        public override void RemoveFromEnd(int count)
        {
            _pairs.RemoveFromEnd(count);
        }

        public void Trim()
        {
            _pairs.Trim();

            // Remove any keys and values which are no longer referenced
            GarbageCollector.Collect((INumberColumn<int>)_pairs.Values, _keys);
            GarbageCollector.Collect((INumberColumn<int>)_pairs.Values, _values);

            _keys.Trim();
            _values.Trim();
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.Keys, _keys);
            writer.Write(Names.Values, _values);
            writer.Write(Names.Pairs, _pairs);
            writer.WriteEndObject();
        }

        private static Dictionary<string, Setter<DictionaryColumn<TKey, TValue>>> setters = new Dictionary<string, Setter<DictionaryColumn<TKey, TValue>>>()
        {
            [Names.Keys] = (r, me) => me._keys.Read(r),
            [Names.Values] = (r, me) => me._values.Read(r),
            [Names.Pairs] = (r, me) => me._pairs.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }
    }
}
