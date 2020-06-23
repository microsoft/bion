using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BSOA.Json.Converters
{
    public static class JsonToEnum<TEnum>
    {
        private static StringEnumConverter _enumConverter = new StringEnumConverter(camelCaseText: true);

        public static TEnum Read<TRoot>(JsonReader reader, TRoot root)
        {
            return (TEnum)_enumConverter.ReadJson(reader, typeof(TEnum), null, null);
        }

        public static void Write(JsonWriter writer, string propertyName, TEnum item, TEnum defaultValue = default)
        {
            if (!item.Equals(defaultValue))
            {
                writer.WritePropertyName(propertyName);
                _enumConverter.WriteJson(writer, item, null);
            }
        }

        public static void Write(JsonWriter writer, TEnum item)
        {
            _enumConverter.WriteJson(writer, item, null);
        }
    }
}
