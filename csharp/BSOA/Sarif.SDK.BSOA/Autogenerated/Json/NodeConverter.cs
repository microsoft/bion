using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(NodeConverter))]
    public partial class Node
    { }
    
    public class NodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Node));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadNode();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Node)value);
        }
    }
    
    internal static class NodeJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Node>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Node>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["label"] = (reader, root, me) => me.Label = reader.ReadMessage(root),
            ["location"] = (reader, root, me) => me.Location = reader.ReadLocation(root),
            ["children"] = (reader, root, me) => reader.ReadList(root, me.Children, NodeJsonExtensions.ReadNode),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Node ReadNode(this JsonReader reader, SarifLog root = null)
        {
            Node item = (root == null ? new Node() : new Node(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Node item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Node item)
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
                writer.Write("location", item.Location);
                writer.WriteList("children", item.Children, NodeJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
