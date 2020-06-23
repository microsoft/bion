using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToAttachment))]
    public partial class Attachment
    { }
    
    internal class JsonToAttachment : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Attachment>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Attachment>>()
        {
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = JsonToArtifactLocation.Read(reader, root),
            ["regions"] = (reader, root, me) => JsonToIList<Region>.Read(reader, root, me.Regions, JsonToRegion.Read),
            ["rectangles"] = (reader, root, me) => JsonToIList<Rectangle>.Read(reader, root, me.Rectangles, JsonToRectangle.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Attachment Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Attachment item = (root == null ? new Attachment() : new Attachment(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Attachment item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Attachment item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToArtifactLocation.Write(writer, "artifactLocation", item.ArtifactLocation);
                JsonToIList<Region>.Write(writer, "regions", item.Regions, JsonToRegion.Write);
                JsonToIList<Rectangle>.Write(writer, "rectangles", item.Rectangles, JsonToRectangle.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Attachment));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Attachment)value);
        }
    }
}
