using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ReportingDescriptorReferenceConverter))]
    public partial class ReportingDescriptorReference
    { }
    
    public class ReportingDescriptorReferenceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptorReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadReportingDescriptorReference();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ReportingDescriptorReference)value);
        }
    }
    
    internal static class ReportingDescriptorReferenceJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptorReference>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["toolComponent"] = (reader, root, me) => me.ToolComponent = reader.ReadToolComponentReference(root),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static ReportingDescriptorReference ReadReportingDescriptorReference(this JsonReader reader, SarifLog root = null)
        {
            ReportingDescriptorReference item = (root == null ? new ReportingDescriptorReference() : new ReportingDescriptorReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ReportingDescriptorReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ReportingDescriptorReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("id", item.Id, default);
                writer.Write("index", item.Index, -1);
                writer.Write("guid", item.Guid, default);
                writer.Write("toolComponent", item.ToolComponent);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
