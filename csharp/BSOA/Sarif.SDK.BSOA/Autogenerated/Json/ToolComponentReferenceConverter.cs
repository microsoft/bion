using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ToolComponentReferenceConverter))]
    public partial class ToolComponentReference
    { }
    
    public class ToolComponentReferenceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ToolComponentReference));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadToolComponentReference();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ToolComponentReference)value);
        }
    }
    
    internal static class ToolComponentReferenceJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ToolComponentReference>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ToolComponentReference>>()
        {
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static ToolComponentReference ReadToolComponentReference(this JsonReader reader, SarifLog root = null)
        {
            ToolComponentReference item = (root == null ? new ToolComponentReference() : new ToolComponentReference(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ToolComponentReference item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ToolComponentReference item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("name", item.Name, default);
                writer.Write("index", item.Index, -1);
                writer.Write("guid", item.Guid, default);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
