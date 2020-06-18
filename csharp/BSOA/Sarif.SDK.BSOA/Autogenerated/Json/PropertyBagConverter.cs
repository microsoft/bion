using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(PropertyBagConverter))]
    public partial class PropertyBag
    { }
    
    public class PropertyBagConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(PropertyBag));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadPropertyBag();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((PropertyBag)value);
        }
    }
    
    internal static class PropertyBagJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, PropertyBag>> setters = new Dictionary<string, Action<JsonReader, SarifLog, PropertyBag>>()
        {
            ["tags"] = (reader, root, me) => reader.ReadList(root, me.Tags, JsonReaderExtensions.ReadString)
        };

        public static PropertyBag ReadPropertyBag(this JsonReader reader, SarifLog root = null)
        {
            PropertyBag item = (root == null ? new PropertyBag() : new PropertyBag(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, PropertyBag item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, PropertyBag item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("tags", item.Tags, default(IList<string>));
                writer.WriteEndObject();
            }
        }
    }
}
