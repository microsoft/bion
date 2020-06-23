using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(JsonToRegion))]
    public partial class Region
    { }
    
    internal class JsonToRegion : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Region>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Region>>()
        {
            ["startLine"] = (reader, root, me) => me.StartLine = JsonToInt.Read(reader, root),
            ["startColumn"] = (reader, root, me) => me.StartColumn = JsonToInt.Read(reader, root),
            ["endLine"] = (reader, root, me) => me.EndLine = JsonToInt.Read(reader, root),
            ["endColumn"] = (reader, root, me) => me.EndColumn = JsonToInt.Read(reader, root),
            ["snippet"] = (reader, root, me) => me.Snippet = JsonToArtifactContent.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root)
        };

        public static Region Read(JsonReader reader, TinyLog root = null)
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
                JsonToInt.Write(writer, "startLine", item.StartLine, 0);
                JsonToInt.Write(writer, "startColumn", item.StartColumn, 0);
                JsonToInt.Write(writer, "endLine", item.EndLine, 0);
                JsonToInt.Write(writer, "endColumn", item.EndColumn, 0);
                JsonToArtifactContent.Write(writer, "snippet", item.Snippet);
                JsonToMessage.Write(writer, "message", item.Message);
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
