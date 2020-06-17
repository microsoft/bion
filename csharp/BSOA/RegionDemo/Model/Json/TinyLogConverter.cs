using Newtonsoft.Json;

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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadTinyLog();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((TinyLog)value);
        }
    }

    internal static class TinyLogJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, TinyLog>> setters = new Dictionary<string, Action<JsonReader, TinyLog, TinyLog>>()
        {
            ["regions"] = (reader, root, me) => reader.ReadList(root, me.Regions, EmployeeJsonExtensions.ReadEmployee)
        };

        public static TinyLog ReadTinyLog(this JsonReader reader, TinyLog root = null)
        {
            TinyLog item = new TinyLog();
            root = item;

            reader.ReadObject(root, item, setters);
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
                writer.WriteList("regions", item.Regions, EmployeeJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
