using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ToolComponentConverter))]
    public partial class ToolComponent
    { }
    
    public class ToolComponentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ToolComponent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadToolComponent();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ToolComponent)value);
        }
    }
    
    internal static class ToolComponentJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ToolComponent>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ToolComponent>>()
        {
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["organization"] = (reader, root, me) => me.Organization = reader.ReadString(root),
            ["product"] = (reader, root, me) => me.Product = reader.ReadString(root),
            ["productSuite"] = (reader, root, me) => me.ProductSuite = reader.ReadString(root),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = reader.ReadMultiformatMessageString(root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = reader.ReadMultiformatMessageString(root),
            ["fullName"] = (reader, root, me) => me.FullName = reader.ReadString(root),
            ["version"] = (reader, root, me) => me.Version = reader.ReadString(root),
            ["semanticVersion"] = (reader, root, me) => me.SemanticVersion = reader.ReadString(root),
            ["dottedQuadFileVersion"] = (reader, root, me) => me.DottedQuadFileVersion = reader.ReadString(root),
            ["releaseDateUtc"] = (reader, root, me) => me.ReleaseDateUtc = reader.ReadString(root),
            ["downloadUri"] = (reader, root, me) => me.DownloadUri = reader.ReadUri(root),
            ["informationUri"] = (reader, root, me) => me.InformationUri = reader.ReadUri(root),
            ["globalMessageStrings"] = (reader, root, me) => reader.ReadDictionary(root, me.GlobalMessageStrings, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["notifications"] = (reader, root, me) => reader.ReadList(root, me.Notifications, ReportingDescriptorJsonExtensions.ReadReportingDescriptor),
            ["rules"] = (reader, root, me) => reader.ReadList(root, me.Rules, ReportingDescriptorJsonExtensions.ReadReportingDescriptor),
            ["taxa"] = (reader, root, me) => reader.ReadList(root, me.Taxa, ReportingDescriptorJsonExtensions.ReadReportingDescriptor),
            ["locations"] = (reader, root, me) => reader.ReadList(root, me.Locations, ArtifactLocationJsonExtensions.ReadArtifactLocation),
            ["language"] = (reader, root, me) => me.Language = reader.ReadString(root),
            ["contents"] = (reader, root, me) => me.Contents = reader.ReadEnum<ToolComponentContents, SarifLog>(root),
            ["isComprehensive"] = (reader, root, me) => me.IsComprehensive = reader.ReadBool(root),
            ["localizedDataSemanticVersion"] = (reader, root, me) => me.LocalizedDataSemanticVersion = reader.ReadString(root),
            ["minimumRequiredLocalizedDataSemanticVersion"] = (reader, root, me) => me.MinimumRequiredLocalizedDataSemanticVersion = reader.ReadString(root),
            ["associatedComponent"] = (reader, root, me) => me.AssociatedComponent = reader.ReadToolComponentReference(root),
            ["translationMetadata"] = (reader, root, me) => me.TranslationMetadata = reader.ReadTranslationMetadata(root),
            ["supportedTaxonomies"] = (reader, root, me) => reader.ReadList(root, me.SupportedTaxonomies, ToolComponentReferenceJsonExtensions.ReadToolComponentReference),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ToolComponent ReadToolComponent(this JsonReader reader, SarifLog root = null)
        {
            ToolComponent item = (root == null ? new ToolComponent() : new ToolComponent(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ToolComponent item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ToolComponent item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("guid", item.Guid, default(string));
                writer.Write("name", item.Name, default(string));
                writer.Write("organization", item.Organization, default(string));
                writer.Write("product", item.Product, default(string));
                writer.Write("productSuite", item.ProductSuite, default(string));
                writer.Write("shortDescription", item.ShortDescription);
                writer.Write("fullDescription", item.FullDescription);
                writer.Write("fullName", item.FullName, default(string));
                writer.Write("version", item.Version, default(string));
                writer.Write("semanticVersion", item.SemanticVersion, default(string));
                writer.Write("dottedQuadFileVersion", item.DottedQuadFileVersion, default(string));
                writer.Write("releaseDateUtc", item.ReleaseDateUtc, default(string));
                writer.Write("downloadUri", item.DownloadUri, default(Uri));
                writer.Write("informationUri", item.InformationUri, default(Uri));
                writer.Write("globalMessageStrings", item.GlobalMessageStrings, default(IDictionary<string, MultiformatMessageString>));
                writer.WriteList("notifications", item.Notifications, ReportingDescriptorJsonExtensions.Write);
                writer.WriteList("rules", item.Rules, ReportingDescriptorJsonExtensions.Write);
                writer.WriteList("taxa", item.Taxa, ReportingDescriptorJsonExtensions.Write);
                writer.WriteList("locations", item.Locations, ArtifactLocationJsonExtensions.Write);
                writer.Write("language", item.Language, "en-US");
                writer.Write("contents", item.Contents);
                writer.Write("isComprehensive", item.IsComprehensive, false);
                writer.Write("localizedDataSemanticVersion", item.LocalizedDataSemanticVersion, default(string));
                writer.Write("minimumRequiredLocalizedDataSemanticVersion", item.MinimumRequiredLocalizedDataSemanticVersion, default(string));
                writer.Write("associatedComponent", item.AssociatedComponent);
                writer.Write("translationMetadata", item.TranslationMetadata);
                writer.WriteList("supportedTaxonomies", item.SupportedTaxonomies, ToolComponentReferenceJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
