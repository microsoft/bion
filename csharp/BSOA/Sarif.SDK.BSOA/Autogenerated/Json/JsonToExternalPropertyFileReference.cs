using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToExternalPropertyFileReference))]
    public partial class ExternalPropertyFileReference
    { }
    
    internal class JsonToExternalPropertyFileReference : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReference>>()
        {
            ["location"] = (reader, root, me) => me.Location = JsonToArtifactLocation.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["itemCount"] = (reader, root, me) => me.ItemCount = JsonToInt.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ExternalPropertyFileReference Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ExternalPropertyFileReference item = (root == null ? new ExternalPropertyFileReference() : new ExternalPropertyFileReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ExternalPropertyFileReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ExternalPropertyFileReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToArtifactLocation.Write(writer, "location", item.Location);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToInt.Write(writer, "itemCount", item.ItemCount, -1);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalPropertyFileReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ExternalPropertyFileReference)value);
        }
    }
}
