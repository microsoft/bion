using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(RectangleConverter))]
    public partial class Rectangle
    { }
    
    public class RectangleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Rectangle));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadRectangle();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Rectangle)value);
        }
    }
    
    internal static class RectangleJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Rectangle>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Rectangle>>()
        {
            ["top"] = (reader, root, me) => me.Top = reader.ReadDouble(root),
            ["left"] = (reader, root, me) => me.Left = reader.ReadDouble(root),
            ["bottom"] = (reader, root, me) => me.Bottom = reader.ReadDouble(root),
            ["right"] = (reader, root, me) => me.Right = reader.ReadDouble(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Rectangle ReadRectangle(this JsonReader reader, SarifLog root = null)
        {
            Rectangle item = (root == null ? new Rectangle() : new Rectangle(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Rectangle item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Rectangle item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("top", item.Top, default(double));
                writer.Write("left", item.Left, default(double));
                writer.Write("bottom", item.Bottom, default(double));
                writer.Write("right", item.Right, default(double));
                writer.Write("message", item.Message);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
