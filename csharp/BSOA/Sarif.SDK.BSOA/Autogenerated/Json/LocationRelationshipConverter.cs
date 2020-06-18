using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(LocationRelationshipConverter))]
    public partial class LocationRelationship
    { }
    
    public class LocationRelationshipConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(LocationRelationship));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadLocationRelationship();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((LocationRelationship)value);
        }
    }
    
    internal static class LocationRelationshipJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, LocationRelationship>> setters = new Dictionary<string, Action<JsonReader, SarifLog, LocationRelationship>>()
        {
            ["target"] = (reader, root, me) => me.Target = reader.ReadInt(root),
            ["kinds"] = (reader, root, me) => reader.ReadList(root, me.Kinds, JsonReaderExtensions.ReadString),
            ["description"] = (reader, root, me) => me.Description = reader.ReadMessage(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static LocationRelationship ReadLocationRelationship(this JsonReader reader, SarifLog root = null)
        {
            LocationRelationship item = (root == null ? new LocationRelationship() : new LocationRelationship(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, LocationRelationship item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, LocationRelationship item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("target", item.Target, default);
                writer.Write("kinds", item.Kinds, default);
                writer.Write("description", item.Description);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
