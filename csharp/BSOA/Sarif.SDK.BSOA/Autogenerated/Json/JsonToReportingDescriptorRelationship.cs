using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToReportingDescriptorRelationship))]
    public partial class ReportingDescriptorRelationship
    { }
    
    internal class JsonToReportingDescriptorRelationship : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorRelationship>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorRelationship>>()
        {
            ["target"] = (reader, root, me) => me.Target = JsonToReportingDescriptorReference.Read(reader, root),
            ["kinds"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Kinds, JsonToString.Read),
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ReportingDescriptorRelationship Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ReportingDescriptorRelationship item = (root == null ? new ReportingDescriptorRelationship() : new ReportingDescriptorRelationship(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ReportingDescriptorRelationship item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ReportingDescriptorRelationship item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToReportingDescriptorReference.Write(writer, "target", item.Target);
                JsonToIList<String>.Write(writer, "kinds", item.Kinds, JsonToString.Write);
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptorRelationship));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ReportingDescriptorRelationship)value);
        }
    }
}
