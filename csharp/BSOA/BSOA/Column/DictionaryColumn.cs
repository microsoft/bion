using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Column
{
    public class DictionaryColumn<TKey, TValue> : LimitedList<IDictionary<TKey, TValue>>, IColumn<IDictionary<TKey, TValue>> where TKey : IEquatable<TKey>
    {
        internal IColumn<TKey> _keys;
        internal IColumn<TValue> _values;
        internal NumberListColumn<int> _pairs;

        public DictionaryColumn(IColumn<TKey> keys, IColumn<TValue> values)
        {
            _keys = keys;
            _values = values;
            _pairs = new NumberListColumn<int>();
        }

        public override IDictionary<TKey, TValue> this[int index] 
        {
            get => new ColumnDictionary<TKey, TValue>(this, index);
            set => new ColumnDictionary<TKey, TValue>(this, index).SetTo(value);
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
            GarbageCollector.Collect(_pairs, _keys);
            GarbageCollector.Collect(_pairs, _values);

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
