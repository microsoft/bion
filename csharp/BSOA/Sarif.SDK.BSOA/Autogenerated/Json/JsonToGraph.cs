using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToGraph))]
    public partial class Graph
    { }
    
    internal class JsonToGraph : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Graph>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Graph>>()
        {
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["nodes"] = (reader, root, me) => JsonToIList<Node>.Read(reader, root, me.Nodes, JsonToNode.Read),
            ["edges"] = (reader, root, me) => JsonToIList<Edge>.Read(reader, root, me.Edges, JsonToEdge.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Graph Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Graph item = (root == null ? new Graph() : new Graph(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Graph item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Graph item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIList<Node>.Write(writer, "nodes", item.Nodes, JsonToNode.Write);
                JsonToIList<Edge>.Write(writer, "edges", item.Edges, JsonToEdge.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Graph));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Graph)value);
        }
    }
}
