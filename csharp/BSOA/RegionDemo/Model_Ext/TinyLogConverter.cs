using Newtonsoft.Json;

using RegionDemo.Model_Ext;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(TinyLogConverter))]
    public partial class TinyLog
    { }

    public class TinyLogConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(TinyLog));
        }

        private static Dictionary<string, Action<JsonReader, TinyLog, TinyLog>> setters = new Dictionary<string, Action<JsonReader, TinyLog, TinyLog>>()
        {
            ["regions"] = (reader, root, me) => Converters.ReadList(reader, root, me.Regions, RegionConverter.ReadJson),
        };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, null);
        }

        public static TinyLog ReadJson(JsonReader reader, TinyLog root)
        {
            TinyLog item = (root == null ? new TinyLog() : root);
            Converters.ReadObject(reader, item, item, setters);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TinyLog item = (TinyLog)value;

            writer.WriteStartObject();

            if (item.Regions?.Count > 0)
            {
                writer.WritePropertyName("regions");
                writer.WriteValue(item.Regions);
            }

            writer.WriteEndObject();
        }
    }
}
