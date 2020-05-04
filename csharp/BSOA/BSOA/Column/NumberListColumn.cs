using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  NumberListColumn stores an array of numbers for each row value.
    ///  Columns which are lists of primitive types should use NumberListColumn directly.
    ///  Other Lists should use ListColumn.
    ///  Values under length 2,048 are packed together to avoid per row overhead.
    /// </summary>
    /// <remarks>
    ///  NumberListColumn is broken into 'Chapters' which are broken into 'Pages'.
    ///  Short values are written back to back in a shared array.
    ///  Long values are stored individually and loaded into a dictionary.
    ///  Each 'Chapter' has a separate shared array, so that the array doesn't get too large.
    ///  Each 'Page' has a tracked starting position in the shared array.
    ///  Each row records the page-relative start position of the value only.
    /// </remarks>
    /// <typeparam name="T">Type of each element of Values (for StringColumn, T is char)</typeparam>
    public class NumberListColumn<T> : IColumn<ArraySlice<T>> where T : unmanaged
    {
        private List<NumberListChapter<T>> _chapters;

        public int Count { get; private set; }
        public bool Empty => Count == 0;

        public NumberListColumn()
        {
            Clear();
        }

        public void Clear()
        {
            Count = 0;
            _chapters = new List<NumberListChapter<T>>();
        }

        public ArraySlice<T> this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                if (index >= Count) { return ArraySlice<T>.Empty; }

                int chapterIndex = index / NumberListChapter<T>.ChapterRowCount;
                int indexInChapter = index % NumberListChapter<T>.ChapterRowCount;

                return _chapters[chapterIndex][indexInChapter];
            }

            set
            {
                int chapterIndex = index / NumberListChapter<T>.ChapterRowCount;
                int indexInChapter = index % NumberListChapter<T>.ChapterRowCount;

                if (index >= Count) { Count = index + 1; }

                if (value.Count == 0 && chapterIndex >= _chapters.Count) { return; }

                while (chapterIndex >= _chapters.Count)
                {
                    _chapters.Add(new NumberListChapter<T>());
                }

                _chapters[chapterIndex][indexInChapter] = value;
            }
        }

        public void Trim()
        {
            foreach (NumberListChapter<T> chapter in _chapters)
            {
                chapter.Trim();
            }
        }

        public void RemoveFromEnd(int count)
        {
            // Clear last 'count' values
            for (int i = Count - count; i < Count; ++i)
            {
                this[i] = ArraySlice<T>.Empty;
            }

            int newLastIndex = ((Count - 1) - count);
            int newLastChapter = newLastIndex / NumberListChapter<T>.ChapterRowCount;
            int newLastIndexInChapter = newLastIndex % NumberListChapter<T>.ChapterRowCount;

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
            Count -= count;
        }

        public void Swap(int index1, int index2)
        {
            // Swapping slices avoids copies by just having the rows refer to 
            // the array slices one another were already using. 
            ArraySlice<T> item = this[index1];
            this[index1] = this[index2];
            this[index2] = item;
        }

        public IEnumerator<ArraySlice<T>> GetEnumerator()
        {
            return new ListEnumerator<ArraySlice<T>>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<ArraySlice<T>>(this);
        }

        private static Dictionary<string, Setter<NumberListColumn<T>>> setters = new Dictionary<string, Setter<NumberListColumn<T>>>()
        {
            [Names.Count] = (r, me) => me.Count = r.ReadAsInt32(),
            [Names.Chapters] = (r, me) => me._chapters = r.ReadList<NumberListChapter<T>>(() => new NumberListChapter<T>())
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(Names.Count, Count);

            writer.WritePropertyName(Names.Chapters);
            writer.WriteList(_chapters);

            writer.WriteEndObject();
        }
    }
}
