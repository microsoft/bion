using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToMultiformatMessageString))]
    public partial class MultiformatMessageString
    { }
    
    internal class JsonToMultiformatMessageString : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, MultiformatMessageString>> setters = new Dictionary<string, Action<JsonReader, SarifLog, MultiformatMessageString>>()
        {
            ["text"] = (reader, root, me) => me.Text = JsonToString.Read(reader, root),
            ["markdown"] = (reader, root, me) => me.Markdown = JsonToString.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static MultiformatMessageString Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            MultiformatMessageString item = (root == null ? new MultiformatMessageString() : new MultiformatMessageString(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, MultiformatMessageString item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, MultiformatMessageString item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "text", item.Text, default);
                JsonToString.Write(writer, "markdown", item.Markdown, default);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(MultiformatMessageString));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (MultiformatMessageString)value);
        }
    }
}
