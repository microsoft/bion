using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    internal static class RegionJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Region>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = reader.ReadInt(root),
            ["startColumn"] = (reader, root, me) => me.StartColumn = reader.ReadInt(root),
            ["endLine"] = (reader, root, me) => me.EndLine = reader.ReadInt(root),
            ["endColumn"] = (reader, root, me) => me.EndColumn = reader.ReadInt(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["snippet"] = (reader, root, me) => me.Snippet = reader.ReadArtifactContent(root)
        };

        public static Region ReadRegion(this JsonReader reader, TinyLog root = null)
        {
            Region item = (root == null ? new Region() : new Region(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, Region item)
        {
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

    internal class RegionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Region));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadRegion();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Region)value);
        }
    }

    [JsonConverter(typeof(RegionConverter))]
    public partial class Region
    { }
}
