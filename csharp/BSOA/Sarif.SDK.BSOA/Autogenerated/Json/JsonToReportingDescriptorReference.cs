using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToReportingDescriptorReference))]
    public partial class ReportingDescriptorReference
    { }
    
    internal class JsonToReportingDescriptorReference : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorReference>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["toolComponent"] = (reader, root, me) => me.ToolComponent = JsonToToolComponentReference.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ReportingDescriptorReference Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ReportingDescriptorReference item = (root == null ? new ReportingDescriptorReference() : new ReportingDescriptorReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ReportingDescriptorReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ReportingDescriptorReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToToolComponentReference.Write(writer, "toolComponent", item.ToolComponent);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptorReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ReportingDescriptorReference)value);
        }
    }
}
