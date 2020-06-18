using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(FixConverter))]
    public partial class Fix
    { }
    
    public class FixConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Fix));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadFix();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Fix)value);
        }
    }
    
    internal static class FixJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Fix>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Fix>>()
        {
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["artifactChanges"] = (reader, root, me) => reader.ReadList(root, me.ArtifactChanges, ArtifactChangeJsonExtensions.ReadArtifactChange),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Fix ReadFix(this JsonReader reader, SarifLog root = null)
        {
            Fix item = (root == null ? new Fix() : new Fix(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Fix item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Fix item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("description", item.Description);
                writer.WriteList("artifactChanges", item.ArtifactChanges, ArtifactChangeJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
