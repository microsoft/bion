using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(CodeFlowConverter))]
    public partial class CodeFlow
    { }
    
    public class CodeFlowConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(CodeFlow));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadCodeFlow();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((CodeFlow)value);
        }
    }
    
    internal static class CodeFlowJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, CodeFlow>> setters = new Dictionary<string, Action<JsonReader, SarifLog, CodeFlow>>()
        {
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["threadFlows"] = (reader, root, me) => reader.ReadList(root, me.ThreadFlows, ThreadFlowJsonExtensions.ReadThreadFlow),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static CodeFlow ReadCodeFlow(this JsonReader reader, SarifLog root = null)
        {
            CodeFlow item = (root == null ? new CodeFlow() : new CodeFlow(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, CodeFlow item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, CodeFlow item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("message", item.Message);
                writer.WriteList("threadFlows", item.ThreadFlows, ThreadFlowJsonExtensions.Write);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
