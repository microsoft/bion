using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BSOA
{
    /// <summary>
    ///  VariableLengthColumn is a column for all types of varying lengths (strings, references, binary).
    ///  Each value in the column is a T[]. Values under length 2,048 are packed together to avoid per row overhead.
    /// </summary>
    /// <typeparam name="T">Type of each element of Values (for StringColumn, T is char)</typeparam>
    public class VariableLengthColumn<T> : IColumn<ArraySlice<T>> where T : unmanaged
    {
        private List<ColumnChapter<T>> _chapters;

        public int Count { get; private set; }

        public VariableLengthColumn()
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

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _chapters.Clear();

            Count = reader.ReadInt32();

            int chapterCount = reader.ReadInt32();
            for (int i = 0; i < chapterCount; ++i)
            {
                ColumnChapter<T> chapter = new ColumnChapter<T>();
                chapter.Read(reader, ref buffer);

                _chapters.Add(chapter);
            }
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            writer.Write(Count);
            writer.Write(_chapters.Count);

            foreach (ColumnChapter<T> chapter in _chapters)
            {
                chapter.Write(writer, ref buffer);
            }
        }

        private const string Chapters = nameof(Chapters);
        private static Dictionary<string, Setter<VariableLengthColumn<T>>> setters = new Dictionary<string, Setter<VariableLengthColumn<T>>>()
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
