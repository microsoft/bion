using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    internal static class TinyLogJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, TinyLog>> setters = new Dictionary<string, Action<JsonReader, TinyLog, TinyLog>>()
        {
            ["regions"] = (reader, root, me) => reader.ReadList(root, me.Regions, RegionJsonExtensions.ReadRegion),
        };

        public static TinyLog ReadTinyLog(this JsonReader reader, TinyLog root = null)
        {
            TinyLog item = new TinyLog();
            reader.ReadObject(item, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, TinyLog item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("regions", item.Regions, default(IList<Region>));
                writer.WriteEndObject();
            }
        }
    }

    public class TinyLogConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(TinyLog));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadTinyLog();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((TinyLog)value);
        }
    }

    [JsonConverter(typeof(TinyLogConverter))]
    public partial class TinyLog
    { }
}
