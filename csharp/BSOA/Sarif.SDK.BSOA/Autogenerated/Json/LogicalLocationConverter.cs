using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(LogicalLocationConverter))]
    public partial class LogicalLocation
    { }
    
    public class LogicalLocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(LogicalLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadLogicalLocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((LogicalLocation)value);
        }
    }
    
    internal static class LogicalLocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, LogicalLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, LogicalLocation>>()
        {
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["fullyQualifiedName"] = (reader, root, me) => me.FullyQualifiedName = reader.ReadString(root),
            ["decoratedName"] = (reader, root, me) => me.DecoratedName = reader.ReadString(root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = reader.ReadInt(root),
            ["kind"] = (reader, root, me) => me.Kind = reader.ReadString(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static LogicalLocation ReadLogicalLocation(this JsonReader reader, SarifLog root = null)
        {
            LogicalLocation item = (root == null ? new LogicalLocation() : new LogicalLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, LogicalLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, LogicalLocation item)
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
                writer.Write("fullyQualifiedName", item.FullyQualifiedName, default);
                writer.Write("decoratedName", item.DecoratedName, default);
                writer.Write("parentIndex", item.ParentIndex, -1);
                writer.Write("kind", item.Kind, default);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
