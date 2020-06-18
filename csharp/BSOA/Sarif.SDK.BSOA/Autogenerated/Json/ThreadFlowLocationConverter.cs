using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ThreadFlowLocationConverter))]
    public partial class ThreadFlowLocation
    { }
    
    public class ThreadFlowLocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ThreadFlowLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadThreadFlowLocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ThreadFlowLocation)value);
        }
    }
    
    internal static class ThreadFlowLocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ThreadFlowLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ThreadFlowLocation>>()
        {
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["location"] = (reader, root, me) => me.Location = reader.ReadLocation(root),
            ["stack"] = (reader, root, me) => me.Stack = reader.ReadStack(root),
            ["kinds"] = (reader, root, me) => reader.ReadList(root, me.Kinds, JsonReaderExtensions.ReadString),
            ["taxa"] = (reader, root, me) => reader.ReadList(root, me.Taxa, ReportingDescriptorReferenceJsonExtensions.ReadReportingDescriptorReference),
            ["module"] = (reader, root, me) => me.Module = reader.ReadString(root),
            ["state"] = (reader, root, me) => reader.ReadDictionary(root, me.State, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["nestingLevel"] = (reader, root, me) => me.NestingLevel = reader.ReadInt(root),
            ["executionOrder"] = (reader, root, me) => me.ExecutionOrder = reader.ReadInt(root),
            ["executionTimeUtc"] = (reader, root, me) => me.ExecutionTimeUtc = reader.ReadDateTime(root),
            ["importance"] = (reader, root, me) => me.Importance = reader.ReadEnum<ThreadFlowLocationImportance, SarifLog>(root),
            ["webRequest"] = (reader, root, me) => me.WebRequest = reader.ReadWebRequest(root),
            ["webResponse"] = (reader, root, me) => me.WebResponse = reader.ReadWebResponse(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ThreadFlowLocation ReadThreadFlowLocation(this JsonReader reader, SarifLog root = null)
        {
            ThreadFlowLocation item = (root == null ? new ThreadFlowLocation() : new ThreadFlowLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ThreadFlowLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ThreadFlowLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("index", item.Index, -1);
                writer.Write("location", item.Location);
                writer.Write("stack", item.Stack);
                writer.Write("kinds", item.Kinds, default);
                writer.WriteList("taxa", item.Taxa, ReportingDescriptorReferenceJsonExtensions.Write);
                writer.Write("module", item.Module, default);
                writer.Write("state", item.State, default);
                writer.Write("nestingLevel", item.NestingLevel, default);
                writer.Write("executionOrder", item.ExecutionOrder, -1);
                writer.Write("executionTimeUtc", item.ExecutionTimeUtc, default);
                writer.WriteEnum("importance", item.Importance, ThreadFlowLocationImportance.Important);
                writer.Write("webRequest", item.WebRequest);
                writer.Write("webResponse", item.WebResponse);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
