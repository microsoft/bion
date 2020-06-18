using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(AddressConverter))]
    public partial class Address
    { }
    
    public class AddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Address));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadAddress();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Address)value);
        }
    }
    
    internal static class AddressJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Address>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Address>>()
        {
            ["absoluteAddress"] = (reader, root, me) => me.AbsoluteAddress = reader.ReadInt(root),
            ["relativeAddress"] = (reader, root, me) => me.RelativeAddress = reader.ReadInt(root),
            ["length"] = (reader, root, me) => me.Length = reader.ReadInt(root),
            ["kind"] = (reader, root, me) => me.Kind = reader.ReadString(root),
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root),
            ["fullyQualifiedName"] = (reader, root, me) => me.FullyQualifiedName = reader.ReadString(root),
            ["offsetFromParent"] = (reader, root, me) => me.OffsetFromParent = reader.ReadInt(root),
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = reader.ReadInt(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Address ReadAddress(this JsonReader reader, SarifLog root = null)
        {
            Address item = (root == null ? new Address() : new Address(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Address item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Address item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("absoluteAddress", item.AbsoluteAddress, -1);
                writer.Write("relativeAddress", item.RelativeAddress, default(int));
                writer.Write("length", item.Length, default(int));
                writer.Write("kind", item.Kind, default(string));
                writer.Write("name", item.Name, default(string));
                writer.Write("fullyQualifiedName", item.FullyQualifiedName, default(string));
                writer.Write("offsetFromParent", item.OffsetFromParent, default(int));
                writer.Write("index", item.Index, -1);
                writer.Write("parentIndex", item.ParentIndex, -1);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
