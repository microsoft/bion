using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(StackFrameConverter))]
    public partial class StackFrame
    { }
    
    public class StackFrameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(StackFrame));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadStackFrame();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((StackFrame)value);
        }
    }
    
    internal static class StackFrameJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, StackFrame>> setters = new Dictionary<string, Action<JsonReader, SarifLog, StackFrame>>()
        {
            ["location"] = (reader, root, me) => me.Location = reader.ReadLocation(root),
            ["module"] = (reader, root, me) => me.Module = reader.ReadString(root),
            ["threadId"] = (reader, root, me) => me.ThreadId = reader.ReadInt(root),
            ["parameters"] = (reader, root, me) => reader.ReadList(root, me.Parameters, JsonReaderExtensions.ReadString),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static StackFrame ReadStackFrame(this JsonReader reader, SarifLog root = null)
        {
            StackFrame item = (root == null ? new StackFrame() : new StackFrame(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, StackFrame item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, StackFrame item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("location", item.Location);
                writer.Write("module", item.Module, default);
                writer.Write("threadId", item.ThreadId, default);
                writer.Write("parameters", item.Parameters, default);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
