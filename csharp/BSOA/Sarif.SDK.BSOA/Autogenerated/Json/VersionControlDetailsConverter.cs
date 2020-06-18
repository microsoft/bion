using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(VersionControlDetailsConverter))]
    public partial class VersionControlDetails
    { }
    
    public class VersionControlDetailsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(VersionControlDetails));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadVersionControlDetails();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((VersionControlDetails)value);
        }
    }
    
    internal static class VersionControlDetailsJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, VersionControlDetails>> setters = new Dictionary<string, Action<JsonReader, SarifLog, VersionControlDetails>>()
        {
            ["repositoryUri"] = (reader, root, me) => me.RepositoryUri = reader.ReadUri(root),
            ["revisionId"] = (reader, root, me) => me.RevisionId = reader.ReadString(root),
            ["branch"] = (reader, root, me) => me.Branch = reader.ReadString(root),
            ["revisionTag"] = (reader, root, me) => me.RevisionTag = reader.ReadString(root),
            ["asOfTimeUtc"] = (reader, root, me) => me.AsOfTimeUtc = reader.ReadDateTime(root),
            ["mappedTo"] = (reader, root, me) => me.MappedTo = reader.ReadArtifactLocation(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static VersionControlDetails ReadVersionControlDetails(this JsonReader reader, SarifLog root = null)
        {
            VersionControlDetails item = (root == null ? new VersionControlDetails() : new VersionControlDetails(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, VersionControlDetails item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, VersionControlDetails item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("repositoryUri", item.RepositoryUri, default);
                writer.Write("revisionId", item.RevisionId, default);
                writer.Write("branch", item.Branch, default);
                writer.Write("revisionTag", item.RevisionTag, default);
                writer.Write("asOfTimeUtc", item.AsOfTimeUtc, default);
                writer.Write("mappedTo", item.MappedTo);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
