using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToVersionControlDetails))]
    public partial class VersionControlDetails
    { }
    
    internal class JsonToVersionControlDetails : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, VersionControlDetails>> setters = new Dictionary<string, Action<JsonReader, SarifLog, VersionControlDetails>>()
        {
            ["repositoryUri"] = (reader, root, me) => me.RepositoryUri = JsonToUri.Read(reader, root),
            ["revisionId"] = (reader, root, me) => me.RevisionId = JsonToString.Read(reader, root),
            ["branch"] = (reader, root, me) => me.Branch = JsonToString.Read(reader, root),
            ["revisionTag"] = (reader, root, me) => me.RevisionTag = JsonToString.Read(reader, root),
            ["asOfTimeUtc"] = (reader, root, me) => me.AsOfTimeUtc = JsonToDateTime.Read(reader, root),
            ["mappedTo"] = (reader, root, me) => me.MappedTo = JsonToArtifactLocation.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static VersionControlDetails Read(JsonReader reader, SarifLog root = null)
        {
            VersionControlDetails item = (root == null ? new VersionControlDetails() : new VersionControlDetails(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, VersionControlDetails item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, VersionControlDetails item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToUri.Write(writer, "repositoryUri", item.RepositoryUri, default);
                JsonToString.Write(writer, "revisionId", item.RevisionId, default);
                JsonToString.Write(writer, "branch", item.Branch, default);
                JsonToString.Write(writer, "revisionTag", item.RevisionTag, default);
                JsonToDateTime.Write(writer, "asOfTimeUtc", item.AsOfTimeUtc, default);
                JsonToArtifactLocation.Write(writer, "mappedTo", item.MappedTo);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(VersionControlDetails));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (VersionControlDetails)value);
        }
    }
}
