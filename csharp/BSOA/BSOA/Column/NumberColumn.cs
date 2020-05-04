using System;
using System.Collections;
using System.Collections.Generic;
using BSOA.Extensions;
using BSOA.IO;

namespace BSOA.Column
{
    /// <summary>
    ///  NumberColumn implements IColumn for built-in numeric types - byte, sbyte,
    ///  ushort, short, uint, int, ulong, long, float, double.
    /// </summary>
    /// <typeparam name="T">Numeric Type of column values</typeparam>
    public class NumberColumn<T> : IColumn<T> where T : unmanaged, IEquatable<T>, IComparable<T>
    {
        private const string Array = nameof(Array);
        private T _defaultValue;
        private T[] _array;
        private int UsedArrayLength => Math.Min(_array?.Length ?? 0, Count);

        /// <summary>
        ///  Build a NumberColumn with the given default value.
        /// </summary>
        /// <param name="defaultValue">Value unset rows should return</param>
        public NumberColumn(T defaultValue)
        {
            _defaultValue = defaultValue;
        }

        /// <summary>
        ///  Return the current valid count for the column.
        ///  This is (index + 1) for the highest non-default value set.
        /// </summary>
        public int Count { get; private set; }
        public bool Empty => Count == 0;

        /// <summary>
        ///  Get or Set the value at a given index
        /// </summary>
        /// <param name="index">Index of value to set</param>
        /// <returns>Value at index</returns>
        public T this[int index]
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
                if (index >= Count) { Count = index + 1; }

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

        public void Clear()
        {
            Count = 0;
            _array = null;
        }

        public void Trim()
        {
            // Nothing to do
        }

        public void RemoveFromEnd(int count)
        {
            // Clear last 'count' values
            int length = UsedArrayLength;
            for (int i = Count - count; i < length; ++i)
            {
                _array[i] = _defaultValue;
            }

            // Track reduced size
            Count -= count;
        }

        public void Swap(int index1, int index2)
        {
            T item = this[index1];
            this[index1] = this[index2];
            this[index2] = item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        private static Dictionary<string, Setter<NumberColumn<T>>> setters = new Dictionary<string, Setter<NumberColumn<T>>>()
        {
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [Array] = (r, me) => me._array = r.ReadBlockArray<T>()
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(nameof(Count), Count);
            writer.WriteBlockArray(Array, _array, 0, UsedArrayLength);
            writer.WriteEndObject();
        }
    }
}
