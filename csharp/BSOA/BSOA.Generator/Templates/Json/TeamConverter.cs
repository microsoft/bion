using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    [JsonConverter(typeof(TeamConverter))]
    public partial class Team
    { }
    
    public class TeamConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Team));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadTeam();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Team)value);
        }
    }
    
    internal static class TeamJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, Company, Team>> setters = new Dictionary<string, Action<JsonReader, Company, Team>>()
        {
            // <SetterList>
            //  <SimpleSetter>
            ["id"] = (reader, root, me) => me.Id = reader.ReadLong(root),
            //  </SimpleSetter>
            //   <EnumSetter>
            ["joinPolicy"] = (reader, root, me) => me.JoinPolicy = (JoinPolicy)reader.ReadLong(root),
            //   </EnumSetter>
            //   <RefSetter>
            ["owner"] = (reader, root, me) => me.Owner = EmployeeJsonExtensions.ReadEmployee,
            //   </RefSetter>
            //   <RefListSetter>
            ["members"] = (reader, root, me) => reader.ReadList(root, me.Members, EmployeeJsonExtensions.ReadEmployee),
            //   </RefListSetter>
            // </SetterList>
        };

        public static Team ReadTeam(this JsonReader reader, Company root = null)
        {
            Team item = (root == null ? new Team() : new Team(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Team item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Team item)
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
                writer.Write("joinPolicy", (int)item.JoinPolicy);
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
