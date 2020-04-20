using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BSOA
{
    /// <summary>
    ///  DateTimeColumn implements IColumn for DateTime on top of a NumberColumn&lt;long&gt;
    /// </summary>
    public class DateTimeColumn : IColumn<DateTime>
    {
        private NumberColumn<long> _innerColumn;

        /// <summary>
        ///  Build a DateTime with the given default value.
        /// </summary>
        /// <param name="defaultValue">Value unset rows should return</param>
        public DateTimeColumn(DateTime defaultValue)
        {
            _innerColumn = new NumberColumn<long>(defaultValue.ToUniversalTime().Ticks);
        }

        /// <summary>
        ///  Return the current valid count for the column.
        ///  This is (index + 1) for the highest non-default value set.
        /// </summary>
        public int Count => _innerColumn.Count;

        /// <summary>
        ///  Get or Set the value at a given index
        /// </summary>
        /// <param name="index">Index of value to set</param>
        /// <returns>Value at index</returns>
        public DateTime this[int index]
        {
            get { return new DateTime(_innerColumn[index], DateTimeKind.Utc); }
            set { _innerColumn[index] = value.ToUniversalTime().Ticks; }
        }

        public void Clear()
        {
            _innerColumn.Clear();
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return new ListEnumerator<DateTime>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<DateTime>(this);
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            _innerColumn.Read(reader, ref buffer);
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            _innerColumn.Write(writer, ref buffer);
        }

        public void Read(ITreeReader reader)
        {
            _innerColumn.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _innerColumn.Write(writer);
        }
    }
}
