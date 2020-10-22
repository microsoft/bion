// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  StringColumn stores strings as UTF-8.
    /// </summary>
    public class StringColumn : LimitedList<string>, IColumn<string>, INumberColumn<byte>
    {
        private BooleanColumn IsNull;
        private ArraySliceColumn<byte> Values;

        // Accumulate changed values and convert into UTF-8 in a shared array
        // to reduce array overhead and allocation time per set value.
        private const int SavedValueCountLimit = 128;
        private Dictionary<int, string> _savedValues;

        // Cache the most recently read value in .NET string form.
        // This helps in common situations (repeated reads) with minimal impact in others.
        // More complex caching mechanisms cost more than the string conversions unless the usage pattern is ideal.
        private CacheItem<string> _cache;

        public StringColumn()
        {
            // Members initialized just-in-time to save unused StringColumn space
        }

        public override int Count => IsNull?.Count ?? 0;

        public override string this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }

                CacheItem<string> item = _cache;
                if (item?.RowIndex == index) { return item.Value; }

                if (_savedValues != null && _savedValues.TryGetValue(index, out string result)) { return result; }
                if (IsNull == null || IsNull[index]) { return null; }

                ArraySlice<byte> bytes = Values[index];
                item = new CacheItem<string>(index, (bytes.Count == 0 ? string.Empty : Encoding.UTF8.GetString(bytes.Array, bytes.Index, bytes.Count)));

                _cache = item;

                return item.Value;
            }

            set
            {
                _cache = default;

                // Always set IsNull; IsNull tracks Count cheaply
                Init();
                IsNull[index] = (value == null);

                int length = value?.Length ?? 0;
                if (length == 0 || length > ArraySliceChapter<byte>.MaximumSmallValueLength)
                {
                    // Directly set null, empty, and long values
                    _savedValues?.Remove(index);
                    Values[index] = (string.IsNullOrEmpty(value) ? ArraySlice<byte>.Empty : new ArraySlice<byte>(Encoding.UTF8.GetBytes(value)));
                }
                else
                {
                    // Cache other values to convert together
                    if (_savedValues == null) { _savedValues = new Dictionary<int, string>(); }
                    _savedValues[index] = value;

                    if (_savedValues.Count >= SavedValueCountLimit) { PushSavedValues(); }
                }
            }
        }

        private void Init()
        {
            if (IsNull == null)
            {
                IsNull = new BooleanColumn(true);
                Values = new ArraySliceColumn<byte>();
            }
        }

        private void PushSavedValues()
        {
            if (_savedValues?.Count > 0)
            {
                Init();

                // Find combined UTF-8 length of pending values
                int totalLength = 0;
                foreach (KeyValuePair<int, string> pair in _savedValues)
                {
                    totalLength += Encoding.UTF8.GetByteCount(pair.Value);
                }

                // Allocate a shared array
                byte[] sharedArray = new byte[totalLength];

                // Convert and set on inner column
                int thisStart = 0;
                foreach (KeyValuePair<int, string> pair in _savedValues)
                {
                    int thisLength = Encoding.UTF8.GetBytes(pair.Value, 0, pair.Value.Length, sharedArray, thisStart);
                    Values[pair.Key] = new ArraySlice<byte>(sharedArray, thisStart, thisLength);

                    thisStart += thisLength;
                }

                _savedValues.Clear();
            }
        }

        public void ForEach(Action<ArraySlice<byte>> action)
        {
            PushSavedValues();
            Values?.ForEach(action);
        }

        public override void Clear()
        {
            _cache = default;
            _savedValues = null;

            IsNull = null;
            Values = null;
        }

        public override void RemoveFromEnd(int count)
        {
            PushSavedValues();
            IsNull?.RemoveFromEnd(count);
            Values?.RemoveFromEnd(count);
        }

        public void Trim()
        {
            PushSavedValues();
            Values?.Trim();
        }

        private static Dictionary<string, Setter<StringColumn>> setters = new Dictionary<string, Setter<StringColumn>>()
        {
            [Names.IsNull] = (r, me) => { me.Init(); me.IsNull.Read(r); },
            [Names.Values] = (r, me) => { me.Init(); me.Values.Read(r); }
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);

            if (IsNull != null)
            {
                if (IsNull.Count == 0 && Values.Count > 0)
                {
                    // Only wrote values means all values are non-null
                    IsNull[Values.Count - 1] = false;
                    IsNull.SetAll(false);
                }
                else if (IsNull.Count > 0 && Values.Count == 0)
                {
                    // Only wrote nulls means all values are null
                    Values[IsNull.Count - 1] = ArraySlice<byte>.Empty;
                }
            }
        }

        public void Write(ITreeWriter writer)
        {
            Trim();

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

