using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToTranslationMetadata))]
    public partial class TranslationMetadata
    { }
    
    internal class JsonToTranslationMetadata : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, TranslationMetadata>> setters = new Dictionary<string, Action<JsonReader, SarifLog, TranslationMetadata>>()
        {
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["fullName"] = (reader, root, me) => me.FullName = JsonToString.Read(reader, root),
            ["shortDescription"] = (reader, root, me) => me.ShortDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["fullDescription"] = (reader, root, me) => me.FullDescription = JsonToMultiformatMessageString.Read(reader, root),
            ["downloadUri"] = (reader, root, me) => me.DownloadUri = JsonToUri.Read(reader, root),
            ["informationUri"] = (reader, root, me) => me.InformationUri = JsonToUri.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static TranslationMetadata Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            TranslationMetadata item = (root == null ? new TranslationMetadata() : new TranslationMetadata(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, TranslationMetadata item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, TranslationMetadata item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToString.Write(writer, "fullName", item.FullName, default);
                JsonToMultiformatMessageString.Write(writer, "shortDescription", item.ShortDescription);
                JsonToMultiformatMessageString.Write(writer, "fullDescription", item.FullDescription);
                JsonToUri.Write(writer, "downloadUri", item.DownloadUri, default);
                JsonToUri.Write(writer, "informationUri", item.InformationUri, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(TranslationMetadata));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (TranslationMetadata)value);
        }
    }
}
