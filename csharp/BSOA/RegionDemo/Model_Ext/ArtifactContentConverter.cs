using Newtonsoft.Json;

using RegionDemo.Model_Ext;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
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

        private static Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>> setters = new Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>>()
        {
            ["text"] = (reader, root, me) => me.Text = (string)reader.Value,
            ["binary"] = (reader, root, me) => me.Binary = (string)reader.Value,
        };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, null);
        }

        public static ArtifactContent ReadJson(JsonReader reader, TinyLog root)
        {
            ArtifactContent item = (root == null ? new ArtifactContent() : new ArtifactContent(root));
            Converters.ReadObject(reader, root, item, setters);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ArtifactContent item = (ArtifactContent)value;

            writer.WriteStartObject();

            if (item.Text != default(string))
            {
                writer.WritePropertyName("text");
                writer.WriteValue(item.Text);
            }

            if (item.Binary != default(string))
            {
                writer.WritePropertyName("binary");
                writer.WriteValue(item.Binary);
            }

            writer.WriteEndObject();
        }
    }
}
