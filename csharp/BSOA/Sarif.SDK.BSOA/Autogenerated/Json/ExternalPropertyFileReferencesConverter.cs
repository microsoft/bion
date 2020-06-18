using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ExternalPropertyFileReferencesConverter))]
    public partial class ExternalPropertyFileReferences
    { }
    
    public class ExternalPropertyFileReferencesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalPropertyFileReferences));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadExternalPropertyFileReferences();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ExternalPropertyFileReferences)value);
        }
    }
    
    internal static class ExternalPropertyFileReferencesJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReferences>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReferences>>()
        {
            ["conversion"] = (reader, root, me) => me.Conversion = reader.ReadExternalPropertyFileReference(root),
            ["graphs"] = (reader, root, me) => reader.ReadList(root, me.Graphs, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["externalizedProperties"] = (reader, root, me) => me.ExternalizedProperties = reader.ReadExternalPropertyFileReference(root),
            ["artifacts"] = (reader, root, me) => reader.ReadList(root, me.Artifacts, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["invocations"] = (reader, root, me) => reader.ReadList(root, me.Invocations, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["logicalLocations"] = (reader, root, me) => reader.ReadList(root, me.LogicalLocations, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["threadFlowLocations"] = (reader, root, me) => reader.ReadList(root, me.ThreadFlowLocations, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["results"] = (reader, root, me) => reader.ReadList(root, me.Results, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["taxonomies"] = (reader, root, me) => reader.ReadList(root, me.Taxonomies, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["addresses"] = (reader, root, me) => reader.ReadList(root, me.Addresses, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["driver"] = (reader, root, me) => me.Driver = reader.ReadExternalPropertyFileReference(root),
            ["extensions"] = (reader, root, me) => reader.ReadList(root, me.Extensions, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["policies"] = (reader, root, me) => reader.ReadList(root, me.Policies, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["translations"] = (reader, root, me) => reader.ReadList(root, me.Translations, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["webRequests"] = (reader, root, me) => reader.ReadList(root, me.WebRequests, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["webResponses"] = (reader, root, me) => reader.ReadList(root, me.WebResponses, ExternalPropertyFileReferenceJsonExtensions.ReadExternalPropertyFileReference),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ExternalPropertyFileReferences ReadExternalPropertyFileReferences(this JsonReader reader, SarifLog root = null)
        {
            ExternalPropertyFileReferences item = (root == null ? new ExternalPropertyFileReferences() : new ExternalPropertyFileReferences(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ExternalPropertyFileReferences item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ExternalPropertyFileReferences item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("conversion", item.Conversion);
                writer.WriteList("graphs", item.Graphs, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.Write("externalizedProperties", item.ExternalizedProperties);
                writer.WriteList("artifacts", item.Artifacts, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("invocations", item.Invocations, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("logicalLocations", item.LogicalLocations, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("threadFlowLocations", item.ThreadFlowLocations, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("results", item.Results, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("taxonomies", item.Taxonomies, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("addresses", item.Addresses, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.Write("driver", item.Driver);
                writer.WriteList("extensions", item.Extensions, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("policies", item.Policies, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("translations", item.Translations, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("webRequests", item.WebRequests, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.WriteList("webResponses", item.WebResponses, ExternalPropertyFileReferenceJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
