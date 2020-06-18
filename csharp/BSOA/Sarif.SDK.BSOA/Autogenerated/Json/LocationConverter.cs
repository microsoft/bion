using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(LocationConverter))]
    public partial class Location
    { }
    
    public class LocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Location));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadLocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Location)value);
        }
    }
    
    internal static class LocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Location>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Location>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadInt(root),
            ["physicalLocation"] = (reader, root, me) => me.PhysicalLocation = reader.ReadPhysicalLocation(root),
            ["logicalLocations"] = (reader, root, me) => reader.ReadList(root, me.LogicalLocations, LogicalLocationJsonExtensions.ReadLogicalLocation),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["annotations"] = (reader, root, me) => reader.ReadList(root, me.Annotations, RegionJsonExtensions.ReadRegion),
            ["relationships"] = (reader, root, me) => reader.ReadList(root, me.Relationships, LocationRelationshipJsonExtensions.ReadLocationRelationship),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Location ReadLocation(this JsonReader reader, SarifLog root = null)
        {
            Location item = (root == null ? new Location() : new Location(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Location item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Location item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("id", item.Id, -1);
                writer.Write("physicalLocation", item.PhysicalLocation);
                writer.WriteList("logicalLocations", item.LogicalLocations, LogicalLocationJsonExtensions.Write);
                writer.Write("message", item.Message);
                writer.WriteList("annotations", item.Annotations, RegionJsonExtensions.Write);
                writer.WriteList("relationships", item.Relationships, LocationRelationshipJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
