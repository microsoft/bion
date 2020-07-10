// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Generator.Templates
{
    [JsonConverter(typeof(JsonToCompany))]
    public partial class Company
    { }

    internal class JsonToCompany : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Company, Company>> setters = new Dictionary<string, Action<JsonReader, Company, Company>>()
        {
            // <SetterList>
            //  <SimpleSetter>
            ["id"] = (reader, root, me) => me.Id = JsonToLong.Read(reader, root),
            //  </SimpleSetter>
            //   <EnumSetter>
            ["joinPolicy"] = (reader, root, me) => me.JoinPolicy = JsonToEnum<SecurityPolicy>.Read(reader, root),
            //   </EnumSetter>
            //   <RefSetter>
            ["owner"] = (reader, root, me) => me.Owner = JsonToEmployee.Read(reader, root),
            //   </RefSetter>
            //   <RefListSetter>
            ["members"] = (reader, root, me) => me.Members = JsonToIList<Employee>.Read(reader, root, null, JsonToEmployee.Read),
            //   </RefListSetter>
            // </SetterList>
        };

        public static Company Read(JsonReader reader, Company root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            Company item = new Company();

            // Company is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Company item, bool required = false)
        {
            if (required || item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Company item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                // <WriterList>
                //  <SimpleWriter>
                JsonToLong.Write(writer, "id", item.Id, 99);
                //  </SimpleWriter>
                //   <EnumWriter>
                JsonToEnum<SecurityPolicy>.Write(writer, "joinPolicy", item.JoinPolicy, SecurityPolicy.Open);
                //   </EnumWriter>
                //   <RefWriter>
                JsonToEmployee.Write(writer, "owner", item.Owner);
                //   </RefWriter>
                //   <RefListWriter>
                JsonToIList<Employee>.Write(writer, "members", item.Members, JsonToEmployee.Write);
                //   </RefListWriter>
                // </WriterList>
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Company));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Company)value);
        }
    }
}
