using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToConfigurationOverride))]
    public partial class ConfigurationOverride
    { }
    
    internal class JsonToConfigurationOverride : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ConfigurationOverride>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ConfigurationOverride>>()
        {
            ["configuration"] = (reader, root, me) => me.Configuration = JsonToReportingConfiguration.Read(reader, root),
            ["descriptor"] = (reader, root, me) => me.Descriptor = JsonToReportingDescriptorReference.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ConfigurationOverride Read(JsonReader reader, SarifLog root = null)
        {
            ConfigurationOverride item = (root == null ? new ConfigurationOverride() : new ConfigurationOverride(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ConfigurationOverride item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ConfigurationOverride item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToReportingConfiguration.Write(writer, "configuration", item.Configuration);
                JsonToReportingDescriptorReference.Write(writer, "descriptor", item.Descriptor);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ConfigurationOverride));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ConfigurationOverride)value);
        }
    }
}
