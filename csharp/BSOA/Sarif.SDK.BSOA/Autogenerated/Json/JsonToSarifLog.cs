using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToSarifLog))]
    public partial class SarifLog
    { }

    internal class JsonToSarifLog : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, SarifLog>> setters = new Dictionary<string, Action<JsonReader, SarifLog, SarifLog>>()
        {
            ["$schema"] = (reader, root, me) => me.SchemaUri = JsonToUri.Read(reader, root),
            ["version"] = (reader, root, me) => me.Version = JsonToEnum<SarifVersion>.Read(reader, root),
            ["runs"] = (reader, root, me) => JsonToIList<Run>.Read(reader, root, me.Runs, JsonToRun.Read),
            ["inlineExternalProperties"] = (reader, root, me) => JsonToIList<ExternalProperties>.Read(reader, root, me.InlineExternalProperties, JsonToExternalProperties.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static SarifLog Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            SarifLog item = new SarifLog();

            // SarifLog is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, SarifLog item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, SarifLog item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToUri.Write(writer, "$schema", item.SchemaUri, default);
                JsonToEnum<SarifVersion>.Write(writer, "version", item.Version, default(SarifVersion));
                JsonToIList<Run>.Write(writer, "runs", item.Runs, JsonToRun.Write);
                JsonToIList<ExternalProperties>.Write(writer, "inlineExternalProperties", item.InlineExternalProperties, JsonToExternalProperties.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(SarifLog));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (SarifLog)value);
        }
    }
}
