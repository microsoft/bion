using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToExternalProperties))]
    public partial class ExternalProperties
    { }
    
    internal class JsonToExternalProperties : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalProperties>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalProperties>>()
        {
            ["schema"] = (reader, root, me) => me.Schema = JsonToUri.Read(reader, root),
            ["version"] = (reader, root, me) => me.Version = JsonToEnum<SarifVersion>.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["runGuid"] = (reader, root, me) => me.RunGuid = JsonToString.Read(reader, root),
            ["conversion"] = (reader, root, me) => me.Conversion = JsonToConversion.Read(reader, root),
            ["graphs"] = (reader, root, me) => JsonToIList<Graph>.Read(reader, root, me.Graphs, JsonToGraph.Read),
            ["externalizedProperties"] = (reader, root, me) => me.ExternalizedProperties = JsonToPropertyBag.Read(reader, root),
            ["artifacts"] = (reader, root, me) => JsonToIList<Artifact>.Read(reader, root, me.Artifacts, JsonToArtifact.Read),
            ["invocations"] = (reader, root, me) => JsonToIList<Invocation>.Read(reader, root, me.Invocations, JsonToInvocation.Read),
            ["logicalLocations"] = (reader, root, me) => JsonToIList<LogicalLocation>.Read(reader, root, me.LogicalLocations, JsonToLogicalLocation.Read),
            ["threadFlowLocations"] = (reader, root, me) => JsonToIList<ThreadFlowLocation>.Read(reader, root, me.ThreadFlowLocations, JsonToThreadFlowLocation.Read),
            ["results"] = (reader, root, me) => JsonToIList<Result>.Read(reader, root, me.Results, JsonToResult.Read),
            ["taxonomies"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Taxonomies, JsonToToolComponent.Read),
            ["driver"] = (reader, root, me) => me.Driver = JsonToToolComponent.Read(reader, root),
            ["extensions"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Extensions, JsonToToolComponent.Read),
            ["policies"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Policies, JsonToToolComponent.Read),
            ["translations"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Translations, JsonToToolComponent.Read),
            ["addresses"] = (reader, root, me) => JsonToIList<Address>.Read(reader, root, me.Addresses, JsonToAddress.Read),
            ["webRequests"] = (reader, root, me) => JsonToIList<WebRequest>.Read(reader, root, me.WebRequests, JsonToWebRequest.Read),
            ["webResponses"] = (reader, root, me) => JsonToIList<WebResponse>.Read(reader, root, me.WebResponses, JsonToWebResponse.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ExternalProperties Read(JsonReader reader, SarifLog root = null)
        {
            ExternalProperties item = (root == null ? new ExternalProperties() : new ExternalProperties(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ExternalProperties item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ExternalProperties item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToUri.Write(writer, "schema", item.Schema, default);
                JsonToEnum<SarifVersion>.Write(writer, "version", item.Version, default(SarifVersion));
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToString.Write(writer, "runGuid", item.RunGuid, default);
                JsonToConversion.Write(writer, "conversion", item.Conversion);
                JsonToIList<Graph>.Write(writer, "graphs", item.Graphs, JsonToGraph.Write);
                JsonToPropertyBag.Write(writer, "externalizedProperties", item.ExternalizedProperties);
                JsonToIList<Artifact>.Write(writer, "artifacts", item.Artifacts, JsonToArtifact.Write);
                JsonToIList<Invocation>.Write(writer, "invocations", item.Invocations, JsonToInvocation.Write);
                JsonToIList<LogicalLocation>.Write(writer, "logicalLocations", item.LogicalLocations, JsonToLogicalLocation.Write);
                JsonToIList<ThreadFlowLocation>.Write(writer, "threadFlowLocations", item.ThreadFlowLocations, JsonToThreadFlowLocation.Write);
                JsonToIList<Result>.Write(writer, "results", item.Results, JsonToResult.Write);
                JsonToIList<ToolComponent>.Write(writer, "taxonomies", item.Taxonomies, JsonToToolComponent.Write);
                JsonToToolComponent.Write(writer, "driver", item.Driver);
                JsonToIList<ToolComponent>.Write(writer, "extensions", item.Extensions, JsonToToolComponent.Write);
                JsonToIList<ToolComponent>.Write(writer, "policies", item.Policies, JsonToToolComponent.Write);
                JsonToIList<ToolComponent>.Write(writer, "translations", item.Translations, JsonToToolComponent.Write);
                JsonToIList<Address>.Write(writer, "addresses", item.Addresses, JsonToAddress.Write);
                JsonToIList<WebRequest>.Write(writer, "webRequests", item.WebRequests, JsonToWebRequest.Write);
                JsonToIList<WebResponse>.Write(writer, "webResponses", item.WebResponses, JsonToWebResponse.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalProperties));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ExternalProperties)value);
        }
    }
}
