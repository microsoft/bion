using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToThreadFlowLocation))]
    public partial class ThreadFlowLocation
    { }
    
    internal class JsonToThreadFlowLocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ThreadFlowLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ThreadFlowLocation>>()
        {
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["location"] = (reader, root, me) => me.Location = JsonToLocation.Read(reader, root),
            ["stack"] = (reader, root, me) => me.Stack = JsonToStack.Read(reader, root),
            ["kinds"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Kinds, JsonToString.Read),
            ["taxa"] = (reader, root, me) => JsonToIList<ReportingDescriptorReference>.Read(reader, root, me.Taxa, JsonToReportingDescriptorReference.Read),
            ["module"] = (reader, root, me) => me.Module = JsonToString.Read(reader, root),
            ["state"] = (reader, root, me) => me.State = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["nestingLevel"] = (reader, root, me) => me.NestingLevel = JsonToInt.Read(reader, root),
            ["executionOrder"] = (reader, root, me) => me.ExecutionOrder = JsonToInt.Read(reader, root),
            ["executionTimeUtc"] = (reader, root, me) => me.ExecutionTimeUtc = JsonToDateTime.Read(reader, root),
            ["importance"] = (reader, root, me) => me.Importance = JsonToEnum<ThreadFlowLocationImportance>.Read(reader, root),
            ["webRequest"] = (reader, root, me) => me.WebRequest = JsonToWebRequest.Read(reader, root),
            ["webResponse"] = (reader, root, me) => me.WebResponse = JsonToWebResponse.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ThreadFlowLocation Read(JsonReader reader, SarifLog root = null)
        {
            ThreadFlowLocation item = (root == null ? new ThreadFlowLocation() : new ThreadFlowLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ThreadFlowLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ThreadFlowLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToLocation.Write(writer, "location", item.Location);
                JsonToStack.Write(writer, "stack", item.Stack);
                JsonToIList<String>.Write(writer, "kinds", item.Kinds, JsonToString.Write);
                JsonToIList<ReportingDescriptorReference>.Write(writer, "taxa", item.Taxa, JsonToReportingDescriptorReference.Write);
                JsonToString.Write(writer, "module", item.Module, default);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "state", item.State, JsonToMultiformatMessageString.Write);
                JsonToInt.Write(writer, "nestingLevel", item.NestingLevel, default);
                JsonToInt.Write(writer, "executionOrder", item.ExecutionOrder, -1);
                JsonToDateTime.Write(writer, "executionTimeUtc", item.ExecutionTimeUtc, default);
                JsonToEnum<ThreadFlowLocationImportance>.Write(writer, "importance", item.Importance, ThreadFlowLocationImportance.Important);
                JsonToWebRequest.Write(writer, "webRequest", item.WebRequest);
                JsonToWebResponse.Write(writer, "webResponse", item.WebResponse);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ThreadFlowLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ThreadFlowLocation)value);
        }
    }
}
