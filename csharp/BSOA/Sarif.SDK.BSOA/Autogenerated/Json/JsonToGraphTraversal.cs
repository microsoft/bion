using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToGraphTraversal))]
    public partial class GraphTraversal
    { }
    
    internal class JsonToGraphTraversal : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, GraphTraversal>> setters = new Dictionary<string, Action<JsonReader, SarifLog, GraphTraversal>>()
        {
            ["runGraphIndex"] = (reader, root, me) => me.RunGraphIndex = JsonToInt.Read(reader, root),
            ["resultGraphIndex"] = (reader, root, me) => me.ResultGraphIndex = JsonToInt.Read(reader, root),
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["initialState"] = (reader, root, me) => me.InitialState = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["immutableState"] = (reader, root, me) => me.ImmutableState = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["edgeTraversals"] = (reader, root, me) => JsonToIList<EdgeTraversal>.Read(reader, root, me.EdgeTraversals, JsonToEdgeTraversal.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static GraphTraversal Read(JsonReader reader, SarifLog root = null)
        {
            GraphTraversal item = (root == null ? new GraphTraversal() : new GraphTraversal(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, GraphTraversal item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, GraphTraversal item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "runGraphIndex", item.RunGraphIndex, -1);
                JsonToInt.Write(writer, "resultGraphIndex", item.ResultGraphIndex, -1);
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "initialState", item.InitialState, JsonToMultiformatMessageString.Write);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "immutableState", item.ImmutableState, JsonToMultiformatMessageString.Write);
                JsonToIList<EdgeTraversal>.Write(writer, "edgeTraversals", item.EdgeTraversals, JsonToEdgeTraversal.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(GraphTraversal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (GraphTraversal)value);
        }
    }
}
