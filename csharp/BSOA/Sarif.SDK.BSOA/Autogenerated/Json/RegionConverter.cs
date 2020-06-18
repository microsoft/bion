using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
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
        private static Dictionary<string, Action<JsonReader, SarifLog, Region>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = reader.ReadInt(root),
            ["startColumn"] = (reader, root, me) => me.StartColumn = reader.ReadInt(root),
            ["endLine"] = (reader, root, me) => me.EndLine = reader.ReadInt(root),
            ["endColumn"] = (reader, root, me) => me.EndColumn = reader.ReadInt(root),
            ["charOffset"] = (reader, root, me) => me.CharOffset = reader.ReadInt(root),
            ["charLength"] = (reader, root, me) => me.CharLength = reader.ReadInt(root),
            ["byteOffset"] = (reader, root, me) => me.ByteOffset = reader.ReadInt(root),
            ["byteLength"] = (reader, root, me) => me.ByteLength = reader.ReadInt(root),
            ["snippet"] = (reader, root, me) => me.Snippet = reader.ReadArtifactContent(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["sourceLanguage"] = (reader, root, me) => me.SourceLanguage = reader.ReadString(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static Region ReadRegion(this JsonReader reader, SarifLog root = null)
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
                writer.Write("startLine", item.StartLine, default);
                writer.Write("startColumn", item.StartColumn, default);
                writer.Write("endLine", item.EndLine, default);
                writer.Write("endColumn", item.EndColumn, default);
                writer.Write("charOffset", item.CharOffset, -1);
                writer.Write("charLength", item.CharLength, default);
                writer.Write("byteOffset", item.ByteOffset, -1);
                writer.Write("byteLength", item.ByteLength, default);
                writer.Write("snippet", item.Snippet);
                writer.Write("message", item.Message);
                writer.Write("sourceLanguage", item.SourceLanguage, default);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
