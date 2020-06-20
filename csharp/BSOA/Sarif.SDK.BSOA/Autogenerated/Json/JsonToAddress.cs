using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToAddress))]
    public partial class Address
    { }
    
    internal class JsonToAddress : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Address>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Address>>()
        {
            ["absoluteAddress"] = (reader, root, me) => me.AbsoluteAddress = JsonToInt.Read(reader, root),
            ["relativeAddress"] = (reader, root, me) => me.RelativeAddress = JsonToInt.Read(reader, root),
            ["length"] = (reader, root, me) => me.Length = JsonToInt.Read(reader, root),
            ["kind"] = (reader, root, me) => me.Kind = JsonToString.Read(reader, root),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["fullyQualifiedName"] = (reader, root, me) => me.FullyQualifiedName = JsonToString.Read(reader, root),
            ["offsetFromParent"] = (reader, root, me) => me.OffsetFromParent = JsonToInt.Read(reader, root),
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = JsonToInt.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Address Read(JsonReader reader, SarifLog root = null)
        {
            Address item = (root == null ? new Address() : new Address(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Address item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Address item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "absoluteAddress", item.AbsoluteAddress, -1);
                JsonToInt.Write(writer, "relativeAddress", item.RelativeAddress, default);
                JsonToInt.Write(writer, "length", item.Length, default);
                JsonToString.Write(writer, "kind", item.Kind, default);
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToString.Write(writer, "fullyQualifiedName", item.FullyQualifiedName, default);
                JsonToInt.Write(writer, "offsetFromParent", item.OffsetFromParent, default);
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToInt.Write(writer, "parentIndex", item.ParentIndex, -1);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Address));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Address)value);
        }
    }
}
