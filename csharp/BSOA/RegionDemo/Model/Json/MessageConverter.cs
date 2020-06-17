using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    [JsonConverter(typeof(MessageConverter))]
    public partial class Message
    { }
    
    public class MessageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Message));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadMessage();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Message)value);
        }
    }
    
    internal static class MessageJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Message>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = reader.ReadString(root),
            ["markdown"] = (reader, root, me) => me.Markdown = reader.ReadString(root),
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root)
        };

        public static Message ReadMessage(this JsonReader reader, TinyLog root = null)
        {
            Message item = (root == null ? new Message() : new Message(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Message item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Message item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("text", item.Text, null);
                writer.Write("markdown", item.Markdown, null);
                writer.Write("id", item.Id, null);
                writer.WriteEndObject();
            }
        }
    }
}
