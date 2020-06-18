using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    [JsonConverter(typeof(CompanyConverter))]
    public partial class Company
    { }

    public class CompanyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Company));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadCompany();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Company)value);
        }
    }

    internal static class CompanyJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, Company, Company>> setters = new Dictionary<string, Action<JsonReader, Company, Company>>()
        {
            // <SetterList>
            //  <SimpleSetter>
            ["id"] = (reader, root, me) => me.Id = reader.ReadLong(root),
            //  </SimpleSetter>
            //   <EnumSetter>
            ["joinPolicy"] = (reader, root, me) => me.JoinPolicy = reader.ReadEnum<SecurityPolicy, Company>(root),
            //   </EnumSetter>
            //   <RefSetter>
            ["owner"] = (reader, root, me) => me.Owner = reader.ReadEmployee(root),
            //   </RefSetter>
            //   <RefListSetter>
            ["members"] = (reader, root, me) => reader.ReadList(root, me.Members, EmployeeJsonExtensions.ReadEmployee),
            //   </RefListSetter>
            // </SetterList>
        };

        public static Company ReadCompany(this JsonReader reader, Company root = null)
        {
            Company item = new Company();
            root = item;

            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, Company item)
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
                writer.Write("id", item.Id, 99);
                //  </SimpleWriter>
                //   <EnumWriter>
                writer.WriteEnum("joinPolicy", item.JoinPolicy, SecurityPolicy.Open);
                //   </EnumWriter>
                //   <RefWriter>
                writer.Write("owner", item.Owner);
                //   </RefWriter>
                //   <RefListWriter>
                writer.WriteList("members", item.Members, EmployeeJsonExtensions.Write);
                //   </RefListWriter>
                // </WriterList>
                writer.WriteEndObject();
            }
        }
    }
}
