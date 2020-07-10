// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Demo.Model.BSOA
{
    [JsonConverter(typeof(JsonToFolder))]
    public partial class Folder
    { }
    
    internal class JsonToFolder : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, FileSystem, Folder>> setters = new Dictionary<string, Action<JsonReader, FileSystem, Folder>>()
        {
            ["parentIndex"] = (reader, root, me) => me.ParentIndex = JsonToInt.Read(reader, root),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root)
        };

        public static Folder Read(JsonReader reader, FileSystem root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Folder item = (root == null ? new Folder() : new Folder(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Folder item, bool required = false)
        {
            if (required || item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Folder item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "parentIndex", item.ParentIndex, default);
                JsonToString.Write(writer, "name", item.Name, default);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Folder));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Folder)value);
        }
    }
}
