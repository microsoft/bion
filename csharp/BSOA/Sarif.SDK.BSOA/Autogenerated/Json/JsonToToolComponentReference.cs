using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToToolComponentReference))]
    public partial class ToolComponentReference
    { }
    
    internal class JsonToToolComponentReference : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ToolComponentReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ToolComponentReference>>()
        {
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ToolComponentReference Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ToolComponentReference item = (root == null ? new ToolComponentReference() : new ToolComponentReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ToolComponentReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ToolComponentReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ToolComponentReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ToolComponentReference)value);
        }
    }
}
