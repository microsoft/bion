using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToRun))]
    public partial class Run
    { }
    
    internal class JsonToRun : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Run>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Run>>()
        {
            ["tool"] = (reader, root, me) => me.Tool = JsonToTool.Read(reader, root),
            ["invocations"] = (reader, root, me) => JsonToIList<Invocation>.Read(reader, root, me.Invocations, JsonToInvocation.Read),
            ["conversion"] = (reader, root, me) => me.Conversion = JsonToConversion.Read(reader, root),
            ["language"] = (reader, root, me) => me.Language = JsonToString.Read(reader, root),
            ["versionControlProvenance"] = (reader, root, me) => JsonToIList<VersionControlDetails>.Read(reader, root, me.VersionControlProvenance, JsonToVersionControlDetails.Read),
            ["originalUriBaseIds"] = (reader, root, me) => me.OriginalUriBaseIds = JsonToIDictionary<String, ArtifactLocation>.Read(reader, root, null, JsonToArtifactLocation.Read),
            ["artifacts"] = (reader, root, me) => JsonToIList<Artifact>.Read(reader, root, me.Artifacts, JsonToArtifact.Read),
            ["logicalLocations"] = (reader, root, me) => JsonToIList<LogicalLocation>.Read(reader, root, me.LogicalLocations, JsonToLogicalLocation.Read),
            ["graphs"] = (reader, root, me) => JsonToIList<Graph>.Read(reader, root, me.Graphs, JsonToGraph.Read),
            ["results"] = (reader, root, me) => JsonToIList<Result>.Read(reader, root, me.Results, JsonToResult.Read),
            ["automationDetails"] = (reader, root, me) => me.AutomationDetails = JsonToRunAutomationDetails.Read(reader, root),
            ["runAggregates"] = (reader, root, me) => JsonToIList<RunAutomationDetails>.Read(reader, root, me.RunAggregates, JsonToRunAutomationDetails.Read),
            ["baselineGuid"] = (reader, root, me) => me.BaselineGuid = JsonToString.Read(reader, root),
            ["redactionTokens"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.RedactionTokens, JsonToString.Read),
            ["defaultEncoding"] = (reader, root, me) => me.DefaultEncoding = JsonToString.Read(reader, root),
            ["defaultSourceLanguage"] = (reader, root, me) => me.DefaultSourceLanguage = JsonToString.Read(reader, root),
            ["newlineSequences"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.NewlineSequences, JsonToString.Read),
            ["columnKind"] = (reader, root, me) => me.ColumnKind = JsonToEnum<ColumnKind>.Read(reader, root),
            ["externalPropertyFileReferences"] = (reader, root, me) => me.ExternalPropertyFileReferences = JsonToExternalPropertyFileReferences.Read(reader, root),
            ["threadFlowLocations"] = (reader, root, me) => JsonToIList<ThreadFlowLocation>.Read(reader, root, me.ThreadFlowLocations, JsonToThreadFlowLocation.Read),
            ["taxonomies"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Taxonomies, JsonToToolComponent.Read),
            ["addresses"] = (reader, root, me) => JsonToIList<Address>.Read(reader, root, me.Addresses, JsonToAddress.Read),
            ["translations"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Translations, JsonToToolComponent.Read),
            ["policies"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Policies, JsonToToolComponent.Read),
            ["webRequests"] = (reader, root, me) => JsonToIList<WebRequest>.Read(reader, root, me.WebRequests, JsonToWebRequest.Read),
            ["webResponses"] = (reader, root, me) => JsonToIList<WebResponse>.Read(reader, root, me.WebResponses, JsonToWebResponse.Read),
            ["specialLocations"] = (reader, root, me) => me.SpecialLocations = JsonToSpecialLocations.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Run Read(JsonReader reader, SarifLog root = null)
        {
            Run item = (root == null ? new Run() : new Run(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Run item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Run item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToTool.Write(writer, "tool", item.Tool);
                JsonToIList<Invocation>.Write(writer, "invocations", item.Invocations, JsonToInvocation.Write);
                JsonToConversion.Write(writer, "conversion", item.Conversion);
                JsonToString.Write(writer, "language", item.Language, "en-US");
                JsonToIList<VersionControlDetails>.Write(writer, "versionControlProvenance", item.VersionControlProvenance, JsonToVersionControlDetails.Write);
                JsonToIDictionary<String, ArtifactLocation>.Write(writer, "originalUriBaseIds", item.OriginalUriBaseIds, JsonToArtifactLocation.Write);
                JsonToIList<Artifact>.Write(writer, "artifacts", item.Artifacts, JsonToArtifact.Write);
                JsonToIList<LogicalLocation>.Write(writer, "logicalLocations", item.LogicalLocations, JsonToLogicalLocation.Write);
                JsonToIList<Graph>.Write(writer, "graphs", item.Graphs, JsonToGraph.Write);
                JsonToIList<Result>.Write(writer, "results", item.Results, JsonToResult.Write);
                JsonToRunAutomationDetails.Write(writer, "automationDetails", item.AutomationDetails);
                JsonToIList<RunAutomationDetails>.Write(writer, "runAggregates", item.RunAggregates, JsonToRunAutomationDetails.Write);
                JsonToString.Write(writer, "baselineGuid", item.BaselineGuid, default);
                JsonToIList<String>.Write(writer, "redactionTokens", item.RedactionTokens, JsonToString.Write);
                JsonToString.Write(writer, "defaultEncoding", item.DefaultEncoding, default);
                JsonToString.Write(writer, "defaultSourceLanguage", item.DefaultSourceLanguage, default);
                JsonToIList<String>.Write(writer, "newlineSequences", item.NewlineSequences, JsonToString.Write);
                JsonToEnum<ColumnKind>.Write(writer, "columnKind", item.ColumnKind, default(ColumnKind));
                JsonToExternalPropertyFileReferences.Write(writer, "externalPropertyFileReferences", item.ExternalPropertyFileReferences);
                JsonToIList<ThreadFlowLocation>.Write(writer, "threadFlowLocations", item.ThreadFlowLocations, JsonToThreadFlowLocation.Write);
                JsonToIList<ToolComponent>.Write(writer, "taxonomies", item.Taxonomies, JsonToToolComponent.Write);
                JsonToIList<Address>.Write(writer, "addresses", item.Addresses, JsonToAddress.Write);
                JsonToIList<ToolComponent>.Write(writer, "translations", item.Translations, JsonToToolComponent.Write);
                JsonToIList<ToolComponent>.Write(writer, "policies", item.Policies, JsonToToolComponent.Write);
                JsonToIList<WebRequest>.Write(writer, "webRequests", item.WebRequests, JsonToWebRequest.Write);
                JsonToIList<WebResponse>.Write(writer, "webResponses", item.WebResponses, JsonToWebResponse.Write);
                JsonToSpecialLocations.Write(writer, "specialLocations", item.SpecialLocations);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Run));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Run)value);
        }
    }
}
