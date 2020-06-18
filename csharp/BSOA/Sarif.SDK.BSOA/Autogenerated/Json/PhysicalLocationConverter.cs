using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(PhysicalLocationConverter))]
    public partial class PhysicalLocation
    { }
    
    public class PhysicalLocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(PhysicalLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadPhysicalLocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((PhysicalLocation)value);
        }
    }
    
    internal static class PhysicalLocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, PhysicalLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, PhysicalLocation>>()
        {
            ["address"] = (reader, root, me) => me.Address = reader.ReadAddress(root),
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = reader.ReadArtifactLocation(root),
            ["region"] = (reader, root, me) => me.Region = reader.ReadRegion(root),
            ["contextRegion"] = (reader, root, me) => me.ContextRegion = reader.ReadRegion(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static PhysicalLocation ReadPhysicalLocation(this JsonReader reader, SarifLog root = null)
        {
            PhysicalLocation item = (root == null ? new PhysicalLocation() : new PhysicalLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, PhysicalLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, PhysicalLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("address", item.Address);
                writer.Write("artifactLocation", item.ArtifactLocation);
                writer.Write("region", item.Region);
                writer.Write("contextRegion", item.ContextRegion);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
