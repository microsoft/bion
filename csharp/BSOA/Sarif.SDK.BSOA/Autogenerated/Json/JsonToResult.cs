using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToResult))]
    public partial class Result
    { }
    
    internal class JsonToResult : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Result>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Result>>()
        {
            ["ruleId"] = (reader, root, me) => me.RuleId = JsonToString.Read(reader, root),
            ["ruleIndex"] = (reader, root, me) => me.RuleIndex = JsonToInt.Read(reader, root),
            ["rule"] = (reader, root, me) => me.Rule = JsonToReportingDescriptorReference.Read(reader, root),
            ["kind"] = (reader, root, me) => me.Kind = JsonToEnum<ResultKind>.Read(reader, root),
            ["level"] = (reader, root, me) => me.Level = JsonToEnum<FailureLevel>.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["analysisTarget"] = (reader, root, me) => me.AnalysisTarget = JsonToArtifactLocation.Read(reader, root),
            ["locations"] = (reader, root, me) => JsonToIList<Location>.Read(reader, root, me.Locations, JsonToLocation.Read),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["correlationGuid"] = (reader, root, me) => me.CorrelationGuid = JsonToString.Read(reader, root),
            ["occurrenceCount"] = (reader, root, me) => me.OccurrenceCount = JsonToInt.Read(reader, root),
            ["partialFingerprints"] = (reader, root, me) => me.PartialFingerprints = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["fingerprints"] = (reader, root, me) => me.Fingerprints = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["stacks"] = (reader, root, me) => JsonToIList<Stack>.Read(reader, root, me.Stacks, JsonToStack.Read),
            ["codeFlows"] = (reader, root, me) => JsonToIList<CodeFlow>.Read(reader, root, me.CodeFlows, JsonToCodeFlow.Read),
            ["graphs"] = (reader, root, me) => JsonToIList<Graph>.Read(reader, root, me.Graphs, JsonToGraph.Read),
            ["graphTraversals"] = (reader, root, me) => JsonToIList<GraphTraversal>.Read(reader, root, me.GraphTraversals, JsonToGraphTraversal.Read),
            ["relatedLocations"] = (reader, root, me) => JsonToIList<Location>.Read(reader, root, me.RelatedLocations, JsonToLocation.Read),
            ["suppressions"] = (reader, root, me) => JsonToIList<Suppression>.Read(reader, root, me.Suppressions, JsonToSuppression.Read),
            ["baselineState"] = (reader, root, me) => me.BaselineState = JsonToEnum<BaselineState>.Read(reader, root),
            ["rank"] = (reader, root, me) => me.Rank = JsonToDouble.Read(reader, root),
            ["attachments"] = (reader, root, me) => JsonToIList<Attachment>.Read(reader, root, me.Attachments, JsonToAttachment.Read),
            ["hostedViewerUri"] = (reader, root, me) => me.HostedViewerUri = JsonToUri.Read(reader, root),
            ["workItemUris"] = (reader, root, me) => JsonToIList<Uri>.Read(reader, root, me.WorkItemUris, JsonToUri.Read),
            ["provenance"] = (reader, root, me) => me.Provenance = JsonToResultProvenance.Read(reader, root),
            ["fixes"] = (reader, root, me) => JsonToIList<Fix>.Read(reader, root, me.Fixes, JsonToFix.Read),
            ["taxa"] = (reader, root, me) => JsonToIList<ReportingDescriptorReference>.Read(reader, root, me.Taxa, JsonToReportingDescriptorReference.Read),
            ["webRequest"] = (reader, root, me) => me.WebRequest = JsonToWebRequest.Read(reader, root),
            ["webResponse"] = (reader, root, me) => me.WebResponse = JsonToWebResponse.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Result Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Result item = (root == null ? new Result() : new Result(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Result item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Result item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "ruleId", item.RuleId, default);
                JsonToInt.Write(writer, "ruleIndex", item.RuleIndex, -1);
                JsonToReportingDescriptorReference.Write(writer, "rule", item.Rule);
                JsonToEnum<ResultKind>.Write(writer, "kind", item.Kind, ResultKind.Fail);
                JsonToEnum<FailureLevel>.Write(writer, "level", item.Level, FailureLevel.Warning);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToArtifactLocation.Write(writer, "analysisTarget", item.AnalysisTarget);
                JsonToIList<Location>.Write(writer, "locations", item.Locations, JsonToLocation.Write);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToString.Write(writer, "correlationGuid", item.CorrelationGuid, default);
                JsonToInt.Write(writer, "occurrenceCount", item.OccurrenceCount, default);
                JsonToIDictionary<String, String>.Write(writer, "partialFingerprints", item.PartialFingerprints, JsonToString.Write);
                JsonToIDictionary<String, String>.Write(writer, "fingerprints", item.Fingerprints, JsonToString.Write);
                JsonToIList<Stack>.Write(writer, "stacks", item.Stacks, JsonToStack.Write);
                JsonToIList<CodeFlow>.Write(writer, "codeFlows", item.CodeFlows, JsonToCodeFlow.Write);
                JsonToIList<Graph>.Write(writer, "graphs", item.Graphs, JsonToGraph.Write);
                JsonToIList<GraphTraversal>.Write(writer, "graphTraversals", item.GraphTraversals, JsonToGraphTraversal.Write);
                JsonToIList<Location>.Write(writer, "relatedLocations", item.RelatedLocations, JsonToLocation.Write);
                JsonToIList<Suppression>.Write(writer, "suppressions", item.Suppressions, JsonToSuppression.Write);
                JsonToEnum<BaselineState>.Write(writer, "baselineState", item.BaselineState, default(BaselineState));
                JsonToDouble.Write(writer, "rank", item.Rank, -1);
                JsonToIList<Attachment>.Write(writer, "attachments", item.Attachments, JsonToAttachment.Write);
                JsonToUri.Write(writer, "hostedViewerUri", item.HostedViewerUri, default);
                JsonToIList<Uri>.Write(writer, "workItemUris", item.WorkItemUris, JsonToUri.Write);
                JsonToResultProvenance.Write(writer, "provenance", item.Provenance);
                JsonToIList<Fix>.Write(writer, "fixes", item.Fixes, JsonToFix.Write);
                JsonToIList<ReportingDescriptorReference>.Write(writer, "taxa", item.Taxa, JsonToReportingDescriptorReference.Write);
                JsonToWebRequest.Write(writer, "webRequest", item.WebRequest);
                JsonToWebResponse.Write(writer, "webResponse", item.WebResponse);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Result));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Result)value);
        }
    }
}
