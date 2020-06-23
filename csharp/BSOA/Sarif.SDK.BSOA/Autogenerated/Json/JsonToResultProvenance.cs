using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToResultProvenance))]
    public partial class ResultProvenance
    { }
    
    internal class JsonToResultProvenance : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ResultProvenance>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ResultProvenance>>()
        {
            ["firstDetectionTimeUtc"] = (reader, root, me) => me.FirstDetectionTimeUtc = JsonToDateTime.Read(reader, root),
            ["lastDetectionTimeUtc"] = (reader, root, me) => me.LastDetectionTimeUtc = JsonToDateTime.Read(reader, root),
            ["firstDetectionRunGuid"] = (reader, root, me) => me.FirstDetectionRunGuid = JsonToString.Read(reader, root),
            ["lastDetectionRunGuid"] = (reader, root, me) => me.LastDetectionRunGuid = JsonToString.Read(reader, root),
            ["invocationIndex"] = (reader, root, me) => me.InvocationIndex = JsonToInt.Read(reader, root),
            ["conversionSources"] = (reader, root, me) => JsonToIList<PhysicalLocation>.Read(reader, root, me.ConversionSources, JsonToPhysicalLocation.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ResultProvenance Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ResultProvenance item = (root == null ? new ResultProvenance() : new ResultProvenance(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ResultProvenance item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ResultProvenance item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToDateTime.Write(writer, "firstDetectionTimeUtc", item.FirstDetectionTimeUtc, default);
                JsonToDateTime.Write(writer, "lastDetectionTimeUtc", item.LastDetectionTimeUtc, default);
                JsonToString.Write(writer, "firstDetectionRunGuid", item.FirstDetectionRunGuid, default);
                JsonToString.Write(writer, "lastDetectionRunGuid", item.LastDetectionRunGuid, default);
                JsonToInt.Write(writer, "invocationIndex", item.InvocationIndex, -1);
                JsonToIList<PhysicalLocation>.Write(writer, "conversionSources", item.ConversionSources, JsonToPhysicalLocation.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ResultProvenance));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ResultProvenance)value);
        }
    }
}
