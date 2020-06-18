using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ArtifactContentConverter))]
    public partial class ArtifactContent
    { }
    
    public class ArtifactContentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactContent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadArtifactContent();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ArtifactContent)value);
        }
    }
    
    internal static class ArtifactContentJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactContent>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactContent>>()
        {
            ["text"] = (reader, root, me) => me.Text = reader.ReadString(root),
            ["binary"] = (reader, root, me) => me.Binary = reader.ReadString(root),
            ["rendered"] = (reader, root, me) => me.Rendered = reader.ReadMultiformatMessageString(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ArtifactContent ReadArtifactContent(this JsonReader reader, SarifLog root = null)
        {
            ArtifactContent item = (root == null ? new ArtifactContent() : new ArtifactContent(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ArtifactContent item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ArtifactContent item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("text", item.Text, default);
                writer.Write("binary", item.Binary, default);
                writer.Write("rendered", item.Rendered);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
