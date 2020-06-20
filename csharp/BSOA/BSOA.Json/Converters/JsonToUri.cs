using Newtonsoft.Json;

using System;

namespace BSOA.Json.Converters
{
    public static class JsonToUri
    {
        public static Uri Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static Uri Read(JsonReader reader)
        {
            return new Uri((string)reader.Value, UriKind.RelativeOrAbsolute);
        }

        public static void Write(JsonWriter writer, string propertyName, Uri item, Uri defaultValue = default)
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item.OriginalString);
            }
        }

        public static void Write(JsonWriter writer, Uri item)
        {
            writer.WriteValue(item.OriginalString);
        }
    }
}
