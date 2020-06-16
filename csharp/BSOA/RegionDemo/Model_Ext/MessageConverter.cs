﻿using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    internal static class MessageJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, TinyLog, Message>> setters = new Dictionary<string, Action<JsonReader, TinyLog, Message>>()
        {
            ["text"] = (reader, root, me) => me.Text = reader.ReadString(root),
            ["markdown"] = (reader, root, me) => me.Markdown = reader.ReadString(root),
        };

        public static Message ReadMessage(this JsonReader reader, TinyLog root = null)
        {
            Message item = (root == null ? new Message() : new Message(root));
            reader.ReadObject(root, item, setters);
            return item;
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
                writer.Write("text", item.Text, default(string));
                writer.Write("markdown", item.Markdown, default(string));
                writer.WriteEndObject();
            }
        }
    }

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

    [JsonConverter(typeof(MessageConverter))]
    public partial class Message
    { }
}
