using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToLocationRelationship))]
    public partial class LocationRelationship
    { }
    
    internal class JsonToLocationRelationship : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, LocationRelationship>> setters = new Dictionary<string, Action<JsonReader, SarifLog, LocationRelationship>>()
        {
            ["target"] = (reader, root, me) => me.Target = JsonToInt.Read(reader, root),
            ["kinds"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Kinds, JsonToString.Read),
            ["description"] = (reader, root, me) => me.Description = JsonToMessage.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static LocationRelationship Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            LocationRelationship item = (root == null ? new LocationRelationship() : new LocationRelationship(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, LocationRelationship item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, LocationRelationship item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "target", item.Target, default);
                JsonToIList<String>.Write(writer, "kinds", item.Kinds, JsonToString.Write);
                JsonToMessage.Write(writer, "description", item.Description);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(LocationRelationship));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (LocationRelationship)value);
        }
    }
}
