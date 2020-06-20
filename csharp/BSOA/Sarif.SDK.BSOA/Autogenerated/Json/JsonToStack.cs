using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToStack))]
    public partial class Stack
    { }
    
    internal class JsonToStack : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Stack>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Stack>>()
        {
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["frames"] = (reader, root, me) => JsonToIList<StackFrame>.Read(reader, root, me.Frames, JsonToStackFrame.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Stack Read(JsonReader reader, SarifLog root = null)
        {
            Stack item = (root == null ? new Stack() : new Stack(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Stack item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Stack item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIList<StackFrame>.Write(writer, "frames", item.Frames, JsonToStackFrame.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Stack));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Stack)value);
        }
    }
}
