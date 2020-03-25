using BSOA.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BSOA
{
    /// <summary>
    ///  Contains one 'Chapter' (1,024 rows) of values for a VariableLengthColumn.
    /// </summary>
    /// <remarks>
    ///  Values with length &lt; 2,048 are stored back-to-back together in a SmallValueArray.
    ///  Longer values are stored in a LargeValueArray and loaded individually into a Dictionary.
    ///  Changed values are put in the LargeValueDictionary, keeping logic simple and the SmallValueArray unchanged.
    ///  
    ///  With a 2 KB short value limit,
    ///   a 32 row page is under 64 KB, and page-relative row positions can be a ushort.
    ///   a 1,024 row chapter is under 2,048 KB, and chapter-relative positions fit in an int.
    ///   
    ///  The overall column uses longs for large values and chapter positions, allowing the column to be over 4 GB.
    /// </remarks>
    /// <typeparam name="T">Type of each part of Values (if each value is a string, this type is char)</typeparam>
    internal class ColumnChapter<T> : IColumn<ArraySlice<T>> where T : unmanaged
    {
        public const int ChapterRowCount = 1024;
        public const int PageRowCount = 32;
        public const int MaximumSmallValueLength = 2047;

        private int[] _pageStartInChapter;      // Position of each Page Start relative to Chapter
        private ushort[] _valueEndInPage;       // Position after end of each Value relative to Page

        private T[] _smallValueArray;
        private Dictionary<int, ArraySlice<T>> _largeValueDictionary;

        public int Count { get; set; }

        public ColumnChapter()
        {
            Count = 0;

            _pageStartInChapter = new int[ChapterRowCount / PageRowCount];
            _valueEndInPage = new ushort[ChapterRowCount];
        }

        public ArraySlice<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= ChapterRowCount) { throw new ArgumentOutOfRangeException("index"); }
                if (index >= Count) { return ArraySlice<T>.Empty; }

                ArraySlice<T> result = default;
                if (_largeValueDictionary != null && _largeValueDictionary.TryGetValue(index, out result)) { return result; }

                int pageIndex = index / PageRowCount;

                int pagePosition = _pageStartInChapter[pageIndex];
                int startInPage = (index == 0 ? 0 : _valueEndInPage[index - 1]);
                int endInPage = _valueEndInPage[index];
                int length = endInPage - startInPage;

                return new ArraySlice<T>(_smallValueArray, pagePosition + startInPage, length);
            }

            set
            {
                if (index >= Count) { Count = index + 1; }
                _largeValueDictionary = _largeValueDictionary ?? new Dictionary<int, ArraySlice<T>>();
                _largeValueDictionary[index] = value;
            }
        }

        public void Trim()
        {
            if (_largeValueDictionary != null)
            {
                // Compute new size needed for SmallValueArray
                int totalSmallValueLength = _pageStartInChapter[(Count - 1) / PageRowCount] + _valueEndInPage[Count - 1];
                int newSmallValueLength = totalSmallValueLength;

                foreach (var pair in _largeValueDictionary)
                {
                    int length = pair.Value.Count;
                    if (length < MaximumSmallValueLength)
                    {
                        int index = pair.Key;
                        int oldLength = _valueEndInPage[index] - (index == 0 ? 0 : _valueEndInPage[index - 1]);
                        newSmallValueLength += (length - oldLength);
                    }
                }

                // Make a new SmallValueArray
                T[] newSmallValueArray = new T[newSmallValueLength];

                // Copy every small-enough value to the new array and remove from LargeValueDictionary
                int nextIndex = 0;
                for (int i = 0; i < Count; ++i)
                {
                    ArraySlice<T> value = this[i];
                    if (value.Count < MaximumSmallValueLength)
                    {
                        value.CopyTo(newSmallValueArray, nextIndex);
                        nextIndex += value.Count;
                        _largeValueDictionary.Remove(i);
                    }
                }

                _smallValueArray = newSmallValueArray;
            }
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _pageStartInChapter = reader.ReadArray<int>(ref buffer);
            _valueEndInPage = reader.ReadArray<ushort>(ref buffer);
            _smallValueArray = reader.ReadArray<T>(ref buffer);

            Count = _valueEndInPage.Length;

            _largeValueDictionary = new Dictionary<int, ArraySlice<T>>();

            int[] largeValueIndices = reader.ReadArray<int>(ref buffer);
            if (largeValueIndices.Length > 0)
            {
                for (int i = 0; i < largeValueIndices.Length; ++i)
                {
                    T[] largeValue = reader.ReadArray<T>(ref buffer);
                    _largeValueDictionary[largeValueIndices[i]] = new ArraySlice<T>(largeValue);
                }
            }
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            // Merge changed small values under cutoff into SmallValueArray
            Trim();

            writer.WriteArray(_pageStartInChapter, ref buffer);
            writer.WriteArray(_valueEndInPage, 0, Count, ref buffer);
            writer.WriteArray(_smallValueArray, ref buffer);

            int[] largeValueKeys = _largeValueDictionary.Keys.ToArray();
            writer.WriteArray(largeValueKeys, ref buffer);

            for (int i = 0; i < largeValueKeys.Length; ++i)
            {
                _largeValueDictionary[i].Write(writer, ref buffer);
            }
        }
    }
}
