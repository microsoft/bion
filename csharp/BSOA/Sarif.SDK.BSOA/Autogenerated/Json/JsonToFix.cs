using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToFix))]
    public partial class Fix
    { }
    
    internal class JsonToFix : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Fix>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Fix>>()
        {
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["artifactChanges"] = (reader, root, me) => JsonToIList<ArtifactChange>.Read(reader, root, me.ArtifactChanges, JsonToArtifactChange.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Fix Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Fix item = (root == null ? new Fix() : new Fix(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Fix item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Fix item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIList<ArtifactChange>.Write(writer, "artifactChanges", item.ArtifactChanges, JsonToArtifactChange.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Fix));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Fix)value);
        }
    }
}
