using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToReportingConfiguration))]
    public partial class ReportingConfiguration
    { }
    
    internal class JsonToReportingConfiguration : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingConfiguration>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingConfiguration>>()
        {
            ["enabled"] = (reader, root, me) => me.Enabled = JsonToBool.Read(reader, root),
            ["level"] = (reader, root, me) => me.Level = JsonToEnum<FailureLevel>.Read(reader, root),
            ["rank"] = (reader, root, me) => me.Rank = JsonToDouble.Read(reader, root),
            ["parameters"] = (reader, root, me) => me.Parameters = JsonToPropertyBag.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ReportingConfiguration Read(JsonReader reader, SarifLog root = null)
        {
            ReportingConfiguration item = (root == null ? new ReportingConfiguration() : new ReportingConfiguration(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ReportingConfiguration item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ReportingConfiguration item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToBool.Write(writer, "enabled", item.Enabled, true);
                JsonToEnum<FailureLevel>.Write(writer, "level", item.Level, FailureLevel.Warning);
                JsonToDouble.Write(writer, "rank", item.Rank, -1);
                JsonToPropertyBag.Write(writer, "parameters", item.Parameters);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingConfiguration));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ReportingConfiguration)value);
        }
    }
}
