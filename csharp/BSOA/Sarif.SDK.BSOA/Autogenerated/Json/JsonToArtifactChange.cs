using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToArtifactChange))]
    public partial class ArtifactChange
    { }
    
    internal class JsonToArtifactChange : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ArtifactChange>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ArtifactChange>>()
        {
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = JsonToArtifactLocation.Read(reader, root),
            ["replacements"] = (reader, root, me) => JsonToIList<Replacement>.Read(reader, root, me.Replacements, JsonToReplacement.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ArtifactChange Read(JsonReader reader, SarifLog root = null)
        {
            ArtifactChange item = (root == null ? new ArtifactChange() : new ArtifactChange(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ArtifactChange item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ArtifactChange item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToArtifactLocation.Write(writer, "artifactLocation", item.ArtifactLocation);
                JsonToIList<Replacement>.Write(writer, "replacements", item.Replacements, JsonToReplacement.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactChange));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ArtifactChange)value);
        }
    }
}
