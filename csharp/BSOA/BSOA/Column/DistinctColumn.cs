using BSOA.IO;
using BSOA.Model;
using System.Collections.Generic;
using System.Linq;

namespace BSOA.Column
{
    /// <summary>
    ///  DistinctColumn compresses data by storing each distinct value of the column
    ///  only once. It reverts to storing values individually if there are too many
    ///  distinct values.
    /// </summary>
    /// <typeparam name="T">Type of values in column</typeparam>
    public class DistinctColumn<T> : LimitedList<T>, IColumn<T>
    {
        private T _defaultValue;
        private Dictionary<T, byte> _distinct;
        private IColumn<T> _values;
        private NumberColumn<byte> _indices;
        private bool _requiresTrim;

        public DistinctColumn(IColumn<T> values, T defaultValue)
        {
            _defaultValue = defaultValue;
            _values = values;
            Clear();
        }

        public bool IsMappingValues => (_distinct != null);
        public int DistinctCount => (IsMappingValues ? _distinct.Count + 1 : -1);
        public override int Count => (IsMappingValues ? _indices.Count : _values.Count);

        public override T this[int index]
        {
            get => (IsMappingValues ? _values[_indices[index]] : _values[index]);

            set
            {
                if (IsMappingValues && TryGetDistinctIndex(value, out byte valueIndex))
                {
                    _requiresTrim = true;
                    _indices[index] = valueIndex;
                }
                else
                {
                    _values[index] = value;
                }
            }
        }

        private bool TryGetDistinctIndex(T value, out byte index)
        {
            index = 0;
            if (value == null) { return _defaultValue == null; }
            if (_defaultValue != null && value.Equals(_defaultValue)) { return true; }

            if (_distinct.TryGetValue(value, out index))
            {
                // Existing value - return current index
                return true;
            }
            else if (DistinctCount <= 256)
            {
                // New value, count still ok - add and return new index
                index = (byte)(_distinct.Count + 1);
                _distinct[value] = index;
                _values[index] = value;
                return true;
            }
            else
            {
                // Too many values - convert to per-value
                List<T> distinctValues = _values.ToList();
                _values.Clear();
                for (int i = 0; i < _indices.Count; ++i)
                {
                    _values[i] = distinctValues[_indices[i]];
                }

                _indices = null;
                _distinct = null;
                _requiresTrim = false;
                return false;
            }
        }

        public override void Clear()
        {
            // Indices empty but non-null
            _indices = new NumberColumn<byte>(0);

            // One distinct value; the default
            _distinct = new Dictionary<T, byte>();
            _values.Clear();
            _values[0] = _defaultValue;

            _requiresTrim = false;
        }

        public override void RemoveFromEnd(int count)
        {
            if (IsMappingValues)
            {
                _indices.RemoveFromEnd(count);
            }
            else
            {
                _values.RemoveFromEnd(count);
            }

            _requiresTrim = true;
        }

        public void Trim()
        {
            if (IsMappingValues)
            {
                if (_requiresTrim == false) { return; }
                _requiresTrim = false;

                // Remove unused values *except the default*, which must stay as '0' so new rows in indices have the default value
                BitVector unusedValues = new BitVector(true, DistinctCount);
                unusedValues.Remove(0);

                bool wasRemapped = GarbageCollector.Collect<byte, T>(_indices, _values, unusedValues);
                if (wasRemapped)
                {
                    RebuildDistinctDictionary();
                }
            }
            else
            {
                _values.Trim();
            }
        }

        private void RebuildDistinctDictionary()
        {
            _distinct.Clear();

            for (int i = 1; i < _values.Count; ++i)
            {
                _distinct[_values[i]] = (byte)i;
            }
        }

        private static Dictionary<string, Setter<DistinctColumn<T>>> setters = new Dictionary<string, Setter<DistinctColumn<T>>>()
        {
            [Names.Indices] = (r, me) => me._indices.Read(r),
            [Names.Values] = (r, me) => me._values.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);

            if (_indices.Count > 0)
            {
                // If column has indices, it's mapped; reconstruct distinct dictionary
                RebuildDistinctDictionary();
            }
            else if (_values.Count > 1)
            {
                // If it has no indices and more than one value (the default is always added), it is non-mapped
                _indices = null;
                _distinct = null;
            }
        }

        public void Write(ITreeWriter writer)
        {
            Trim();

            writer.WriteStartObject();
            writer.Write(Names.Indices, _indices);
            writer.Write(Names.Values, _values);
            writer.WriteEndObject();
        }
    }
}
