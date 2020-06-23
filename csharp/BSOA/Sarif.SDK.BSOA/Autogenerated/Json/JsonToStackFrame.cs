using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToStackFrame))]
    public partial class StackFrame
    { }
    
    internal class JsonToStackFrame : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, StackFrame>> setters = new Dictionary<string, Action<JsonReader, SarifLog, StackFrame>>()
        {
            ["location"] = (reader, root, me) => me.Location = JsonToLocation.Read(reader, root),
            ["module"] = (reader, root, me) => me.Module = JsonToString.Read(reader, root),
            ["threadId"] = (reader, root, me) => me.ThreadId = JsonToInt.Read(reader, root),
            ["parameters"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Parameters, JsonToString.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static StackFrame Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            StackFrame item = (root == null ? new StackFrame() : new StackFrame(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, StackFrame item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, StackFrame item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToLocation.Write(writer, "location", item.Location);
                JsonToString.Write(writer, "module", item.Module, default);
                JsonToInt.Write(writer, "threadId", item.ThreadId, default);
                JsonToIList<String>.Write(writer, "parameters", item.Parameters, JsonToString.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(StackFrame));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (StackFrame)value);
        }
    }
}
