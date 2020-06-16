using Newtonsoft.Json;

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
            reader.ReadObject(root, item, setters);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Region item = (Region)value;

            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("startLine", item.StartLine, -1);
                writer.Write("startColumn", item.StartColumn, -1);
                writer.Write("endLine", item.EndLine, -1);
                writer.Write("endColumn", item.EndColumn, -1);
                writer.Write("message", item.Message, default(Message));
                writer.Write("snippet", item.Snippet, default(ArtifactContent));
                writer.WriteEndObject();
            }
        }
    }
}
