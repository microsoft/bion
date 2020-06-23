using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToSuppression))]
    public partial class Suppression
    { }
    
    internal class JsonToSuppression : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Suppression>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Suppression>>()
        {
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["kind"] = (reader, root, me) => me.Kind = JsonToEnum<SuppressionKind>.Read(reader, root),
            ["status"] = (reader, root, me) => me.Status = JsonToEnum<SuppressionStatus>.Read(reader, root),
            ["justification"] = (reader, root, me) => me.Justification = JsonToString.Read(reader, root),
            ["location"] = (reader, root, me) => me.Location = JsonToLocation.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Suppression Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Suppression item = (root == null ? new Suppression() : new Suppression(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Suppression item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Suppression item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToEnum<SuppressionKind>.Write(writer, "kind", item.Kind, default(SuppressionKind));
                JsonToEnum<SuppressionStatus>.Write(writer, "status", item.Status, default(SuppressionStatus));
                JsonToString.Write(writer, "justification", item.Justification, default);
                JsonToLocation.Write(writer, "location", item.Location);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Suppression));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Suppression)value);
        }
    }
}
