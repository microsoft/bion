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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadRegion();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Region)value);
        }
    }
    
    internal static class RegionJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Region>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = reader.ReadInt(root),
            ["startColumn"] = (reader, root, me) => me.StartColumn = reader.ReadInt(root),
            ["endLine"] = (reader, root, me) => me.EndLine = reader.ReadInt(root),
            ["endColumn"] = (reader, root, me) => me.EndColumn = reader.ReadInt(root),
            ["snippet"] = (reader, root, me) => me.Snippet = ArtifactContentJsonExtensions.ReadArtifactContent(reader, root),
            ["message"] = (reader, root, me) => me.Message = MessageJsonExtensions.ReadMessage(reader, root)
        };

        public static Region ReadRegion(this JsonReader reader, TinyLog root = null)
        {
            Region item = (root == null ? new Region() : new Region(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Region item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
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
                writer.Write("startLine", item.StartLine, 0);
                writer.Write("startColumn", item.StartColumn, 0);
                writer.Write("endLine", item.EndLine, 0);
                writer.Write("endColumn", item.EndColumn, 0);
                writer.Write("snippet", item.Snippet);
                writer.Write("message", item.Message);
                writer.WriteEndObject();
            }
        }
    }
}
