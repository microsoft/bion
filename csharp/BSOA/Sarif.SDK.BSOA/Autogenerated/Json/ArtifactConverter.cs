using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ArtifactConverter))]
    public partial class Artifact
    { }
    
    public class ArtifactConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Artifact));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadArtifact();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Artifact)value);
        }
    }
    
    internal static class ArtifactJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Artifact>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Artifact>>()
        {
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["location"] = (reader, root, me) => me.Location = reader.ReadArtifactLocation(root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = reader.ReadInt(root),
            ["offset"] = (reader, root, me) => me.Offset = reader.ReadInt(root),
            ["length"] = (reader, root, me) => me.Length = reader.ReadInt(root),
            ["roles"] = (reader, root, me) => me.Roles = reader.ReadEnum<ArtifactRoles, SarifLog>(root),
            ["mimeType"] = (reader, root, me) => me.MimeType = reader.ReadString(root),
            ["contents"] = (reader, root, me) => me.Contents = reader.ReadArtifactContent(root),
            ["encoding"] = (reader, root, me) => me.Encoding = reader.ReadString(root),
            ["sourceLanguage"] = (reader, root, me) => me.SourceLanguage = reader.ReadString(root),
            ["hashes"] = (reader, root, me) => reader.ReadDictionary(root, me.Hashes, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["lastModifiedTimeUtc"] = (reader, root, me) => me.LastModifiedTimeUtc = reader.ReadDateTime(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Artifact ReadArtifact(this JsonReader reader, SarifLog root = null)
        {
            Artifact item = (root == null ? new Artifact() : new Artifact(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Artifact item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Artifact item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("description", item.Description);
                writer.Write("location", item.Location);
                writer.Write("parentIndex", item.ParentIndex, -1);
                writer.Write("offset", item.Offset, default(int));
                writer.Write("length", item.Length, -1);
                writer.Write("roles", item.Roles);
                writer.Write("mimeType", item.MimeType, default(string));
                writer.Write("contents", item.Contents);
                writer.Write("encoding", item.Encoding, default(string));
                writer.Write("sourceLanguage", item.SourceLanguage, default(string));
                writer.Write("hashes", item.Hashes, default(IDictionary<string, string>));
                writer.Write("lastModifiedTimeUtc", item.LastModifiedTimeUtc, default(DateTime));
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
