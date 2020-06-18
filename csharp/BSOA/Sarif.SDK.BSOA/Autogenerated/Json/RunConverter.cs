using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(RunConverter))]
    public partial class Run
    { }
    
    public class RunConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Run));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadRun();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Run)value);
        }
    }
    
    internal static class RunJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Run>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Run>>()
        {
            ["tool"] = (reader, root, me) => me.Tool = reader.ReadTool(root),
            ["invocations"] = (reader, root, me) => reader.ReadList(root, me.Invocations, InvocationJsonExtensions.ReadInvocation),
            ["conversion"] = (reader, root, me) => me.Conversion = reader.ReadConversion(root),
            ["language"] = (reader, root, me) => me.Language = reader.ReadString(root),
            ["versionControlProvenance"] = (reader, root, me) => reader.ReadList(root, me.VersionControlProvenance, VersionControlDetailsJsonExtensions.ReadVersionControlDetails),
            ["originalUriBaseIds"] = (reader, root, me) => reader.ReadDictionary(root, me.OriginalUriBaseIds, JsonReaderExtensions.ReadString, ArtifactLocationJsonExtensions.ReadArtifactLocation),
            ["artifacts"] = (reader, root, me) => reader.ReadList(root, me.Artifacts, ArtifactJsonExtensions.ReadArtifact),
            ["logicalLocations"] = (reader, root, me) => reader.ReadList(root, me.LogicalLocations, LogicalLocationJsonExtensions.ReadLogicalLocation),
            ["graphs"] = (reader, root, me) => reader.ReadList(root, me.Graphs, GraphJsonExtensions.ReadGraph),
            ["results"] = (reader, root, me) => reader.ReadList(root, me.Results, ResultJsonExtensions.ReadResult),
            ["automationDetails"] = (reader, root, me) => me.AutomationDetails = reader.ReadRunAutomationDetails(root),
            ["runAggregates"] = (reader, root, me) => reader.ReadList(root, me.RunAggregates, RunAutomationDetailsJsonExtensions.ReadRunAutomationDetails),
            ["baselineGuid"] = (reader, root, me) => me.BaselineGuid = reader.ReadString(root),
            ["redactionTokens"] = (reader, root, me) => reader.ReadList(root, me.RedactionTokens, JsonReaderExtensions.ReadString),
            ["defaultEncoding"] = (reader, root, me) => me.DefaultEncoding = reader.ReadString(root),
            ["defaultSourceLanguage"] = (reader, root, me) => me.DefaultSourceLanguage = reader.ReadString(root),
            ["newlineSequences"] = (reader, root, me) => reader.ReadList(root, me.NewlineSequences, JsonReaderExtensions.ReadString),
            ["columnKind"] = (reader, root, me) => me.ColumnKind = reader.ReadEnum<ColumnKind, SarifLog>(root),
            ["externalPropertyFileReferences"] = (reader, root, me) => me.ExternalPropertyFileReferences = reader.ReadExternalPropertyFileReferences(root),
            ["threadFlowLocations"] = (reader, root, me) => reader.ReadList(root, me.ThreadFlowLocations, ThreadFlowLocationJsonExtensions.ReadThreadFlowLocation),
            ["taxonomies"] = (reader, root, me) => reader.ReadList(root, me.Taxonomies, ToolComponentJsonExtensions.ReadToolComponent),
            ["addresses"] = (reader, root, me) => reader.ReadList(root, me.Addresses, AddressJsonExtensions.ReadAddress),
            ["translations"] = (reader, root, me) => reader.ReadList(root, me.Translations, ToolComponentJsonExtensions.ReadToolComponent),
            ["policies"] = (reader, root, me) => reader.ReadList(root, me.Policies, ToolComponentJsonExtensions.ReadToolComponent),
            ["webRequests"] = (reader, root, me) => reader.ReadList(root, me.WebRequests, WebRequestJsonExtensions.ReadWebRequest),
            ["webResponses"] = (reader, root, me) => reader.ReadList(root, me.WebResponses, WebResponseJsonExtensions.ReadWebResponse),
            ["specialLocations"] = (reader, root, me) => me.SpecialLocations = reader.ReadSpecialLocations(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Run ReadRun(this JsonReader reader, SarifLog root = null)
        {
            Run item = (root == null ? new Run() : new Run(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Run item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Run item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("tool", item.Tool);
                writer.WriteList("invocations", item.Invocations, InvocationJsonExtensions.Write);
                writer.Write("conversion", item.Conversion);
                writer.Write("language", item.Language, "en-US");
                writer.WriteList("versionControlProvenance", item.VersionControlProvenance, VersionControlDetailsJsonExtensions.Write);
                writer.Write("originalUriBaseIds", item.OriginalUriBaseIds, default(IDictionary<string, ArtifactLocation>));
                writer.WriteList("artifacts", item.Artifacts, ArtifactJsonExtensions.Write);
                writer.WriteList("logicalLocations", item.LogicalLocations, LogicalLocationJsonExtensions.Write);
                writer.WriteList("graphs", item.Graphs, GraphJsonExtensions.Write);
                writer.WriteList("results", item.Results, ResultJsonExtensions.Write);
                writer.Write("automationDetails", item.AutomationDetails);
                writer.WriteList("runAggregates", item.RunAggregates, RunAutomationDetailsJsonExtensions.Write);
                writer.Write("baselineGuid", item.BaselineGuid, default(string));
                writer.Write("redactionTokens", item.RedactionTokens, default(IList<string>));
                writer.Write("defaultEncoding", item.DefaultEncoding, default(string));
                writer.Write("defaultSourceLanguage", item.DefaultSourceLanguage, default(string));
                writer.Write("newlineSequences", item.NewlineSequences, default(IList<string>));
                writer.Write("columnKind", item.ColumnKind);
                writer.Write("externalPropertyFileReferences", item.ExternalPropertyFileReferences);
                writer.WriteList("threadFlowLocations", item.ThreadFlowLocations, ThreadFlowLocationJsonExtensions.Write);
                writer.WriteList("taxonomies", item.Taxonomies, ToolComponentJsonExtensions.Write);
                writer.WriteList("addresses", item.Addresses, AddressJsonExtensions.Write);
                writer.WriteList("translations", item.Translations, ToolComponentJsonExtensions.Write);
                writer.WriteList("policies", item.Policies, ToolComponentJsonExtensions.Write);
                writer.WriteList("webRequests", item.WebRequests, WebRequestJsonExtensions.Write);
                writer.WriteList("webResponses", item.WebResponses, WebResponseJsonExtensions.Write);
                writer.Write("specialLocations", item.SpecialLocations);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
