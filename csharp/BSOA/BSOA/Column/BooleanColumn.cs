using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  BooleanColumn implements IColumn for bool on top of a NumberColumn&lt;uint&gt;
    /// </summary>
    public class BooleanColumn : IColumn<bool>
    {
        private BitVector _vector;

        /// <summary>
        ///  Build a BooleanColumn with the given default value.
        /// </summary>
        /// <param name="defaultValue">Value unset rows should return</param>
        public BooleanColumn(bool defaultValue)
        {
            _vector = new BitVector(defaultValue, 0);
        }

        /// <summary>
        ///  Return the current valid count for the column.
        ///  This is (index + 1) for the highest non-default value set.
        /// </summary>
        public int Count => _vector.Capacity;
        public bool Empty => Count == 0;

        /// <summary>
        ///  Get or Set the value at a given index
        /// </summary>
        /// <param name="index">Index of value to set</param>
        /// <returns>Value at index</returns>
        public bool this[int index]
        {
            // Check or set the bit in the right uint (each one holds 32 bits)
            get { return _vector[index]; }
            set { _vector[index] = value; }
        }

        public void Trim()
        {
            // Nothing to do
        }

        public void Clear()
        {
            _vector.Clear();
        }

        public void RemoveFromEnd(int count)
        {
            _vector.RemoveFromEnd(count);
        }

        public void Swap(int index1, int index2)
        {
            bool item = this[index1];
            this[index1] = this[index2];
            this[index2] = item;
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return new ListEnumerator<bool>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<bool>(this);
        }

        public void Read(ITreeReader reader)
        {
            _vector.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _vector.Write(writer);
        }
    }
}
