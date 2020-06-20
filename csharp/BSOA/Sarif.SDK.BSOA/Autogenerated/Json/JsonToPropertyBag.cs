using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToPropertyBag))]
    public partial class PropertyBag
    { }
    
    internal class JsonToPropertyBag : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, PropertyBag>> setters = new Dictionary<string, Action<JsonReader, SarifLog, PropertyBag>>()
        {
            ["tags"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Tags, JsonToString.Read)
        };

        public static PropertyBag Read(JsonReader reader, SarifLog root = null)
        {
            PropertyBag item = (root == null ? new PropertyBag() : new PropertyBag(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, PropertyBag item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, PropertyBag item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<String>.Write(writer, "tags", item.Tags, JsonToString.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(PropertyBag));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (PropertyBag)value);
        }
    }
}
