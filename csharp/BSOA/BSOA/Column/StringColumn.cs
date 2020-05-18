using BSOA.Converter;

namespace BSOA.Column
{
    /// <summary>
    ///  StringColumn stores strings as UTF-8.
    /// </summary>
    public class StringColumn : NullableColumn<string>
    {
        // StringColumn is a:
        //  - NullableColumn, to track and return nulls, over a
        //  - ConvertingColumn, to convert strings to and from byte[], over a
        //  - ArraySliceColumn<byte>, to store the UTF-8 bytes per row
        public StringColumn() : base(
            new ConvertingColumn<string, ArraySlice<byte>>(new ArraySliceColumn<byte>(), StringConverter.Instance))
        { }
    }
}
