using System;
using System.Collections.Generic;
using BSOA.Extensions;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  NumberColumn implements IColumn for built-in numeric types - byte, sbyte,
    ///  ushort, short, uint, int, ulong, long, float, double.
    /// </summary>
    /// <typeparam name="T">Numeric Type of column values</typeparam>
    public class NumberColumn<T> : LimitedList<T>, IColumn<T>, INumberColumn<T> where T : unmanaged
    {
        private T _defaultValue;
        private T[] _array;
        private int _count;
        private int UsedArrayLength => Math.Min(_array?.Length ?? 0, Count);

        public NumberColumn(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override int Count => _count;

        public override T this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }

                // Return the value (or defaultValue if the array is null or smaller than the requested index)
                return (_array?.Length > index ? _array[index] : _defaultValue);
            }

            set
            {
                // Track logical count
                if (index >= Count) { _count = index + 1; }

                // Resize if required
                if (_array == null || _array.Length <= index)
                {
                    // Don't resize for default values; the defaulting will respond correctly for them
                    if (_defaultValue.Equals(value)) { return; }

                    ArrayExtensions.ResizeTo(ref _array, index + 1, _defaultValue);
                }

                _array[index] = value;
            }
        }

        public ArraySlice<T> Slice => (UsedArrayLength == 0 ? ArraySlice<T>.Empty : new ArraySlice<T>(_array, index: 0, length: UsedArrayLength));

        public void ForEach(Action<ArraySlice<T>> action)
        {
            action(Slice);
        }

        public override void Clear()
        {
            _count = 0;
            _array = null;
        }

        public override void RemoveFromEnd(int count)
        {
            // Clear last 'count' values
            int length = UsedArrayLength;
            for (int i = Count - count; i < length; ++i)
            {
                _array[i] = _defaultValue;
            }

            // Track reduced size
            _count -= count;
        }

        public void Trim()
        {
            // Nothing to do
        }

        private static Dictionary<string, Setter<NumberColumn<T>>> setters = new Dictionary<string, Setter<NumberColumn<T>>>()
        {
            [Names.Count] = (r, me) => me._count = r.ReadAsInt32(),
            [Names.Array] = (r, me) => me._array = r.ReadBlockArray<T>()
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.Count, Count);
            writer.WriteBlockArray(Names.Array, _array, 0, UsedArrayLength);
            writer.WriteEndObject();
        }
    }
}
