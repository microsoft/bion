using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToRectangle))]
    public partial class Rectangle
    { }
    
    internal class JsonToRectangle : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Rectangle>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Rectangle>>()
        {
            ["top"] = (reader, root, me) => me.Top = JsonToDouble.Read(reader, root),
            ["left"] = (reader, root, me) => me.Left = JsonToDouble.Read(reader, root),
            ["bottom"] = (reader, root, me) => me.Bottom = JsonToDouble.Read(reader, root),
            ["right"] = (reader, root, me) => me.Right = JsonToDouble.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Rectangle Read(JsonReader reader, SarifLog root = null)
        {
            Rectangle item = (root == null ? new Rectangle() : new Rectangle(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Rectangle item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Rectangle item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToDouble.Write(writer, "top", item.Top, default);
                JsonToDouble.Write(writer, "left", item.Left, default);
                JsonToDouble.Write(writer, "bottom", item.Bottom, default);
                JsonToDouble.Write(writer, "right", item.Right, default);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Rectangle));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Rectangle)value);
        }
    }
}
