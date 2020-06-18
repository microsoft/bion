using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ConfigurationOverrideConverter))]
    public partial class ConfigurationOverride
    { }
    
    public class ConfigurationOverrideConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ConfigurationOverride));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadConfigurationOverride();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ConfigurationOverride)value);
        }
    }
    
    internal static class ConfigurationOverrideJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ConfigurationOverride>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ConfigurationOverride>>()
        {
            ["configuration"] = (reader, root, me) => me.Configuration = reader.ReadReportingConfiguration(root),
            ["descriptor"] = (reader, root, me) => me.Descriptor = reader.ReadReportingDescriptorReference(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ConfigurationOverride ReadConfigurationOverride(this JsonReader reader, SarifLog root = null)
        {
            ConfigurationOverride item = (root == null ? new ConfigurationOverride() : new ConfigurationOverride(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ConfigurationOverride item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ConfigurationOverride item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("configuration", item.Configuration);
                writer.Write("descriptor", item.Descriptor);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
