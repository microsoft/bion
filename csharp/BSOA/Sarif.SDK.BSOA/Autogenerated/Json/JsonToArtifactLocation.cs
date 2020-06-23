using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToArtifactLocation))]
    public partial class ArtifactLocation
    { }
    
    internal class JsonToArtifactLocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactLocation>>()
        {
            ["uri"] = (reader, root, me) => me.Uri = JsonToUri.Read(reader, root),
            ["uriBaseId"] = (reader, root, me) => me.UriBaseId = JsonToString.Read(reader, root),
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ArtifactLocation Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ArtifactLocation item = (root == null ? new ArtifactLocation() : new ArtifactLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ArtifactLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ArtifactLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToUri.Write(writer, "uri", item.Uri, default);
                JsonToString.Write(writer, "uriBaseId", item.UriBaseId, default);
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ArtifactLocation)value);
        }
    }
}
