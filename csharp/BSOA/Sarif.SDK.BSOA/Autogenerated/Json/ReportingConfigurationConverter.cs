using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ReportingConfigurationConverter))]
    public partial class ReportingConfiguration
    { }
    
    public class ReportingConfigurationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ReportingConfiguration));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadReportingConfiguration();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ReportingConfiguration)value);
        }
    }
    
    internal static class ReportingConfigurationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ReportingConfiguration>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ReportingConfiguration>>()
        {
            ["enabled"] = (reader, root, me) => me.Enabled = reader.ReadBool(root),
            ["level"] = (reader, root, me) => me.Level = reader.ReadEnum<FailureLevel, SarifLog>(root),
            ["rank"] = (reader, root, me) => me.Rank = reader.ReadDouble(root),
            ["parameters"] = (reader, root, me) => me.Parameters = reader.ReadPropertyBag(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ReportingConfiguration ReadReportingConfiguration(this JsonReader reader, SarifLog root = null)
        {
            ReportingConfiguration item = (root == null ? new ReportingConfiguration() : new ReportingConfiguration(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ReportingConfiguration item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ReportingConfiguration item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("enabled", item.Enabled, true);
                writer.Write("level", item.Level);
                writer.Write("rank", item.Rank, -1);
                writer.Write("parameters", item.Parameters);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
