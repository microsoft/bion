using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(JsonToMessage))]
    public partial class Message
    { }
    
    internal class JsonToMessage : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Message>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = JsonToString.Read(reader, root),
            ["markdown"] = (reader, root, me) => me.Markdown = JsonToString.Read(reader, root),
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root)
        };

        public static Message Read(JsonReader reader, TinyLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
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
                JsonToString.Write(writer, "text", item.Text, null);
                JsonToString.Write(writer, "markdown", item.Markdown, null);
                JsonToString.Write(writer, "id", item.Id, null);
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
