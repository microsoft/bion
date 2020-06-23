using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToNotification))]
    public partial class Notification
    { }
    
    internal class JsonToNotification : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Notification>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Notification>>()
        {
            ["locations"] = (reader, root, me) => JsonToIList<Location>.Read(reader, root, me.Locations, JsonToLocation.Read),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["level"] = (reader, root, me) => me.Level = JsonToEnum<FailureLevel>.Read(reader, root),
            ["threadId"] = (reader, root, me) => me.ThreadId = JsonToInt.Read(reader, root),
            ["timeUtc"] = (reader, root, me) => me.TimeUtc = JsonToDateTime.Read(reader, root),
            ["exception"] = (reader, root, me) => me.Exception = JsonToExceptionData.Read(reader, root),
            ["descriptor"] = (reader, root, me) => me.Descriptor = JsonToReportingDescriptorReference.Read(reader, root),
            ["associatedRule"] = (reader, root, me) => me.AssociatedRule = JsonToReportingDescriptorReference.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Notification Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Notification item = (root == null ? new Notification() : new Notification(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Notification item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Notification item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<Location>.Write(writer, "locations", item.Locations, JsonToLocation.Write);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToEnum<FailureLevel>.Write(writer, "level", item.Level, FailureLevel.Warning);
                JsonToInt.Write(writer, "threadId", item.ThreadId, default);
                JsonToDateTime.Write(writer, "timeUtc", item.TimeUtc, default);
                JsonToExceptionData.Write(writer, "exception", item.Exception);
                JsonToReportingDescriptorReference.Write(writer, "descriptor", item.Descriptor);
                JsonToReportingDescriptorReference.Write(writer, "associatedRule", item.AssociatedRule);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Notification));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Notification)value);
        }
    }
}
