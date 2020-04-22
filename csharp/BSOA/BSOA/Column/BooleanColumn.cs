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
        private const uint FirstBit = 0x1U << 31;
        private const string Inner = nameof(Inner);
        private NumberColumn<uint> _innerColumn;

        /// <summary>
        ///  Build a BooleanColumn with the given default value.
        /// </summary>
        /// <param name="defaultValue">Value unset rows should return</param>
        public BooleanColumn(bool defaultValue)
        {
            uint defaultNumber = (defaultValue ? ~0U : 0U);
            _innerColumn = new NumberColumn<uint>(defaultNumber);
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
        public bool this[int index]
        {
            // Check or set the bit in the right uint (each one holds 32 bits)
            get { return (_innerColumn[index >> 5] & (FirstBit >> (index & 31))) != 0UL; }
            set
            {
                if (index >= Count) { Count = index + 1; }

                if (value)
                {
                    _innerColumn[index >> 5] |= (FirstBit >> (index & 31));
                }
                else
                {
                    _innerColumn[index >> 5] &= ~(FirstBit >> (index & 31));
                }
            }
        }

        public void Clear()
        {
            Count = 0;
            _innerColumn.Clear();
        }

        public void Trim()
        {
            // Nothing to do
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return new ListEnumerator<bool>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<bool>(this);
        }

        private static Dictionary<string, Setter<BooleanColumn>> setters = new Dictionary<string, Setter<BooleanColumn>>()
        {
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [Inner] = (r, me) => me._innerColumn.Read(r)
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            writer.Write(nameof(Count), Count);

            writer.WritePropertyName(Inner);
            _innerColumn.Write(writer);

            writer.WriteEndObject();
        }
    }
}
