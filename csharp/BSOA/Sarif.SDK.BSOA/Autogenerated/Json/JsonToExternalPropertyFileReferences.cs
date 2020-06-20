using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToExternalPropertyFileReferences))]
    public partial class ExternalPropertyFileReferences
    { }
    
    internal class JsonToExternalPropertyFileReferences : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReferences>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReferences>>()
        {
            ["conversion"] = (reader, root, me) => me.Conversion = JsonToExternalPropertyFileReference.Read(reader, root),
            ["graphs"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Graphs, JsonToExternalPropertyFileReference.Read),
            ["externalizedProperties"] = (reader, root, me) => me.ExternalizedProperties = JsonToExternalPropertyFileReference.Read(reader, root),
            ["artifacts"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Artifacts, JsonToExternalPropertyFileReference.Read),
            ["invocations"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Invocations, JsonToExternalPropertyFileReference.Read),
            ["logicalLocations"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.LogicalLocations, JsonToExternalPropertyFileReference.Read),
            ["threadFlowLocations"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.ThreadFlowLocations, JsonToExternalPropertyFileReference.Read),
            ["results"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Results, JsonToExternalPropertyFileReference.Read),
            ["taxonomies"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Taxonomies, JsonToExternalPropertyFileReference.Read),
            ["addresses"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Addresses, JsonToExternalPropertyFileReference.Read),
            ["driver"] = (reader, root, me) => me.Driver = JsonToExternalPropertyFileReference.Read(reader, root),
            ["extensions"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Extensions, JsonToExternalPropertyFileReference.Read),
            ["policies"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Policies, JsonToExternalPropertyFileReference.Read),
            ["translations"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.Translations, JsonToExternalPropertyFileReference.Read),
            ["webRequests"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.WebRequests, JsonToExternalPropertyFileReference.Read),
            ["webResponses"] = (reader, root, me) => JsonToIList<ExternalPropertyFileReference>.Read(reader, root, me.WebResponses, JsonToExternalPropertyFileReference.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ExternalPropertyFileReferences Read(JsonReader reader, SarifLog root = null)
        {
            ExternalPropertyFileReferences item = (root == null ? new ExternalPropertyFileReferences() : new ExternalPropertyFileReferences(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ExternalPropertyFileReferences item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ExternalPropertyFileReferences item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToExternalPropertyFileReference.Write(writer, "conversion", item.Conversion);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "graphs", item.Graphs, JsonToExternalPropertyFileReference.Write);
                JsonToExternalPropertyFileReference.Write(writer, "externalizedProperties", item.ExternalizedProperties);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "artifacts", item.Artifacts, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "invocations", item.Invocations, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "logicalLocations", item.LogicalLocations, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "threadFlowLocations", item.ThreadFlowLocations, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "results", item.Results, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "taxonomies", item.Taxonomies, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "addresses", item.Addresses, JsonToExternalPropertyFileReference.Write);
                JsonToExternalPropertyFileReference.Write(writer, "driver", item.Driver);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "extensions", item.Extensions, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "policies", item.Policies, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "translations", item.Translations, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "webRequests", item.WebRequests, JsonToExternalPropertyFileReference.Write);
                JsonToIList<ExternalPropertyFileReference>.Write(writer, "webResponses", item.WebResponses, JsonToExternalPropertyFileReference.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalPropertyFileReferences));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ExternalPropertyFileReferences)value);
        }
    }
}
