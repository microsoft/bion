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
    ///  ArraySliceColumn stores an array of numbers for each row value.
    ///  ArraySlices are easily constructed around arrays, but are immutable.
    ///  Columns using values which only change via replacement should use ArraySliceColumn. (Strings are byte arrays which only are replaced)
    ///  Columns which are Lists of numbers which are mutable should use NumberListColumn.
    ///  Columns which are Lists of other types should use ListColumn.
    ///  Values under length 2,048 are packed together to avoid per row overhead.
    /// </summary>
    /// <remarks>
    ///  ArraySliceColumn is broken into 'Chapters' which are broken into 'Pages'.
    ///  Short values are written back to back in a shared array.
    ///  Long values are stored individually and loaded into a dictionary.
    ///  Each 'Chapter' has a separate shared array, so that the array doesn't get too large.
    ///  Each 'Page' has a tracked starting position in the shared array.
    ///  Each row records the page-relative start position of the value only.
    /// </remarks>
    /// <typeparam name="T">Type of each element of Values (for StringColumn, T is char)</typeparam>
    public class ArraySliceColumn<T> : LimitedList<ArraySlice<T>>, IColumn<ArraySlice<T>>, INumberColumn<T> where T : unmanaged, IEquatable<T>
    {
        private int _count;
        private List<ArraySliceChapter<T>> _chapters;

        public override int Count => _count;

        public ArraySliceColumn()
        {
            Clear();
        }

        public override ArraySlice<T> this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                if (index >= Count) { return ArraySlice<T>.Empty; }

                int chapterIndex = index / ArraySliceChapter<T>.ChapterRowCount;
                int indexInChapter = index % ArraySliceChapter<T>.ChapterRowCount;

                return _chapters[chapterIndex][indexInChapter];
            }

            set
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                int chapterIndex = index / ArraySliceChapter<T>.ChapterRowCount;
                int indexInChapter = index % ArraySliceChapter<T>.ChapterRowCount;

                if (index >= Count)
                {
                    _count = index + 1;

                    if (_chapters == null) { _chapters = new List<ArraySliceChapter<T>>(); }
                    while (chapterIndex >= _chapters.Count)
                    {
                        _chapters.Add(new ArraySliceChapter<T>());
                    }
                }

                _chapters[chapterIndex][indexInChapter] = value;
            }
        }

        public override void Clear()
        {
            _count = 0;
            _chapters = null;
        }

        public override void RemoveFromEnd(int count)
        {
            // Clear last 'count' values
            for (int i = Count - count; i < Count; ++i)
            {
                this[i] = ArraySlice<T>.Empty;
            }

            int newLastIndex = ((Count - 1) - count);
            int newLastChapter = newLastIndex / ArraySliceChapter<T>.ChapterRowCount;
            int newLastIndexInChapter = newLastIndex % ArraySliceChapter<T>.ChapterRowCount;

            // Cut length of last chapter
            if (_chapters.Count > newLastChapter)
            {
                _chapters[newLastChapter].Count = newLastIndexInChapter + 1;
            }

            // Remove any now-empty chapters
            if (_chapters.Count > newLastChapter + 1)
            {
                _chapters.RemoveRange(newLastChapter + 1, _chapters.Count - (newLastChapter + 1));
            }

            // Track reduced size
            _count -= count;
        }

        public void ForEach(Action<ArraySlice<T>> action)
        {
            if (_chapters != null)
            {
                foreach (ArraySliceChapter<T> chapter in _chapters)
                {
                    chapter.ForEach(action);
                }
            }
        }

        public void Trim()
        {
            if (_chapters != null)
            {
                foreach (ArraySliceChapter<T> chapter in _chapters)
                {
                    chapter.Trim();
                }
            }
        }

        public void Read(ITreeReader reader)
        {
            _chapters = reader.ReadList<ArraySliceChapter<T>>(() => new ArraySliceChapter<T>());

            int chapterCount = _chapters?.Count ?? 0;
            if (chapterCount > 0)
            {
                // Infer count; N - 1 full chapters and the actual row count from the last
                _count = ArraySliceChapter<T>.ChapterRowCount * (chapterCount - 1) + _chapters[chapterCount - 1].Count;
            }
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteList(_chapters);
        }
    }
}
