using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
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
        private static Dictionary<string, Action<JsonReader, SarifLog, Message>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = reader.ReadString(root),
            ["markdown"] = (reader, root, me) => me.Markdown = reader.ReadString(root),
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["arguments"] = (reader, root, me) => reader.ReadList(root, me.Arguments, JsonReaderExtensions.ReadString),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static Message ReadMessage(this JsonReader reader, SarifLog root = null)
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
                writer.Write("text", item.Text, default);
                writer.Write("markdown", item.Markdown, default);
                writer.Write("id", item.Id, default);
                writer.Write("arguments", item.Arguments, default);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
