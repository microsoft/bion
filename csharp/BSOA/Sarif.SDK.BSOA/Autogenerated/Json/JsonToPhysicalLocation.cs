using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToPhysicalLocation))]
    public partial class PhysicalLocation
    { }
    
    internal class JsonToPhysicalLocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, PhysicalLocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, PhysicalLocation>>()
        {
            ["address"] = (reader, root, me) => me.Address = JsonToAddress.Read(reader, root),
            ["artifactLocation"] = (reader, root, me) => me.ArtifactLocation = JsonToArtifactLocation.Read(reader, root),
            ["region"] = (reader, root, me) => me.Region = JsonToRegion.Read(reader, root),
            ["contextRegion"] = (reader, root, me) => me.ContextRegion = JsonToRegion.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static PhysicalLocation Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            PhysicalLocation item = (root == null ? new PhysicalLocation() : new PhysicalLocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, PhysicalLocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, PhysicalLocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToAddress.Write(writer, "address", item.Address);
                JsonToArtifactLocation.Write(writer, "artifactLocation", item.ArtifactLocation);
                JsonToRegion.Write(writer, "region", item.Region);
                JsonToRegion.Write(writer, "contextRegion", item.ContextRegion);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(PhysicalLocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (PhysicalLocation)value);
        }
    }
}
