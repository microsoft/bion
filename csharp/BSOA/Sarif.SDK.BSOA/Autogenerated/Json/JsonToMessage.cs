using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToMessage))]
    public partial class Message
    { }
    
    internal class JsonToMessage : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Message>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = JsonToString.Read(reader, root),
            ["markdown"] = (reader, root, me) => me.Markdown = JsonToString.Read(reader, root),
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["arguments"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Arguments, JsonToString.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Message Read(JsonReader reader, SarifLog root = null)
        {
            Message item = (root == null ? new Message() : new Message(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Message item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Message item)
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
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToIList<String>.Write(writer, "arguments", item.Arguments, JsonToString.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Message));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Message)value);
        }
    }
}
