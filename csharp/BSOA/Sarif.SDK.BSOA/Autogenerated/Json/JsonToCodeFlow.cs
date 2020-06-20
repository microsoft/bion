using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToCodeFlow))]
    public partial class CodeFlow
    { }
    
    internal class JsonToCodeFlow : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, CodeFlow>> setters = new Dictionary<string, Action<JsonReader, SarifLog, CodeFlow>>()
        {
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["threadFlows"] = (reader, root, me) => JsonToIList<ThreadFlow>.Read(reader, root, me.ThreadFlows, JsonToThreadFlow.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static CodeFlow Read(JsonReader reader, SarifLog root = null)
        {
            CodeFlow item = (root == null ? new CodeFlow() : new CodeFlow(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, CodeFlow item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, CodeFlow item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIList<ThreadFlow>.Write(writer, "threadFlows", item.ThreadFlows, JsonToThreadFlow.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(CodeFlow));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (CodeFlow)value);
        }
    }
}
