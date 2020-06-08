using BSOA.Collections;

using System.Text;

namespace BSOA.Converter
{
    /// <summary>
    ///  StringConverter provides conversion between string and byte[]
    /// </summary>
    public class StringConverter : IConverter<ArraySlice<byte>, string>, IConverter<string, ArraySlice<byte>>
    {
        public static StringConverter Instance = new StringConverter();

        private StringConverter()
        { }

        public string Convert(ArraySlice<byte> value)
        {
            return Encoding.UTF8.GetString(value.Array, value.Index, value.Count);
        }

        public ArraySlice<byte> Convert(string value)
        {
            if(value == null) { return ArraySlice<byte>.Empty; }
            return new ArraySlice<byte>(Encoding.UTF8.GetBytes(value));
        }
    }
}
