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
    public class NumberColumn<T> : IColumn<T> where T : unmanaged, IEquatable<T>
    {
        private const int MinimumSize = 32;
        private const string Array = nameof(Array);

        private T _defaultValue;
        private T[] _array;

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
                if (index >= Count)
                {
                    Count = index + 1;

                    // Don't resize for default values; the defaulting will respond correctly for them
                    if (_defaultValue.Equals(value)) { return; }
                }

                // Resize if required
                if (_array == null || _array.Length <= index)
                {
                    ResizeTo(index + 1);
                }

                _array[index] = value;
            }
        }

        public void Clear()
        {
            Count = 0;
            _array = null;
        }

        public void Trim()
        {
            // Nothing to do
        }

        private void ResizeTo(int size)
        {
            int currentLength = _array?.Length ?? 0;
            int newLength = Math.Max(MinimumSize, Math.Max(size, (currentLength + currentLength / 2)));
            T[] newArray = new T[newLength];

            if (currentLength > 0)
            {
                _array.CopyTo(newArray, 0);
            }

            for (int i = currentLength; i < newLength; ++i)
            {
                newArray[i] = _defaultValue;
            }

            _array = newArray;
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
            writer.WriteBlockArray(Array, _array, 0, Math.Min(Count, _array?.Length ?? 0));
            writer.WriteEndObject();
        }
    }
}
