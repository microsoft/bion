using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(EdgeConverter))]
    public partial class Edge
    { }
    
    public class EdgeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Edge));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadEdge();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Edge)value);
        }
    }
    
    internal static class EdgeJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Edge>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Edge>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["label"] = (reader, root, me) => me.Label = reader.ReadMessage(root),
            ["sourceNodeId"] = (reader, root, me) => me.SourceNodeId = reader.ReadString(root),
            ["targetNodeId"] = (reader, root, me) => me.TargetNodeId = reader.ReadString(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Edge ReadEdge(this JsonReader reader, SarifLog root = null)
        {
            Edge item = (root == null ? new Edge() : new Edge(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Edge item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Edge item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("id", item.Id, default);
                writer.Write("label", item.Label);
                writer.Write("sourceNodeId", item.SourceNodeId, default);
                writer.Write("targetNodeId", item.TargetNodeId, default);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
