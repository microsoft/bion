using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ExternalPropertyFileReferenceConverter))]
    public partial class ExternalPropertyFileReference
    { }
    
    public class ExternalPropertyFileReferenceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExternalPropertyFileReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadExternalPropertyFileReference();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ExternalPropertyFileReference)value);
        }
    }
    
    internal static class ExternalPropertyFileReferenceJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExternalPropertyFileReference>>()
        {
            ["location"] = (reader, root, me) => me.Location = reader.ReadArtifactLocation(root),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["itemCount"] = (reader, root, me) => me.ItemCount = reader.ReadInt(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ExternalPropertyFileReference ReadExternalPropertyFileReference(this JsonReader reader, SarifLog root = null)
        {
            ExternalPropertyFileReference item = (root == null ? new ExternalPropertyFileReference() : new ExternalPropertyFileReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ExternalPropertyFileReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ExternalPropertyFileReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("location", item.Location);
                writer.Write("guid", item.Guid, default);
                writer.Write("itemCount", item.ItemCount, -1);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
