using System.Text;

namespace BSOA.Converter
{
    /// <summary>
    ///  StringConverter provides conversion between string and byte[]
    /// </summary>
    public class Utf8StringConverter : IConverter<ArraySlice<byte>, string>, IConverter<string, ArraySlice<byte>>
    {
        public static Utf8StringConverter Instance = new Utf8StringConverter();

        private Utf8StringConverter()
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
