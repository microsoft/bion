using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToLocation))]
    public partial class Location
    { }
    
    internal class JsonToLocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Location>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Location>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToInt.Read(reader, root),
            ["physicalLocation"] = (reader, root, me) => me.PhysicalLocation = JsonToPhysicalLocation.Read(reader, root),
            ["logicalLocations"] = (reader, root, me) => JsonToIList<LogicalLocation>.Read(reader, root, me.LogicalLocations, JsonToLogicalLocation.Read),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["annotations"] = (reader, root, me) => JsonToIList<Region>.Read(reader, root, me.Annotations, JsonToRegion.Read),
            ["relationships"] = (reader, root, me) => JsonToIList<LocationRelationship>.Read(reader, root, me.Relationships, JsonToLocationRelationship.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Location Read(JsonReader reader, SarifLog root = null)
        {
            Location item = (root == null ? new Location() : new Location(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Location item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Location item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "id", item.Id, -1);
                JsonToPhysicalLocation.Write(writer, "physicalLocation", item.PhysicalLocation);
                JsonToIList<LogicalLocation>.Write(writer, "logicalLocations", item.LogicalLocations, JsonToLogicalLocation.Write);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIList<Region>.Write(writer, "annotations", item.Annotations, JsonToRegion.Write);
                JsonToIList<LocationRelationship>.Write(writer, "relationships", item.Relationships, JsonToLocationRelationship.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Location));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Location)value);
        }
    }
}
