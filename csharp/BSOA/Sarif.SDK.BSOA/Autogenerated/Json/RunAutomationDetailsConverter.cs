using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(RunAutomationDetailsConverter))]
    public partial class RunAutomationDetails
    { }
    
    public class RunAutomationDetailsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(RunAutomationDetails));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadRunAutomationDetails();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((RunAutomationDetails)value);
        }
    }
    
    internal static class RunAutomationDetailsJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, RunAutomationDetails>> setters = new Dictionary<string, Action<JsonReader, SarifLog, RunAutomationDetails>>()
        {
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["correlationGuid"] = (reader, root, me) => me.CorrelationGuid = reader.ReadString(root),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static RunAutomationDetails ReadRunAutomationDetails(this JsonReader reader, SarifLog root = null)
        {
            RunAutomationDetails item = (root == null ? new RunAutomationDetails() : new RunAutomationDetails(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, RunAutomationDetails item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, RunAutomationDetails item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("description", item.Description);
                writer.Write("id", item.Id, default);
                writer.Write("guid", item.Guid, default);
                writer.Write("correlationGuid", item.CorrelationGuid, default);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
