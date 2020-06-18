using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(AttachmentConverter))]
    public partial class Attachment
    { }
    
    public class AttachmentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Attachment));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadAttachment();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Attachment)value);
        }
    }
    
    internal static class AttachmentJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Attachment>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Attachment>>()
        {
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = reader.ReadArtifactLocation(root),
            ["regions"] = (reader, root, me) => reader.ReadList(root, me.Regions, RegionJsonExtensions.ReadRegion),
            ["rectangles"] = (reader, root, me) => reader.ReadList(root, me.Rectangles, RectangleJsonExtensions.ReadRectangle),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Attachment ReadAttachment(this JsonReader reader, SarifLog root = null)
        {
            Attachment item = (root == null ? new Attachment() : new Attachment(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Attachment item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Attachment item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("description", item.Description);
                writer.Write("artifactLocation", item.ArtifactLocation);
                writer.WriteList("regions", item.Regions, RegionJsonExtensions.Write);
                writer.WriteList("rectangles", item.Rectangles, RectangleJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
