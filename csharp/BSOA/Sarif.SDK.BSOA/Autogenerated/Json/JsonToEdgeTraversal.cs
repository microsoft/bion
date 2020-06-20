using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToEdgeTraversal))]
    public partial class EdgeTraversal
    { }
    
    internal class JsonToEdgeTraversal : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, EdgeTraversal>> setters = new Dictionary<string, Action<JsonReader, SarifLog, EdgeTraversal>>()
        {
            ["edgeId"] = (reader, root, me) => me.EdgeId = JsonToString.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["finalState"] = (reader, root, me) => me.FinalState = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["stepOverEdgeCount"] = (reader, root, me) => me.StepOverEdgeCount = JsonToInt.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static EdgeTraversal Read(JsonReader reader, SarifLog root = null)
        {
            EdgeTraversal item = (root == null ? new EdgeTraversal() : new EdgeTraversal(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, EdgeTraversal item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, EdgeTraversal item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "edgeId", item.EdgeId, default);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "finalState", item.FinalState, JsonToMultiformatMessageString.Write);
                JsonToInt.Write(writer, "stepOverEdgeCount", item.StepOverEdgeCount, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(EdgeTraversal));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (EdgeTraversal)value);
        }
    }
}
