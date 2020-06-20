using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToReportingDescriptor))]
    public partial class ReportingDescriptor
    { }
    
    internal class JsonToReportingDescriptor : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptor>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptor>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["deprecatedIds"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.DeprecatedIds, JsonToString.Read),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["deprecatedGuids"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.DeprecatedGuids, JsonToString.Read),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["deprecatedNames"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.DeprecatedNames, JsonToString.Read),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["messageStrings"] = (reader, root, me) => me.MessageStrings = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["defaultConfiguration"] = (reader, root, me) => me.DefaultConfiguration = JsonToReportingConfiguration.Read(reader, root),
            ["helpUri"] = (reader, root, me) => me.HelpUri = JsonToUri.Read(reader, root),
            ["help"] = (reader, root, me) => me.Help = JsonToMultiformatMessageString.Read(reader, root),
            ["relationships"] = (reader, root, me) => JsonToIList<ReportingDescriptorRelationship>.Read(reader, root, me.Relationships, JsonToReportingDescriptorRelationship.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ReportingDescriptor Read(JsonReader reader, SarifLog root = null)
        {
            ReportingDescriptor item = (root == null ? new ReportingDescriptor() : new ReportingDescriptor(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ReportingDescriptor item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ReportingDescriptor item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToIList<String>.Write(writer, "deprecatedIds", item.DeprecatedIds, JsonToString.Write);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToIList<String>.Write(writer, "deprecatedGuids", item.DeprecatedGuids, JsonToString.Write);
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToIList<String>.Write(writer, "deprecatedNames", item.DeprecatedNames, JsonToString.Write);
                JsonToMultiformatMessageString.Write(writer, "shortDescription", item.ShortDescription);
                JsonToMultiformatMessageString.Write(writer, "fullDescription", item.FullDescription);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "messageStrings", item.MessageStrings, JsonToMultiformatMessageString.Write);
                JsonToReportingConfiguration.Write(writer, "defaultConfiguration", item.DefaultConfiguration);
                JsonToUri.Write(writer, "helpUri", item.HelpUri, default);
                JsonToMultiformatMessageString.Write(writer, "help", item.Help);
                JsonToIList<ReportingDescriptorRelationship>.Write(writer, "relationships", item.Relationships, JsonToReportingDescriptorRelationship.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptor));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ReportingDescriptor)value);
        }
    }
}
