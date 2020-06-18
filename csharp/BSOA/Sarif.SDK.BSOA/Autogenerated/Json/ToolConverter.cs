using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ToolConverter))]
    public partial class Tool
    { }
    
    public class ToolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Tool));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadTool();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Tool)value);
        }
    }
    
    internal static class ToolJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Tool>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Tool>>()
        {
            ["driver"] = (reader, root, me) => me.Driver = reader.ReadToolComponent(root),
            ["extensions"] = (reader, root, me) => reader.ReadList(root, me.Extensions, ToolComponentJsonExtensions.ReadToolComponent),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static Tool ReadTool(this JsonReader reader, SarifLog root = null)
        {
            Tool item = (root == null ? new Tool() : new Tool(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Tool item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Tool item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("driver", item.Driver);
                writer.WriteList("extensions", item.Extensions, ToolComponentJsonExtensions.Write);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
