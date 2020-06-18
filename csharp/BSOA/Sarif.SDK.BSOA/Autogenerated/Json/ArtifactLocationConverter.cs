using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ArtifactLocationConverter))]
    public partial class ArtifactLocation
    { }
    
    public class ArtifactLocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadArtifactLocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ArtifactLocation)value);
        }
    }
    
    internal static class ArtifactLocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactLocation>>()
        {
            ["uri"] = (reader, root, me) => me.Uri = reader.ReadUri(root),
            ["uriBaseId"] = (reader, root, me) => me.UriBaseId = reader.ReadString(root),
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ArtifactLocation ReadArtifactLocation(this JsonReader reader, SarifLog root = null)
        {
            ArtifactLocation item = (root == null ? new ArtifactLocation() : new ArtifactLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ArtifactLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ArtifactLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("uri", item.Uri, default);
                writer.Write("uriBaseId", item.UriBaseId, default);
                writer.Write("index", item.Index, -1);
                writer.Write("description", item.Description);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
