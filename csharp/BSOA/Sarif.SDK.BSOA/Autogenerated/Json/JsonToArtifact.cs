using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToArtifact))]
    public partial class Artifact
    { }
    
    internal class JsonToArtifact : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Artifact>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Artifact>>()
        {
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["location"] = (reader, root, me) => me.Location = JsonToArtifactLocation.Read(reader, root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = JsonToInt.Read(reader, root),
            ["offset"] = (reader, root, me) => me.Offset = JsonToInt.Read(reader, root),
            ["length"] = (reader, root, me) => me.Length = JsonToInt.Read(reader, root),
            ["roles"] = (reader, root, me) => me.Roles = JsonToEnum<ArtifactRoles>.Read(reader, root),
            ["mimeType"] = (reader, root, me) => me.MimeType = JsonToString.Read(reader, root),
            ["contents"] = (reader, root, me) => me.Contents = JsonToArtifactContent.Read(reader, root),
            ["encoding"] = (reader, root, me) => me.Encoding = JsonToString.Read(reader, root),
            ["sourceLanguage"] = (reader, root, me) => me.SourceLanguage = JsonToString.Read(reader, root),
            ["hashes"] = (reader, root, me) => me.Hashes = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["lastModifiedTimeUtc"] = (reader, root, me) => me.LastModifiedTimeUtc = JsonToDateTime.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Artifact Read(JsonReader reader, SarifLog root = null)
        {
            Artifact item = (root == null ? new Artifact() : new Artifact(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Artifact item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Artifact item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToArtifactLocation.Write(writer, "location", item.Location);
                JsonToInt.Write(writer, "parentIndex", item.ParentIndex, -1);
                JsonToInt.Write(writer, "offset", item.Offset, default);
                JsonToInt.Write(writer, "length", item.Length, -1);
                JsonToEnum<ArtifactRoles>.Write(writer, "roles", item.Roles, default(ArtifactRoles));
                JsonToString.Write(writer, "mimeType", item.MimeType, default);
                JsonToArtifactContent.Write(writer, "contents", item.Contents);
                JsonToString.Write(writer, "encoding", item.Encoding, default);
                JsonToString.Write(writer, "sourceLanguage", item.SourceLanguage, default);
                JsonToIDictionary<String, String>.Write(writer, "hashes", item.Hashes, JsonToString.Write);
                JsonToDateTime.Write(writer, "lastModifiedTimeUtc", item.LastModifiedTimeUtc, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Artifact));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Artifact)value);
        }
    }
}
