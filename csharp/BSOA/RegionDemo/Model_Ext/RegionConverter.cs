using Newtonsoft.Json;

using RegionDemo.Model_Ext;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(RegionConverter))]
    public partial class Region
    { }

    public class RegionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Region));
        }

        private static Dictionary<string, Action<JsonReader, TinyLog, Region>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = (int)(long)reader.Value,
            ["startColumn"] = (reader, root, me) => me.StartColumn = (int)(long)reader.Value,
            ["endLine"] = (reader, root, me) => me.EndLine = (int)(long)reader.Value,
            ["endColumn"] = (reader, root, me) => me.EndColumn = (int)(long)reader.Value,
            ["message"] = (reader, root, me) => me.Message = MessageConverter.ReadJson(reader, root),
            ["snippet"] = (reader, root, me) => me.Snippet = ArtifactContentConverter.ReadJson(reader, root)
        };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, null);
        }

        public static Region ReadJson(JsonReader reader, TinyLog root)
        {
            Region item = (root == null ? new Region() : new Region(root));
            Converters.ReadObject(reader, root, item, setters);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Region item = (Region)value;

            writer.WriteStartObject();

            if (item.StartLine != -1)
            {
                writer.WritePropertyName("startLine");
                writer.WriteValue(item.StartLine);
            }

            if (item.StartColumn != -1)
            {
                writer.WritePropertyName("startColumn");
                writer.WriteValue(item.StartColumn);
            }

            if (item.EndLine != -1)
            {
                writer.WritePropertyName("endLine");
                writer.WriteValue(item.EndLine);
            }

            if (item.EndColumn != -1)
            {
                writer.WritePropertyName("endColumn");
                writer.WriteValue(item.EndColumn);
            }

            if (item.Message != default(Message))
            {
                writer.WritePropertyName("message");
                writer.WriteValue(item.Message);
            }

            if (item.Snippet != default(ArtifactContent))
            {
                writer.WritePropertyName("snippet");
                writer.WriteValue(item.Snippet);
            }

            writer.WriteEndObject();
        }
    }
}
