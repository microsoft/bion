using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(JsonToArtifactContent))]
    public partial class ArtifactContent
    { }
    
    internal class JsonToArtifactContent : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>> setters = new Dictionary<string, Action<JsonReader, TinyLog, ArtifactContent>>()
        {
            ["text"] = (reader, root, me) => me.Text = JsonToString.Read(reader, root),
            ["binary"] = (reader, root, me) => me.Binary = JsonToString.Read(reader, root)
        };

        public static ArtifactContent Read(JsonReader reader, TinyLog root = null)
        {
            ArtifactContent item = (root == null ? new ArtifactContent() : new ArtifactContent(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ArtifactContent item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ArtifactContent item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "text", item.Text, null);
                JsonToString.Write(writer, "binary", item.Binary, null);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ArtifactContent));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ArtifactContent)value);
        }
    }
}
