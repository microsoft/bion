using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ResultConverter))]
    public partial class Result
    { }
    
    public class ResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Result));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadResult();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Result)value);
        }
    }
    
    internal static class ResultJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Result>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Result>>()
        {
            ["ruleId"] = (reader, root, me) => me.RuleId = reader.ReadString(root),
            ["ruleIndex"] = (reader, root, me) => me.RuleIndex = reader.ReadInt(root),
            ["rule"] = (reader, root, me) => me.Rule = reader.ReadReportingDescriptorReference(root),
            ["kind"] = (reader, root, me) => me.Kind = reader.ReadEnum<ResultKind, SarifLog>(root),
            ["level"] = (reader, root, me) => me.Level = reader.ReadEnum<FailureLevel, SarifLog>(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["analysisTarget"] = (reader, root, me) => me.AnalysisTarget = reader.ReadArtifactLocation(root),
            ["locations"] = (reader, root, me) => reader.ReadList(root, me.Locations, LocationJsonExtensions.ReadLocation),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["correlationGuid"] = (reader, root, me) => me.CorrelationGuid = reader.ReadString(root),
            ["occurrenceCount"] = (reader, root, me) => me.OccurrenceCount = reader.ReadInt(root),
            ["partialFingerprints"] = (reader, root, me) => reader.ReadDictionary(root, me.PartialFingerprints, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["fingerprints"] = (reader, root, me) => reader.ReadDictionary(root, me.Fingerprints, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["stacks"] = (reader, root, me) => reader.ReadList(root, me.Stacks, StackJsonExtensions.ReadStack),
            ["codeFlows"] = (reader, root, me) => reader.ReadList(root, me.CodeFlows, CodeFlowJsonExtensions.ReadCodeFlow),
            ["graphs"] = (reader, root, me) => reader.ReadList(root, me.Graphs, GraphJsonExtensions.ReadGraph),
            ["graphTraversals"] = (reader, root, me) => reader.ReadList(root, me.GraphTraversals, GraphTraversalJsonExtensions.ReadGraphTraversal),
            ["relatedLocations"] = (reader, root, me) => reader.ReadList(root, me.RelatedLocations, LocationJsonExtensions.ReadLocation),
            ["suppressions"] = (reader, root, me) => reader.ReadList(root, me.Suppressions, SuppressionJsonExtensions.ReadSuppression),
            ["baselineState"] = (reader, root, me) => me.BaselineState = reader.ReadEnum<BaselineState, SarifLog>(root),
            ["rank"] = (reader, root, me) => me.Rank = reader.ReadDouble(root),
            ["attachments"] = (reader, root, me) => reader.ReadList(root, me.Attachments, AttachmentJsonExtensions.ReadAttachment),
            ["hostedViewerUri"] = (reader, root, me) => me.HostedViewerUri = reader.ReadUri(root),
            ["workItemUris"] = (reader, root, me) => reader.ReadList(root, me.WorkItemUris, JsonReaderExtensions.ReadUri),
            ["provenance"] = (reader, root, me) => me.Provenance = reader.ReadResultProvenance(root),
            ["fixes"] = (reader, root, me) => reader.ReadList(root, me.Fixes, FixJsonExtensions.ReadFix),
            ["taxa"] = (reader, root, me) => reader.ReadList(root, me.Taxa, ReportingDescriptorReferenceJsonExtensions.ReadReportingDescriptorReference),
            ["webRequest"] = (reader, root, me) => me.WebRequest = reader.ReadWebRequest(root),
            ["webResponse"] = (reader, root, me) => me.WebResponse = reader.ReadWebResponse(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static Result ReadResult(this JsonReader reader, SarifLog root = null)
        {
            Result item = (root == null ? new Result() : new Result(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Result item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Result item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("ruleId", item.RuleId, default);
                writer.Write("ruleIndex", item.RuleIndex, -1);
                writer.Write("rule", item.Rule);
                writer.WriteEnum("kind", item.Kind, ResultKind.Fail);
                writer.WriteEnum("level", item.Level, FailureLevel.Warning);
                writer.Write("message", item.Message);
                writer.Write("analysisTarget", item.AnalysisTarget);
                writer.WriteList("locations", item.Locations, LocationJsonExtensions.Write);
                writer.Write("guid", item.Guid, default);
                writer.Write("correlationGuid", item.CorrelationGuid, default);
                writer.Write("occurrenceCount", item.OccurrenceCount, default);
                writer.Write("partialFingerprints", item.PartialFingerprints, default);
                writer.Write("fingerprints", item.Fingerprints, default);
                writer.WriteList("stacks", item.Stacks, StackJsonExtensions.Write);
                writer.WriteList("codeFlows", item.CodeFlows, CodeFlowJsonExtensions.Write);
                writer.WriteList("graphs", item.Graphs, GraphJsonExtensions.Write);
                writer.WriteList("graphTraversals", item.GraphTraversals, GraphTraversalJsonExtensions.Write);
                writer.WriteList("relatedLocations", item.RelatedLocations, LocationJsonExtensions.Write);
                writer.WriteList("suppressions", item.Suppressions, SuppressionJsonExtensions.Write);
                writer.WriteEnum("baselineState", item.BaselineState, default(BaselineState));
                writer.Write("rank", item.Rank, -1);
                writer.WriteList("attachments", item.Attachments, AttachmentJsonExtensions.Write);
                writer.Write("hostedViewerUri", item.HostedViewerUri, default);
                writer.Write("workItemUris", item.WorkItemUris, default);
                writer.Write("provenance", item.Provenance);
                writer.WriteList("fixes", item.Fixes, FixJsonExtensions.Write);
                writer.WriteList("taxa", item.Taxa, ReportingDescriptorReferenceJsonExtensions.Write);
                writer.Write("webRequest", item.WebRequest);
                writer.Write("webResponse", item.WebResponse);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
