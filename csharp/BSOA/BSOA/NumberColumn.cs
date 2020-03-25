using System;
using System.IO;

using BSOA.Extensions;

namespace BSOA
{
    /// <summary>
    ///  NumberColumn implements IColumn for built-in numeric types - byte, sbyte,
    ///  ushort, short, uint, int, ulong, long, float, double.
    /// </summary>
    /// <typeparam name="T">Numeric Type of column values</typeparam>
    public class NumberColumn<T> : IColumn<T> where T : unmanaged, IEquatable<T>
    {
        private const int MinimumSize = 32;

        private T _defaultValue;
        private T[] _array;
        private int _count;

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
        public int Count => _count;

        /// <summary>
        ///  Get or Set the value at a given index
        /// </summary>
        /// <param name="index">Index of value to set</param>
        /// <returns>Value at index</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException("index"); }

                // Return the value (or defaultValue if the array is null or smaller than the requested index)
                return (_array?.Length > index ? _array[index] : _defaultValue);
            }

            set
            {
                if (index >= _count)
                {
                    // Don't resize for default values; the defaulting will respond correctly for them
                    if (_defaultValue.Equals(value)) { return; }

                    // Resize if required
                    ResizeTo(index + 1);
                }

                _array[index] = value;
            }
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _array = reader.ReadArray<T>(ref buffer);
            _count = _array.Length;
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            writer.WriteArray(_array, 0, _count, ref buffer);
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
            _count = size;
        }
    }
}
