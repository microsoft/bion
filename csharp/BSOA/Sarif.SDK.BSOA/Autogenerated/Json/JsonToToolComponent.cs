using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToToolComponent))]
    public partial class ToolComponent
    { }
    
    internal class JsonToToolComponent : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ToolComponent>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ToolComponent>>()
        {
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["organization"] = (reader, root, me) => me.Organization = JsonToString.Read(reader, root),
            ["product"] = (reader, root, me) => me.Product = JsonToString.Read(reader, root),
            ["productSuite"] = (reader, root, me) => me.ProductSuite = JsonToString.Read(reader, root),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["fullName"] = (reader, root, me) => me.FullName = JsonToString.Read(reader, root),
            ["version"] = (reader, root, me) => me.Version = JsonToString.Read(reader, root),
            ["semanticVersion"] = (reader, root, me) => me.SemanticVersion = JsonToString.Read(reader, root),
            ["dottedQuadFileVersion"] = (reader, root, me) => me.DottedQuadFileVersion = JsonToString.Read(reader, root),
            ["releaseDateUtc"] = (reader, root, me) => me.ReleaseDateUtc = JsonToString.Read(reader, root),
            ["downloadUri"] = (reader, root, me) => me.DownloadUri = JsonToUri.Read(reader, root),
            ["informationUri"] = (reader, root, me) => me.InformationUri = JsonToUri.Read(reader, root),
            ["globalMessageStrings"] = (reader, root, me) => me.GlobalMessageStrings = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["notifications"] = (reader, root, me) => JsonToIList<ReportingDescriptor>.Read(reader, root, me.Notifications, JsonToReportingDescriptor.Read),
            ["rules"] = (reader, root, me) => JsonToIList<ReportingDescriptor>.Read(reader, root, me.Rules, JsonToReportingDescriptor.Read),
            ["taxa"] = (reader, root, me) => JsonToIList<ReportingDescriptor>.Read(reader, root, me.Taxa, JsonToReportingDescriptor.Read),
            ["locations"] = (reader, root, me) => JsonToIList<ArtifactLocation>.Read(reader, root, me.Locations, JsonToArtifactLocation.Read),
            ["language"] = (reader, root, me) => me.Language = JsonToString.Read(reader, root),
            ["contents"] = (reader, root, me) => me.Contents = JsonToEnum<ToolComponentContents>.Read(reader, root),
            ["isComprehensive"] = (reader, root, me) => me.IsComprehensive = JsonToBool.Read(reader, root),
            ["localizedDataSemanticVersion"] = (reader, root, me) => me.LocalizedDataSemanticVersion = JsonToString.Read(reader, root),
            ["minimumRequiredLocalizedDataSemanticVersion"] = (reader, root, me) => me.MinimumRequiredLocalizedDataSemanticVersion = JsonToString.Read(reader, root),
            ["associatedComponent"] = (reader, root, me) => me.AssociatedComponent = JsonToToolComponentReference.Read(reader, root),
            ["translationMetadata"] = (reader, root, me) => me.TranslationMetadata = JsonToTranslationMetadata.Read(reader, root),
            ["supportedTaxonomies"] = (reader, root, me) => JsonToIList<ToolComponentReference>.Read(reader, root, me.SupportedTaxonomies, JsonToToolComponentReference.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ToolComponent Read(JsonReader reader, SarifLog root = null)
        {
            ToolComponent item = (root == null ? new ToolComponent() : new ToolComponent(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ToolComponent item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ToolComponent item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToString.Write(writer, "organization", item.Organization, default);
                JsonToString.Write(writer, "product", item.Product, default);
                JsonToString.Write(writer, "productSuite", item.ProductSuite, default);
                JsonToMultiformatMessageString.Write(writer, "shortDescription", item.ShortDescription);
                JsonToMultiformatMessageString.Write(writer, "fullDescription", item.FullDescription);
                JsonToString.Write(writer, "fullName", item.FullName, default);
                JsonToString.Write(writer, "version", item.Version, default);
                JsonToString.Write(writer, "semanticVersion", item.SemanticVersion, default);
                JsonToString.Write(writer, "dottedQuadFileVersion", item.DottedQuadFileVersion, default);
                JsonToString.Write(writer, "releaseDateUtc", item.ReleaseDateUtc, default);
                JsonToUri.Write(writer, "downloadUri", item.DownloadUri, default);
                JsonToUri.Write(writer, "informationUri", item.InformationUri, default);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "globalMessageStrings", item.GlobalMessageStrings, JsonToMultiformatMessageString.Write);
                JsonToIList<ReportingDescriptor>.Write(writer, "notifications", item.Notifications, JsonToReportingDescriptor.Write);
                JsonToIList<ReportingDescriptor>.Write(writer, "rules", item.Rules, JsonToReportingDescriptor.Write);
                JsonToIList<ReportingDescriptor>.Write(writer, "taxa", item.Taxa, JsonToReportingDescriptor.Write);
                JsonToIList<ArtifactLocation>.Write(writer, "locations", item.Locations, JsonToArtifactLocation.Write);
                JsonToString.Write(writer, "language", item.Language, "en-US");
                JsonToEnum<ToolComponentContents>.Write(writer, "contents", item.Contents, default(ToolComponentContents));
                JsonToBool.Write(writer, "isComprehensive", item.IsComprehensive, false);
                JsonToString.Write(writer, "localizedDataSemanticVersion", item.LocalizedDataSemanticVersion, default);
                JsonToString.Write(writer, "minimumRequiredLocalizedDataSemanticVersion", item.MinimumRequiredLocalizedDataSemanticVersion, default);
                JsonToToolComponentReference.Write(writer, "associatedComponent", item.AssociatedComponent);
                JsonToTranslationMetadata.Write(writer, "translationMetadata", item.TranslationMetadata);
                JsonToIList<ToolComponentReference>.Write(writer, "supportedTaxonomies", item.SupportedTaxonomies, JsonToToolComponentReference.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ToolComponent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ToolComponent)value);
        }
    }
}
