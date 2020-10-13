// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  DictionaryColumn stores a Dictionary for each row.
    /// </summary>
    /// <remarks>
    ///  DictionaryColumn really stores a huge flat list of Key/Value pairs across every per-row Dictionary.
    ///  
    ///  To find the Key/Value pairs in one particular row's Dictionary, we get the 'Pairs' list.
    ///  Each integer in the pairs list is the index of a Key and Value in the Keys and Values column which matches
    ///  and which belongs to the outer row's Dictionary.
    /// </remarks>
    /// <typeparam name="TKey">Type of Dictionary entry keys</typeparam>
    /// <typeparam name="TValue">Type of Dictionary entry values</typeparam>
    public class DictionaryColumn<TKey, TValue> : LimitedList<IDictionary<TKey, TValue>>, IColumn<IDictionary<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        internal IColumn<TKey> _keys;
        internal IColumn<TValue> _values;
        internal IColumn<NumberList<int>> _pairs;
        private NumberListColumn<int> _pairsInner;

        public DictionaryColumn(IColumn<TKey> keys, IColumn<TValue> values, Nullability nullability = Nullability.DefaultToNull)
        {
            _keys = keys;
            _values = values;
            _pairsInner = new NumberListColumn<int>();
            _pairs = NullableColumn<NumberList<int>>.Wrap(_pairsInner, nullability);
        }

        // ColumnFactory untyped constructor
        public DictionaryColumn(IColumn keys, IColumn values, object defaultValue) : this((IColumn<TKey>)keys, (IColumn<TValue>)values, (defaultValue == null ? Nullability.DefaultToNull : Nullability.DefaultToEmpty))
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
            GarbageCollector.Collect(_pairsInner, _keys);
            GarbageCollector.Collect(_pairsInner, _values);

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
