using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToTool))]
    public partial class Tool
    { }
    
    internal class JsonToTool : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Tool>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Tool>>()
        {
            ["driver"] = (reader, root, me) => me.Driver = JsonToToolComponent.Read(reader, root),
            ["extensions"] = (reader, root, me) => JsonToIList<ToolComponent>.Read(reader, root, me.Extensions, JsonToToolComponent.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Tool Read(JsonReader reader, SarifLog root = null)
        {
            Tool item = (root == null ? new Tool() : new Tool(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Tool item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Tool item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToToolComponent.Write(writer, "driver", item.Driver);
                JsonToIList<ToolComponent>.Write(writer, "extensions", item.Extensions, JsonToToolComponent.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Tool));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Tool)value);
        }
    }
}
