using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToLogicalLocation))]
    public partial class LogicalLocation
    { }
    
    internal class JsonToLogicalLocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, LogicalLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, LogicalLocation>>()
        {
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["fullyQualifiedName"] = (reader, root, me) => me.FullyQualifiedName = JsonToString.Read(reader, root),
            ["decoratedName"] = (reader, root, me) => me.DecoratedName = JsonToString.Read(reader, root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = JsonToInt.Read(reader, root),
            ["kind"] = (reader, root, me) => me.Kind = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static LogicalLocation Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            LogicalLocation item = (root == null ? new LogicalLocation() : new LogicalLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, LogicalLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, LogicalLocation item)
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
                JsonToString.Write(writer, "fullyQualifiedName", item.FullyQualifiedName, default);
                JsonToString.Write(writer, "decoratedName", item.DecoratedName, default);
                JsonToInt.Write(writer, "parentIndex", item.ParentIndex, -1);
                JsonToString.Write(writer, "kind", item.Kind, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(LogicalLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (LogicalLocation)value);
        }
    }
}
