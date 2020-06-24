// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Test.Model.V2
{
    [JsonConverter(typeof(JsonToPerson))]
    public partial class Person
    { }
    
    internal class JsonToPerson : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Community, Person>> setters = new Dictionary<string, Action<JsonReader, Community, Person>>()
        {
            ["birthdate"] = (reader, root, me) => me.Birthdate = JsonToDateTime.Read(reader, root),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root)
        };

        public static Person Read(JsonReader reader, Community root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Person item = (root == null ? new Person() : new Person(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Person item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Person item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToDateTime.Write(writer, "birthdate", item.Birthdate, default);
                JsonToString.Write(writer, "name", item.Name, default);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Person));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Person)value);
        }
    }
}
