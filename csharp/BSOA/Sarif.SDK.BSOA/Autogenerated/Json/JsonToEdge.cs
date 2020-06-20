using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToEdge))]
    public partial class Edge
    { }
    
    internal class JsonToEdge : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Edge>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Edge>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["label"] = (reader, root, me) => me.Label = JsonToMessage.Read(reader, root),
            ["sourceNodeId"] = (reader, root, me) => me.SourceNodeId = JsonToString.Read(reader, root),
            ["targetNodeId"] = (reader, root, me) => me.TargetNodeId = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Edge Read(JsonReader reader, SarifLog root = null)
        {
            Edge item = (root == null ? new Edge() : new Edge(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Edge item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Edge item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToMessage.Write(writer, "label", item.Label);
                JsonToString.Write(writer, "sourceNodeId", item.SourceNodeId, default);
                JsonToString.Write(writer, "targetNodeId", item.TargetNodeId, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Edge));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Edge)value);
        }
    }
}
