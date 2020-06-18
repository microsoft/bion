using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(EdgeTraversalConverter))]
    public partial class EdgeTraversal
    { }
    
    public class EdgeTraversalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(EdgeTraversal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadEdgeTraversal();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((EdgeTraversal)value);
        }
    }
    
    internal static class EdgeTraversalJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, EdgeTraversal>> setters = new Dictionary<string, Action<JsonReader, SarifLog, EdgeTraversal>>()
        {
            ["edgeId"] = (reader, root, me) => me.EdgeId = reader.ReadString(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["finalState"] = (reader, root, me) => reader.ReadDictionary(root, me.FinalState, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["stepOverEdgeCount"] = (reader, root, me) => me.StepOverEdgeCount = reader.ReadInt(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static EdgeTraversal ReadEdgeTraversal(this JsonReader reader, SarifLog root = null)
        {
            EdgeTraversal item = (root == null ? new EdgeTraversal() : new EdgeTraversal(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, EdgeTraversal item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, EdgeTraversal item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("edgeId", item.EdgeId, default(string));
                writer.Write("message", item.Message);
                writer.Write("finalState", item.FinalState, default(IDictionary<string, MultiformatMessageString>));
                writer.Write("stepOverEdgeCount", item.StepOverEdgeCount, default(int));
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
