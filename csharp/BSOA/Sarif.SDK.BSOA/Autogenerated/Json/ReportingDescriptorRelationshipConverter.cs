using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ReportingDescriptorRelationshipConverter))]
    public partial class ReportingDescriptorRelationship
    { }
    
    public class ReportingDescriptorRelationshipConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptorRelationship));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadReportingDescriptorRelationship();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ReportingDescriptorRelationship)value);
        }
    }
    
    internal static class ReportingDescriptorRelationshipJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorRelationship>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorRelationship>>()
        {
            ["target"] = (reader, root, me) => me.Target = reader.ReadReportingDescriptorReference(root),
            ["kinds"] = (reader, root, me) => reader.ReadList(root, me.Kinds, JsonReaderExtensions.ReadString),
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ReportingDescriptorRelationship ReadReportingDescriptorRelationship(this JsonReader reader, SarifLog root = null)
        {
            ReportingDescriptorRelationship item = (root == null ? new ReportingDescriptorRelationship() : new ReportingDescriptorRelationship(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ReportingDescriptorRelationship item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ReportingDescriptorRelationship item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("target", item.Target);
                writer.Write("kinds", item.Kinds, default);
                writer.Write("description", item.Description);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
