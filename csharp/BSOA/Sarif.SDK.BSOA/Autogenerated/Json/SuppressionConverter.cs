using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(SuppressionConverter))]
    public partial class Suppression
    { }
    
    public class SuppressionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Suppression));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadSuppression();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Suppression)value);
        }
    }
    
    internal static class SuppressionJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Suppression>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Suppression>>()
        {
            ["guid"] = (reader, root, me) => me.Guid = reader.ReadString(root),
            ["kind"] = (reader, root, me) => me.Kind = reader.ReadEnum<SuppressionKind, SarifLog>(root),
            ["status"] = (reader, root, me) => me.Status = reader.ReadEnum<SuppressionStatus, SarifLog>(root),
            ["justification"] = (reader, root, me) => me.Justification = reader.ReadString(root),
            ["location"] = (reader, root, me) => me.Location = reader.ReadLocation(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Suppression ReadSuppression(this JsonReader reader, SarifLog root = null)
        {
            Suppression item = (root == null ? new Suppression() : new Suppression(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Suppression item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Suppression item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("guid", item.Guid, default);
                writer.Write("kind", item.Kind);
                writer.Write("status", item.Status);
                writer.Write("justification", item.Justification, default);
                writer.Write("location", item.Location);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
