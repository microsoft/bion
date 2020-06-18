using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V1
{
    [JsonConverter(typeof(PersonConverter))]
    public partial class Person
    { }
    
    public class PersonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Person));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadPerson();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Person)value);
        }
    }
    
    internal static class PersonJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, Community, Person>> setters = new Dictionary<string, Action<JsonReader, Community, Person>>()
        {
            ["age"] = (reader, root, me) => me.Age = reader.ReadByte(root),
            ["name"] = (reader, root, me) => me.Name = reader.ReadString(root)
        };

        public static Person ReadPerson(this JsonReader reader, Community root = null)
        {
            Person item = (root == null ? new Person() : new Person(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Person item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Person item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("age", item.Age, default);
                writer.Write("name", item.Name, default);
                writer.WriteEndObject();
            }
        }
    }
}
