using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(SarifLogConverter))]
    public partial class SarifLog
    { }

    public class SarifLogConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(SarifLog));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadSarifLog();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((SarifLog)value);
        }
    }

    internal static class SarifLogJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, SarifLog>> setters = new Dictionary<string, Action<JsonReader, SarifLog, SarifLog>>()
        {
            ["$schema"] = (reader, root, me) => me.SchemaUri = reader.ReadUri(root),
            ["version"] = (reader, root, me) => me.Version = reader.ReadEnum<SarifVersion, SarifLog>(root),
            ["runs"] = (reader, root, me) => reader.ReadList(root, me.Runs, RunJsonExtensions.ReadRun),
            ["inlineExternalProperties"] = (reader, root, me) => reader.ReadList(root, me.InlineExternalProperties, ExternalPropertiesJsonExtensions.ReadExternalProperties),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static SarifLog ReadSarifLog(this JsonReader reader, SarifLog root = null)
        {
            SarifLog item = new SarifLog();
            root = item;

            reader.ReadObject(root, item, setters);
            
            item.DB.Trim();

            return item;
        }

        public static void Write(this JsonWriter writer, SarifLog item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("$schema", item.SchemaUri, default);
                writer.WriteEnum("version", item.Version, default(SarifVersion));
                writer.WriteList("runs", item.Runs, RunJsonExtensions.Write);
                writer.WriteList("inlineExternalProperties", item.InlineExternalProperties, ExternalPropertiesJsonExtensions.Write);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
