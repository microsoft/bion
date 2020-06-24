using BSOA.Json;
using BSOA.Test.Model.V2;

using System;

using Xunit;

namespace BSOA.Test.Json
{
    public class GeneratedConverterTests
    {
        [Fact]
        public void JsonToPerson_Basics()
        {
            Community readRoot = new Community();
            Person p = new Person() { Name = "Scott", Birthdate = new DateTime(1981, 01, 01, 00, 00, 00, DateTimeKind.Utc) };

            // Serialization via typed methods
            JsonRoundTrip.ValueOnly(p, JsonToPerson.Write, (r, db) => JsonToPerson.Read(r, readRoot));
            JsonRoundTrip.ValueOnly(null, JsonToPerson.Write, (r, db) => JsonToPerson.Read(r, readRoot));
            JsonRoundTrip.NameAndValue(p, null, (w, pn, v, dv) => JsonToPerson.Write(w, pn, v), (r, db) => JsonToPerson.Read(r, readRoot));

            // JsonConverter.CanConvert (not called by default serialization)
            JsonToPerson converter = new JsonToPerson();
            Assert.True(converter.CanConvert(typeof(Person)));
            Assert.False(converter.CanConvert(typeof(Community)));

            // Serialization via Newtonsoft default
            string personPath = "Person.NewtonsoftDefault.json";
            AsJson.Save(personPath, p, true);
            Person roundTrip = AsJson.Load<Person>(personPath);
            Assert.Equal(p, roundTrip);

            // Serialize null via Newtonsoft
            AsJson.Save<Person>(personPath, null);
            roundTrip = AsJson.Load<Person>(personPath);
            Assert.Null(roundTrip);

            // Serialize empty root
            string communityPath = "Community.NewtonsoftDefault.json";
            AsJson.Save(communityPath, readRoot);
            Community roundTripCommunity = AsJson.Load<Community>(communityPath);
            Assert.Empty(roundTripCommunity.People);

            // Serialize root with Person
            readRoot.People.Add(p);
            AsJson.Save(communityPath, readRoot);
            roundTripCommunity = AsJson.Load<Community>(communityPath);
            Assert.Single(roundTripCommunity.People);
            Assert.Equal(p, roundTripCommunity.People[0]);
            Assert.Equal("Scott", roundTripCommunity.People[0].Name);
        }
    }
}
