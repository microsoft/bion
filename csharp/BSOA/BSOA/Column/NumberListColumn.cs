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
        private List<ColumnChapter<T>> _chapters;

        public int Count { get; private set; }
        public bool Empty => Count == 0;

        public NumberListColumn()
        {
            Clear();
        }

        public void Clear()
        {
            Count = 0;
            _chapters = new List<ColumnChapter<T>>();
        }

        public ArraySlice<T> this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                if (index >= Count) { return ArraySlice<T>.Empty; }

                int chapterIndex = index / ColumnChapter<T>.ChapterRowCount;
                int indexInChapter = index % ColumnChapter<T>.ChapterRowCount;

                return _chapters[chapterIndex][indexInChapter];
            }

            set
            {
                int chapterIndex = index / ColumnChapter<T>.ChapterRowCount;
                int indexInChapter = index % ColumnChapter<T>.ChapterRowCount;

                if (index >= Count) { Count = index + 1; }

                if (value.Count == 0 && chapterIndex >= _chapters.Count) { return; }

                while (chapterIndex >= _chapters.Count)
                {
                    _chapters.Add(new ColumnChapter<T>());
                }

                _chapters[chapterIndex][indexInChapter] = value;
            }
        }

        public void Trim()
        {
            foreach (ColumnChapter<T> chapter in _chapters)
            {
                chapter.Trim();
            }
        }

        public IEnumerator<ArraySlice<T>> GetEnumerator()
        {
            return new ListEnumerator<ArraySlice<T>>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<ArraySlice<T>>(this);
        }

        private const string Chapters = nameof(Chapters);
        private static Dictionary<string, Setter<NumberListColumn<T>>> setters = new Dictionary<string, Setter<NumberListColumn<T>>>()
        {
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [nameof(Chapters)] = (r, me) => me._chapters = r.ReadList<ColumnChapter<T>>(() => new ColumnChapter<T>())
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(nameof(Count), Count);

            writer.WritePropertyName(nameof(Chapters));
            writer.WriteList(_chapters);

            writer.WriteEndObject();
        }
    }
}
