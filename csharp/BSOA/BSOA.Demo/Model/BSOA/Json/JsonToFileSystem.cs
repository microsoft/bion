// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Demo.Model.BSOA
{
    [JsonConverter(typeof(JsonToFileSystem))]
    public partial class FileSystem
    { }

    internal class JsonToFileSystem : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, FileSystem, FileSystem>> setters = new Dictionary<string, Action<JsonReader, FileSystem, FileSystem>>()
        {
            ["folders"] = (reader, root, me) => JsonToIList<Folder>.Read(reader, root, me.Folders, JsonToFolder.Read),
            ["files"] = (reader, root, me) => JsonToIList<File>.Read(reader, root, me.Files, JsonToFile.Read)
        };

        public static FileSystem Read(JsonReader reader, FileSystem root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            FileSystem item = new FileSystem();

            // FileSystem is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, FileSystem item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, FileSystem item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<Folder>.Write(writer, "folders", item.Folders, JsonToFolder.Write);
                JsonToIList<File>.Write(writer, "files", item.Files, JsonToFile.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(FileSystem));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (FileSystem)value);
        }
    }
}
