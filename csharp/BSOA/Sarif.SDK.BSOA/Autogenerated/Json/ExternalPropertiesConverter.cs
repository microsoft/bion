using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ExternalPropertiesConverter))]
    public partial class ExternalProperties
    { }
    
    public class ExternalPropertiesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalProperties));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadExternalProperties();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ExternalProperties)value);
        }
    }
    
    internal static class ExternalPropertiesJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalProperties>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalProperties>>()
        {
            ["schema"] = (reader, root, me) => me.Schema = reader.ReadUri(root),
            ["version"] = (reader, root, me) => me.Version = reader.ReadEnum<SarifVersion, SarifLog>(root),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["runGuid"] = (reader, root, me) => me.RunGuid = reader.ReadString(root),
            ["conversion"] = (reader, root, me) => me.Conversion = reader.ReadConversion(root),
            ["graphs"] = (reader, root, me) => reader.ReadList(root, me.Graphs, GraphJsonExtensions.ReadGraph),
            ["externalizedProperties"] = (reader, root, me) => me.ExternalizedProperties = reader.ReadPropertyBag(root),
            ["artifacts"] = (reader, root, me) => reader.ReadList(root, me.Artifacts, ArtifactJsonExtensions.ReadArtifact),
            ["invocations"] = (reader, root, me) => reader.ReadList(root, me.Invocations, InvocationJsonExtensions.ReadInvocation),
            ["logicalLocations"] = (reader, root, me) => reader.ReadList(root, me.LogicalLocations, LogicalLocationJsonExtensions.ReadLogicalLocation),
            ["threadFlowLocations"] = (reader, root, me) => reader.ReadList(root, me.ThreadFlowLocations, ThreadFlowLocationJsonExtensions.ReadThreadFlowLocation),
            ["results"] = (reader, root, me) => reader.ReadList(root, me.Results, ResultJsonExtensions.ReadResult),
            ["taxonomies"] = (reader, root, me) => reader.ReadList(root, me.Taxonomies, ToolComponentJsonExtensions.ReadToolComponent),
            ["driver"] = (reader, root, me) => me.Driver = reader.ReadToolComponent(root),
            ["extensions"] = (reader, root, me) => reader.ReadList(root, me.Extensions, ToolComponentJsonExtensions.ReadToolComponent),
            ["policies"] = (reader, root, me) => reader.ReadList(root, me.Policies, ToolComponentJsonExtensions.ReadToolComponent),
            ["translations"] = (reader, root, me) => reader.ReadList(root, me.Translations, ToolComponentJsonExtensions.ReadToolComponent),
            ["addresses"] = (reader, root, me) => reader.ReadList(root, me.Addresses, AddressJsonExtensions.ReadAddress),
            ["webRequests"] = (reader, root, me) => reader.ReadList(root, me.WebRequests, WebRequestJsonExtensions.ReadWebRequest),
            ["webResponses"] = (reader, root, me) => reader.ReadList(root, me.WebResponses, WebResponseJsonExtensions.ReadWebResponse),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ExternalProperties ReadExternalProperties(this JsonReader reader, SarifLog root = null)
        {
            ExternalProperties item = (root == null ? new ExternalProperties() : new ExternalProperties(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ExternalProperties item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ExternalProperties item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("schema", item.Schema, default(Uri));
                writer.Write("version", item.Version);
                writer.Write("guid", item.Guid, default(string));
                writer.Write("runGuid", item.RunGuid, default(string));
                writer.Write("conversion", item.Conversion);
                writer.WriteList("graphs", item.Graphs, GraphJsonExtensions.Write);
                writer.Write("externalizedProperties", item.ExternalizedProperties);
                writer.WriteList("artifacts", item.Artifacts, ArtifactJsonExtensions.Write);
                writer.WriteList("invocations", item.Invocations, InvocationJsonExtensions.Write);
                writer.WriteList("logicalLocations", item.LogicalLocations, LogicalLocationJsonExtensions.Write);
                writer.WriteList("threadFlowLocations", item.ThreadFlowLocations, ThreadFlowLocationJsonExtensions.Write);
                writer.WriteList("results", item.Results, ResultJsonExtensions.Write);
                writer.WriteList("taxonomies", item.Taxonomies, ToolComponentJsonExtensions.Write);
                writer.Write("driver", item.Driver);
                writer.WriteList("extensions", item.Extensions, ToolComponentJsonExtensions.Write);
                writer.WriteList("policies", item.Policies, ToolComponentJsonExtensions.Write);
                writer.WriteList("translations", item.Translations, ToolComponentJsonExtensions.Write);
                writer.WriteList("addresses", item.Addresses, AddressJsonExtensions.Write);
                writer.WriteList("webRequests", item.WebRequests, WebRequestJsonExtensions.Write);
                writer.WriteList("webResponses", item.WebResponses, WebResponseJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
