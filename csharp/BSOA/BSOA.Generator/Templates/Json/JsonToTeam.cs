using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    [JsonConverter(typeof(JsonToTeam))]
    public partial class Team
    { }
    
    internal class JsonToTeam : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Company, Team>> setters = new Dictionary<string, Action<JsonReader, Company, Team>>()
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
            ["members"] = (reader, root, me) => JsonToIList<Employee>.Read(reader, root, me.Members, JsonToEmployee.Read),
            //   </RefListSetter>
            // </SetterList>
        };

        public static Team Read(JsonReader reader, Company root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Team item = (root == null ? new Team() : new Team(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Team item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Team item)
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
            return objectType.Equals(typeof(Team));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Team)value);
        }
    }
}
