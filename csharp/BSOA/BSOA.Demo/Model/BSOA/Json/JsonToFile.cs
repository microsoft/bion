// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Demo.Model.BSOA
{
    [JsonConverter(typeof(JsonToFile))]
    public partial class File
    { }
    
    internal class JsonToFile : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, FileSystem, File>> setters = new Dictionary<string, Action<JsonReader, FileSystem, File>>()
        {
            ["parentFolderIndex"] = (reader, root, me) => me.ParentFolderIndex = JsonToInt.Read(reader, root),
            ["name"] = (reader, root, me) => me.Name = JsonToString.Read(reader, root),
            ["lastModifiedUtc"] = (reader, root, me) => me.LastModifiedUtc = JsonToDateTime.Read(reader, root),
            ["createdUtc"] = (reader, root, me) => me.CreatedUtc = JsonToDateTime.Read(reader, root),
            ["attributes"] = (reader, root, me) => me.Attributes = JsonToEnum<System.IO.FileAttributes>.Read(reader, root),
            ["length"] = (reader, root, me) => me.Length = JsonToLong.Read(reader, root)
        };

        public static File Read(JsonReader reader, FileSystem root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            File item = (root == null ? new File() : new File(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, File item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, File item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "parentFolderIndex", item.ParentFolderIndex, default);
                JsonToString.Write(writer, "name", item.Name, default);
                JsonToDateTime.Write(writer, "lastModifiedUtc", item.LastModifiedUtc, default);
                JsonToDateTime.Write(writer, "createdUtc", item.CreatedUtc, default);
                JsonToEnum<System.IO.FileAttributes>.Write(writer, "attributes", item.Attributes, default);
                JsonToLong.Write(writer, "length", item.Length, default);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(File));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (File)value);
        }
    }
}
