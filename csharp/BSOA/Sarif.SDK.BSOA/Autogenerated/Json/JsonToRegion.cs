using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToRegion))]
    public partial class Region
    { }
    
    internal class JsonToRegion : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Region>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = JsonToInt.Read(reader, root),
            ["startColumn"] = (reader, root, me) => me.StartColumn = JsonToInt.Read(reader, root),
            ["endLine"] = (reader, root, me) => me.EndLine = JsonToInt.Read(reader, root),
            ["endColumn"] = (reader, root, me) => me.EndColumn = JsonToInt.Read(reader, root),
            ["charOffset"] = (reader, root, me) => me.CharOffset = JsonToInt.Read(reader, root),
            ["charLength"] = (reader, root, me) => me.CharLength = JsonToInt.Read(reader, root),
            ["byteOffset"] = (reader, root, me) => me.ByteOffset = JsonToInt.Read(reader, root),
            ["byteLength"] = (reader, root, me) => me.ByteLength = JsonToInt.Read(reader, root),
            ["snippet"] = (reader, root, me) => me.Snippet = JsonToArtifactContent.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["sourceLanguage"] = (reader, root, me) => me.SourceLanguage = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Region Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Region item = (root == null ? new Region() : new Region(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Region item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Region item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "startLine", item.StartLine, default);
                JsonToInt.Write(writer, "startColumn", item.StartColumn, default);
                JsonToInt.Write(writer, "endLine", item.EndLine, default);
                JsonToInt.Write(writer, "endColumn", item.EndColumn, default);
                JsonToInt.Write(writer, "charOffset", item.CharOffset, -1);
                JsonToInt.Write(writer, "charLength", item.CharLength, default);
                JsonToInt.Write(writer, "byteOffset", item.ByteOffset, -1);
                JsonToInt.Write(writer, "byteLength", item.ByteLength, default);
                JsonToArtifactContent.Write(writer, "snippet", item.Snippet);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToString.Write(writer, "sourceLanguage", item.SourceLanguage, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Region));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Region)value);
        }
    }
}
