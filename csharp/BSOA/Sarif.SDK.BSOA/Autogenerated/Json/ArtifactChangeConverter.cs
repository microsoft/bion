using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ArtifactChangeConverter))]
    public partial class ArtifactChange
    { }
    
    public class ArtifactChangeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactChange));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadArtifactChange();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ArtifactChange)value);
        }
    }
    
    internal static class ArtifactChangeJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactChange>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactChange>>()
        {
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = reader.ReadArtifactLocation(root),
            ["replacements"] = (reader, root, me) => reader.ReadList(root, me.Replacements, ReplacementJsonExtensions.ReadReplacement),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ArtifactChange ReadArtifactChange(this JsonReader reader, SarifLog root = null)
        {
            ArtifactChange item = (root == null ? new ArtifactChange() : new ArtifactChange(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ArtifactChange item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ArtifactChange item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("artifactLocation", item.ArtifactLocation);
                writer.WriteList("replacements", item.Replacements, ReplacementJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
