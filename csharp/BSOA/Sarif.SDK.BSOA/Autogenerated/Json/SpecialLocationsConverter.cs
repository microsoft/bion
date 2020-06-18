using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(SpecialLocationsConverter))]
    public partial class SpecialLocations
    { }
    
    public class SpecialLocationsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(SpecialLocations));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadSpecialLocations();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((SpecialLocations)value);
        }
    }
    
    internal static class SpecialLocationsJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, SpecialLocations>> setters = new Dictionary<string, Action<JsonReader, SarifLog, SpecialLocations>>()
        {
            ["displayBase"] = (reader, root, me) => me.DisplayBase = reader.ReadArtifactLocation(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static SpecialLocations ReadSpecialLocations(this JsonReader reader, SarifLog root = null)
        {
            SpecialLocations item = (root == null ? new SpecialLocations() : new SpecialLocations(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, SpecialLocations item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, SpecialLocations item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("displayBase", item.DisplayBase);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
