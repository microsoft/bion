using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToReplacement))]
    public partial class Replacement
    { }
    
    internal class JsonToReplacement : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Replacement>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Replacement>>()
        {
            ["deletedRegion"] = (reader, root, me) => me.DeletedRegion = JsonToRegion.Read(reader, root),
            ["insertedContent"] = (reader, root, me) => me.InsertedContent = JsonToArtifactContent.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Replacement Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Replacement item = (root == null ? new Replacement() : new Replacement(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Replacement item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Replacement item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToRegion.Write(writer, "deletedRegion", item.DeletedRegion);
                JsonToArtifactContent.Write(writer, "insertedContent", item.InsertedContent);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Replacement));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Replacement)value);
        }
    }
}
