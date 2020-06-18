using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V1
{
    [JsonConverter(typeof(CommunityConverter))]
    public partial class Community
    { }

    public class CommunityConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Community));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadCommunity();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Community)value);
        }
    }

    internal static class CommunityJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, Community, Community>> setters = new Dictionary<string, Action<JsonReader, Community, Community>>()
        {
            ["people"] = (reader, root, me) => reader.ReadList(root, me.People, PersonJsonExtensions.ReadPerson)
        };

        public static Community ReadCommunity(this JsonReader reader, Community root = null)
        {
            Community item = new Community();
            root = item;

            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, Community item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteList("people", item.People, PersonJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
