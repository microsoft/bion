using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(NotificationConverter))]
    public partial class Notification
    { }
    
    public class NotificationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Notification));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadNotification();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Notification)value);
        }
    }
    
    internal static class NotificationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Notification>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Notification>>()
        {
            ["locations"] = (reader, root, me) => reader.ReadList(root, me.Locations, LocationJsonExtensions.ReadLocation),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["level"] = (reader, root, me) => me.Level = reader.ReadEnum<FailureLevel, SarifLog>(root),
            ["threadId"] = (reader, root, me) => me.ThreadId = reader.ReadInt(root),
            ["timeUtc"] = (reader, root, me) => me.TimeUtc = reader.ReadDateTime(root),
            ["exception"] = (reader, root, me) => me.Exception = reader.ReadExceptionData(root),
            ["descriptor"] = (reader, root, me) => me.Descriptor = reader.ReadReportingDescriptorReference(root),
            ["associatedRule"] = (reader, root, me) => me.AssociatedRule = reader.ReadReportingDescriptorReference(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Notification ReadNotification(this JsonReader reader, SarifLog root = null)
        {
            Notification item = (root == null ? new Notification() : new Notification(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Notification item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Notification item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteList("locations", item.Locations, LocationJsonExtensions.Write);
                writer.Write("message", item.Message);
                writer.Write("level", item.Level);
                writer.Write("threadId", item.ThreadId, default(int));
                writer.Write("timeUtc", item.TimeUtc, default(DateTime));
                writer.Write("exception", item.Exception);
                writer.Write("descriptor", item.Descriptor);
                writer.Write("associatedRule", item.AssociatedRule);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
