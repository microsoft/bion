using Newtonsoft.Json;

using RegionDemo.Model_Ext;

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

        private static Dictionary<string, Action<JsonReader, TinyLog, Message>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = (string)reader.Value,
            ["markdown"] = (reader, root, me) => me.Markdown = (string)reader.Value,
        };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ReadJson(reader, null);
        }

        public static Message ReadJson(JsonReader reader, TinyLog root)
        {
            Message item = (root == null ? new Message() : new Message(root));
            Converters.ReadObject(reader, root, item, setters);
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Message item = (Message)value;

            writer.WriteStartObject();

            if (item.Text != default(string))
            {
                writer.WritePropertyName("text");
                writer.WriteValue(item.Text);
            }

            if (item.Markdown != default(string))
            {
                writer.WritePropertyName("markdown");
                writer.WriteValue(item.Markdown);
            }

            writer.WriteEndObject();
        }
    }
}
