using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToRunAutomationDetails))]
    public partial class RunAutomationDetails
    { }
    
    internal class JsonToRunAutomationDetails : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, RunAutomationDetails>> setters = new Dictionary<string, Action<JsonReader, SarifLog, RunAutomationDetails>>()
        {
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["correlationGuid"] = (reader, root, me) => me.CorrelationGuid = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static RunAutomationDetails Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            RunAutomationDetails item = (root == null ? new RunAutomationDetails() : new RunAutomationDetails(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, RunAutomationDetails item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, RunAutomationDetails item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToString.Write(writer, "correlationGuid", item.CorrelationGuid, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(RunAutomationDetails));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (RunAutomationDetails)value);
        }
    }
}
