using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(GraphTraversalConverter))]
    public partial class GraphTraversal
    { }
    
    public class GraphTraversalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(GraphTraversal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadGraphTraversal();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((GraphTraversal)value);
        }
    }
    
    internal static class GraphTraversalJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, GraphTraversal>> setters = new Dictionary<string, Action<JsonReader, SarifLog, GraphTraversal>>()
        {
            ["runGraphIndex"] = (reader, root, me) => me.RunGraphIndex = reader.ReadInt(root),
            ["resultGraphIndex"] = (reader, root, me) => me.ResultGraphIndex = reader.ReadInt(root),
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["initialState"] = (reader, root, me) => reader.ReadDictionary(root, me.InitialState, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["immutableState"] = (reader, root, me) => reader.ReadDictionary(root, me.ImmutableState, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["edgeTraversals"] = (reader, root, me) => reader.ReadList(root, me.EdgeTraversals, EdgeTraversalJsonExtensions.ReadEdgeTraversal),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static GraphTraversal ReadGraphTraversal(this JsonReader reader, SarifLog root = null)
        {
            GraphTraversal item = (root == null ? new GraphTraversal() : new GraphTraversal(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, GraphTraversal item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, GraphTraversal item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("runGraphIndex", item.RunGraphIndex, -1);
                writer.Write("resultGraphIndex", item.ResultGraphIndex, -1);
                writer.Write("description", item.Description);
                writer.Write("initialState", item.InitialState, default);
                writer.Write("immutableState", item.ImmutableState, default);
                writer.WriteList("edgeTraversals", item.EdgeTraversals, EdgeTraversalJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
