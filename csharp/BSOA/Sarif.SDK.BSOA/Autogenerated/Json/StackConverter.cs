using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(StackConverter))]
    public partial class Stack
    { }
    
    public class StackConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Stack));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadStack();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Stack)value);
        }
    }
    
    internal static class StackJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Stack>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Stack>>()
        {
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["frames"] = (reader, root, me) => reader.ReadList(root, me.Frames, StackFrameJsonExtensions.ReadStackFrame),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static Stack ReadStack(this JsonReader reader, SarifLog root = null)
        {
            Stack item = (root == null ? new Stack() : new Stack(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Stack item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Stack item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("message", item.Message);
                writer.WriteList("frames", item.Frames, StackFrameJsonExtensions.Write);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
