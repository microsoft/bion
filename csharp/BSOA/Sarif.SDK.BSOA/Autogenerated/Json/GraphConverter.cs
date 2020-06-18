using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(GraphConverter))]
    public partial class Graph
    { }
    
    public class GraphConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Graph));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadGraph();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Graph)value);
        }
    }
    
    internal static class GraphJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Graph>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Graph>>()
        {
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["nodes"] = (reader, root, me) => reader.ReadList(root, me.Nodes, NodeJsonExtensions.ReadNode),
            ["edges"] = (reader, root, me) => reader.ReadList(root, me.Edges, EdgeJsonExtensions.ReadEdge),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Graph ReadGraph(this JsonReader reader, SarifLog root = null)
        {
            Graph item = (root == null ? new Graph() : new Graph(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Graph item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Graph item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("description", item.Description);
                writer.WriteList("nodes", item.Nodes, NodeJsonExtensions.Write);
                writer.WriteList("edges", item.Edges, EdgeJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
