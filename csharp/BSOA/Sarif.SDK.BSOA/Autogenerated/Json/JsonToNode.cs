using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToNode))]
    public partial class Node
    { }
    
    internal class JsonToNode : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Node>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Node>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["label"] = (reader, root, me) => me.Label = JsonToMessage.Read(reader, root),
            ["location"] = (reader, root, me) => me.Location = JsonToLocation.Read(reader, root),
            ["children"] = (reader, root, me) => JsonToIList<Node>.Read(reader, root, me.Children, JsonToNode.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Node Read(JsonReader reader, SarifLog root = null)
        {
            Node item = (root == null ? new Node() : new Node(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Node item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Node item)
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
                JsonToLocation.Write(writer, "location", item.Location);
                JsonToIList<Node>.Write(writer, "children", item.Children, JsonToNode.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Node));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Node)value);
        }
    }
}
