using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToSpecialLocations))]
    public partial class SpecialLocations
    { }
    
    internal class JsonToSpecialLocations : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, SpecialLocations>> setters = new Dictionary<string, Action<JsonReader, SarifLog, SpecialLocations>>()
        {
            ["displayBase"] = (reader, root, me) => me.DisplayBase = JsonToArtifactLocation.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static SpecialLocations Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            SpecialLocations item = (root == null ? new SpecialLocations() : new SpecialLocations(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, SpecialLocations item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, SpecialLocations item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToArtifactLocation.Write(writer, "displayBase", item.DisplayBase);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(SpecialLocations));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (SpecialLocations)value);
        }
    }
}
