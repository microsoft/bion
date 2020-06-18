using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ResultProvenanceConverter))]
    public partial class ResultProvenance
    { }
    
    public class ResultProvenanceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ResultProvenance));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadResultProvenance();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ResultProvenance)value);
        }
    }
    
    internal static class ResultProvenanceJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ResultProvenance>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ResultProvenance>>()
        {
            ["firstDetectionTimeUtc"] = (reader, root, me) => me.FirstDetectionTimeUtc = reader.ReadDateTime(root),
            ["lastDetectionTimeUtc"] = (reader, root, me) => me.LastDetectionTimeUtc = reader.ReadDateTime(root),
            ["firstDetectionRunGuid"] = (reader, root, me) => me.FirstDetectionRunGuid = reader.ReadString(root),
            ["lastDetectionRunGuid"] = (reader, root, me) => me.LastDetectionRunGuid = reader.ReadString(root),
            ["invocationIndex"] = (reader, root, me) => me.InvocationIndex = reader.ReadInt(root),
            ["conversionSources"] = (reader, root, me) => reader.ReadList(root, me.ConversionSources, PhysicalLocationJsonExtensions.ReadPhysicalLocation),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ResultProvenance ReadResultProvenance(this JsonReader reader, SarifLog root = null)
        {
            ResultProvenance item = (root == null ? new ResultProvenance() : new ResultProvenance(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ResultProvenance item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ResultProvenance item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("firstDetectionTimeUtc", item.FirstDetectionTimeUtc, default);
                writer.Write("lastDetectionTimeUtc", item.LastDetectionTimeUtc, default);
                writer.Write("firstDetectionRunGuid", item.FirstDetectionRunGuid, default);
                writer.Write("lastDetectionRunGuid", item.LastDetectionRunGuid, default);
                writer.Write("invocationIndex", item.InvocationIndex, -1);
                writer.WriteList("conversionSources", item.ConversionSources, PhysicalLocationJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
