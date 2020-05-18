using System;

namespace BSOA.Converter
{
    /// <summary>
    ///  UriConverter provides conversion between Uri and string.
    /// </summary>
    public class UriConverter : IConverter<string, Uri>, IConverter<Uri, string>
    {
        public static UriConverter Instance = new UriConverter();

        private UriConverter()
        { }

        public Uri Convert(string value)
        {
            return (value == null ? null : new Uri(value, UriKind.RelativeOrAbsolute));
        }

        public string Convert(Uri value)
        {
            return value?.OriginalString;
        }
    }
}
