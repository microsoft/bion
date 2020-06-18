using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(TranslationMetadataConverter))]
    public partial class TranslationMetadata
    { }
    
    public class TranslationMetadataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(TranslationMetadata));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadTranslationMetadata();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((TranslationMetadata)value);
        }
    }
    
    internal static class TranslationMetadataJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, TranslationMetadata>> setters = new Dictionary<string, Action<JsonReader, SarifLog, TranslationMetadata>>()
        {
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["fullName"] = (reader, root, me) => me.FullName = reader.ReadString(root),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = reader.ReadMultiformatMessageString(root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = reader.ReadMultiformatMessageString(root),
            ["downloadUri"] = (reader, root, me) => me.DownloadUri = reader.ReadUri(root),
            ["informationUri"] = (reader, root, me) => me.InformationUri = reader.ReadUri(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static TranslationMetadata ReadTranslationMetadata(this JsonReader reader, SarifLog root = null)
        {
            TranslationMetadata item = (root == null ? new TranslationMetadata() : new TranslationMetadata(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, TranslationMetadata item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, TranslationMetadata item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("name", item.Name, default(string));
                writer.Write("fullName", item.FullName, default(string));
                writer.Write("shortDescription", item.ShortDescription);
                writer.Write("fullDescription", item.FullDescription);
                writer.Write("downloadUri", item.DownloadUri, default(Uri));
                writer.Write("informationUri", item.InformationUri, default(Uri));
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
