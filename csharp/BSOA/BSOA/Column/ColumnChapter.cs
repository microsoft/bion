using BSOA.Extensions;
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
    internal class ColumnChapter<T> where T : unmanaged
    {
        public const int ChapterRowCount = 1024;
        public const int PageRowCount = 32;
        public const int MaximumSmallValueLength = 2047;

        private int[] _pageStartInChapter;      // Position of each Page Start relative to Chapter
        private ushort[] _valueEndInPage;       // Position after end of each Value relative to Page

        private T[] _smallValueArray;
        private Dictionary<int, ArraySlice<T>> _largeValueDictionary;

        private bool _requiresTrim;

        public int Count { get; set; }

        public ColumnChapter()
        {
            Count = 0;
        }

        private int EndPosition(int index)
        {
            return _pageStartInChapter[index / PageRowCount] + _valueEndInPage[index];
        }

        private int StartPosition(int index)
        {
            return (index == 0 ? 0 : EndPosition(index - 1));
        }

        public ArraySlice<T> this[int index]
        {
            get
            {
                // VariableLengthColumn can't pass out of range values
                // if (index < 0 || index >= ChapterRowCount) { throw new ArgumentOutOfRangeException("index"); }

                ArraySlice<T> result = default;
                if (_largeValueDictionary != null && _largeValueDictionary.TryGetValue(index, out result)) { return result; }

                if (index < _valueEndInPage?.Length)
                {
                    int position = StartPosition(index);
                    int length = EndPosition(index) - position;
                    return new ArraySlice<T>(_smallValueArray, position, length);
                }
                else
                {
                    return ArraySlice<T>.Empty;
                }
            }

            set
            {
                if (index >= Count) { Count = index + 1; }
                _largeValueDictionary = _largeValueDictionary ?? new Dictionary<int, ArraySlice<T>>();
                _largeValueDictionary[index] = value;
                _requiresTrim |= (value.Count <= MaximumSmallValueLength);
            }
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _pageStartInChapter = reader.ReadBlockArray<int>(ref buffer);
            _valueEndInPage = reader.ReadBlockArray<ushort>(ref buffer);
            _smallValueArray = reader.ReadBlockArray<T>(ref buffer);

            Count = _valueEndInPage.Length;

            _largeValueDictionary = new Dictionary<int, ArraySlice<T>>();

            int[] largeValueIndices = reader.ReadBlockArray<int>(ref buffer);
            if (largeValueIndices.Length > 0)
            {
                for (int i = 0; i < largeValueIndices.Length; ++i)
                {
                    T[] largeValue = reader.ReadBlockArray<T>(ref buffer);
                    _largeValueDictionary[largeValueIndices[i]] = new ArraySlice<T>(largeValue);
                }
            }
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            // Merge changed small values under cutoff into SmallValueArray
            Trim();

            writer.WriteBlockArray(_pageStartInChapter, ref buffer);
            writer.WriteBlockArray(_valueEndInPage, 0, Count, ref buffer);
            writer.WriteBlockArray(_smallValueArray, ref buffer);

            int[] largeValueKeys = _largeValueDictionary?.Keys.ToArray();
            writer.WriteBlockArray(largeValueKeys, ref buffer);

            if (largeValueKeys != null)
            {
                for (int i = 0; i < largeValueKeys.Length; ++i)
                {
                    _largeValueDictionary[largeValueKeys[i]].Write(writer, ref buffer);
                }
            }
        }

        public void Trim()
        {
            if (_requiresTrim == false) { return; }

            // Compute new size needed for SmallValueArray
            int totalSmallValueLength = _smallValueArray?.Length ?? 0;
            int newSmallValueLength = totalSmallValueLength;

            foreach (var pair in _largeValueDictionary)
            {
                int length = pair.Value.Count;
                if (length <= MaximumSmallValueLength)
                {
                    int index = pair.Key;
                    int oldLength = ((index < _valueEndInPage?.Length) ? EndPosition(index) - StartPosition(index) : 0);
                    newSmallValueLength += (length - oldLength);
                }
            }

            // Make new arrays
            T[] newSmallValueArray = new T[newSmallValueLength];
            int[] newPageStartInChapter = new int[(Count / PageRowCount) + 1];
            ushort[] newValueEndInPage = new ushort[Count];

            // Copy every small-enough value to the new array and remove from LargeValueDictionary
            int currentPageStart = 0;
            int nextIndex = 0;
            for (int i = 0; i < Count; ++i)
            {
                // Set new page start for each page
                if ((i % PageRowCount) == 0)
                {
                    currentPageStart = nextIndex;
                    newPageStartInChapter[i / PageRowCount] = currentPageStart;
                }

                // Get current value
                ArraySlice<T> value = this[i];

                // Copy the value to the new _smallValueArray, if it fits
                if (value.Count <= MaximumSmallValueLength)
                {
                    value.CopyTo(newSmallValueArray, nextIndex);
                    nextIndex += value.Count;
                    _largeValueDictionary.Remove(i);
                }

                // Set new valueEnd
                newValueEndInPage[i] = (ushort)(nextIndex - currentPageStart);
            }

            if (_largeValueDictionary.Count == 0) { _largeValueDictionary = null; }
            _smallValueArray = newSmallValueArray;
            _pageStartInChapter = newPageStartInChapter;
            _valueEndInPage = newValueEndInPage;
            
            _requiresTrim = false;
        }
    }
}
