using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToArtifactContent))]
    public partial class ArtifactContent
    { }
    
    internal class JsonToArtifactContent : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactContent>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactContent>>()
        {
            ["text"] = (reader, root, me) => me.Text = JsonToString.Read(reader, root),
            ["binary"] = (reader, root, me) => me.Binary = JsonToString.Read(reader, root),
            ["rendered"] = (reader, root, me) => me.Rendered = JsonToMultiformatMessageString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ArtifactContent Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ArtifactContent item = (root == null ? new ArtifactContent() : new ArtifactContent(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ArtifactContent item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ArtifactContent item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "text", item.Text, default);
                JsonToString.Write(writer, "binary", item.Binary, default);
                JsonToMultiformatMessageString.Write(writer, "rendered", item.Rendered);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactContent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ArtifactContent)value);
        }
    }
}
