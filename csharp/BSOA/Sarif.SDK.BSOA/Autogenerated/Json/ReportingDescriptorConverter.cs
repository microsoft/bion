using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ReportingDescriptorConverter))]
    public partial class ReportingDescriptor
    { }
    
    public class ReportingDescriptorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingDescriptor));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadReportingDescriptor();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ReportingDescriptor)value);
        }
    }
    
    internal static class ReportingDescriptorJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptor>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingDescriptor>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["deprecatedIds"] = (reader, root, me) => reader.ReadList(root, me.DeprecatedIds, JsonReaderExtensions.ReadString),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["deprecatedGuids"] = (reader, root, me) => reader.ReadList(root, me.DeprecatedGuids, JsonReaderExtensions.ReadString),
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["deprecatedNames"] = (reader, root, me) => reader.ReadList(root, me.DeprecatedNames, JsonReaderExtensions.ReadString),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = reader.ReadMultiformatMessageString(root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = reader.ReadMultiformatMessageString(root),
            ["messageStrings"] = (reader, root, me) => reader.ReadDictionary(root, me.MessageStrings, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["defaultConfiguration"] = (reader, root, me) => me.DefaultConfiguration = reader.ReadReportingConfiguration(root),
            ["helpUri"] = (reader, root, me) => me.HelpUri = reader.ReadUri(root),
            ["help"] = (reader, root, me) => me.Help = reader.ReadMultiformatMessageString(root),
            ["relationships"] = (reader, root, me) => reader.ReadList(root, me.Relationships, ReportingDescriptorRelationshipJsonExtensions.ReadReportingDescriptorRelationship),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ReportingDescriptor ReadReportingDescriptor(this JsonReader reader, SarifLog root = null)
        {
            ReportingDescriptor item = (root == null ? new ReportingDescriptor() : new ReportingDescriptor(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ReportingDescriptor item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ReportingDescriptor item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("id", item.Id, default);
                writer.Write("deprecatedIds", item.DeprecatedIds, default);
                writer.Write("guid", item.Guid, default);
                writer.Write("deprecatedGuids", item.DeprecatedGuids, default);
                writer.Write("name", item.Name, default);
                writer.Write("deprecatedNames", item.DeprecatedNames, default);
                writer.Write("shortDescription", item.ShortDescription);
                writer.Write("fullDescription", item.FullDescription);
                writer.Write("messageStrings", item.MessageStrings, default);
                writer.Write("defaultConfiguration", item.DefaultConfiguration);
                writer.Write("helpUri", item.HelpUri, default);
                writer.Write("help", item.Help);
                writer.WriteList("relationships", item.Relationships, ReportingDescriptorRelationshipJsonExtensions.Write);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}