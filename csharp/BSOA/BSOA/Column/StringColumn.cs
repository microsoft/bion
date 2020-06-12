using BSOA.Collections;

using System.Text;

namespace BSOA.Column
{
    /// <summary>
    ///  StringColumn stores strings as UTF-8.
    /// </summary>
    public class StringColumn : NullableColumn<string>
    {
        // StringColumn is a:
        //  - NullableColumn, to track and return nulls, over an
        //  - ArraySliceColumn<byte>, to store the UTF-8 bytes per row
        public StringColumn() : base(new NotNullStringColumn())
        { }
    }

    /// <summary>
    ///  StringColumn stores strings as UTF-8.
    /// </summary>
    internal class NotNullStringColumn : WrappingColumn<string, ArraySlice<byte>>
    {
        public NotNullStringColumn() : base(new ArraySliceColumn<byte>())
        { }

        public override string this[int index] 
        {
            get
            {
                ArraySlice<byte> value = Inner[index];
                if (value.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return Encoding.UTF8.GetString(value.Array, value.Index, value.Count);
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Inner[index] = ArraySlice<byte>.Empty;
                }
                else
                {
                    Inner[index] = new ArraySlice<byte>(Encoding.UTF8.GetBytes(value));
                }
            }
        }
    }
}
