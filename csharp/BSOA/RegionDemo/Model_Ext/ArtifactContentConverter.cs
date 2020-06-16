using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    internal static class ArtifactContentJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>> setters = new Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>>()
        {
            ["text"] = (reader, root, me) => me.Text = (string)reader.Value,
            ["binary"] = (reader, root, me) => me.Binary = (string)reader.Value,
        };

        public static ArtifactContent ReadArtifactContent(this JsonReader reader, TinyLog root = null)
        {
            ArtifactContent item = (root == null ? new ArtifactContent() : new ArtifactContent(root));
            reader.ReadObject(root, item, setters);
            return item;
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
                writer.Write("text", item.Text, default(string));
                writer.Write("binary", item.Binary, default(string));
                writer.WriteEndObject();
            }
        }
    }

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

    [JsonConverter(typeof(ArtifactContentConverter))]
    public partial class ArtifactContent
    { }
}
