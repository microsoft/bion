using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(JsonToTinyLog))]
    public partial class TinyLog
    { }

    internal class JsonToTinyLog : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, TinyLog>> setters = new Dictionary<string, Action<JsonReader, TinyLog, TinyLog>>()
        {
            ["regions"] = (reader, root, me) => JsonToIList<Region>.Read(reader, root, me.Regions, JsonToRegion.Read)
        };

        public static TinyLog Read(JsonReader reader, TinyLog root = null)
        {
            TinyLog item = new TinyLog();

            // TinyLog is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, TinyLog item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, TinyLog item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<Region>.Write(writer, "regions", item.Regions, JsonToRegion.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(TinyLog));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (TinyLog)value);
        }
    }
}
