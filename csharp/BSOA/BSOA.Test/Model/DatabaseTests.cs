using BSOA.Test.Components;
using System;
using System.IO;
using Xunit;

namespace BSOA.Test.Model
{
    public class DatabaseTests
    {
        [Fact]
        public void Database_Basics()
        {
            PersonDatabase database = new PersonDatabase();
            PersonTable table = database.People;
            Person one = new Person(table) { Age = 39, Name = "Scott" };
            Person two = new Person(table) { Age = 36, Name = "Adam" };

            // Use ReadOnlyList.VerifySame to check count, enumerators, and indexer
            PersonDatabase roundTripped = TreeSerializable.RoundTrip(database, TreeFormat.Binary);
            ReadOnlyList.VerifySame(table, roundTripped.People);

            // Verify Database.Clear works
            database.Clear();
            Assert.Empty(database.People);
            Assert.Equal(0, database.People[0].Age);
        }

        [Fact]
        public void Database_ReplaceColumn()
        {
            PersonDatabase v1 = new PersonDatabase();
            new Person(v1.People) { Age = 39, Name = "Scott" };
            new Person(v1.People) { Age = 36, Name = "Adam" };

            string filePath = "People.bsoa.bin";

            // Save V1 PersonDatabase (Age and Name)
            v1.Save(filePath, TreeFormat.Binary);

            // Load as V2 PersonDatabase (BirthDate and Name)
            V2.PersonDatabase v2 = new V2.PersonDatabase();
            v2.Load(filePath, TreeFormat.Binary);

            // Verify row count the same, Name loaded properly
            Assert.Equal(v1.People.Count, v2.People.Count);
            Assert.Equal(v1.People[0].Name, v2.People[0].Name);

            DateTime birthdate = DateTime.Parse("1981-01-01").ToUniversalTime();
            v2.People[0].Birthdate = birthdate;

            // Verify new database serializes new column
            V2.PersonDatabase v2RoundTrip = new V2.PersonDatabase();

            v2.Save(filePath, TreeFormat.Binary);
            v2RoundTrip.Load(filePath, TreeFormat.Binary);

            Assert.Equal(birthdate, v2RoundTrip.People[0].Birthdate);
            ReadOnlyList.VerifySame(v2.People, v2RoundTrip.People);

            // Load *new format* into V1 object model
            PersonDatabase v1RoundTrip = new PersonDatabase();
            v1RoundTrip.Load(filePath, TreeFormat.Binary);

            // Verify unchanged columns come back
            Assert.Equal(v1.People.Count, v1RoundTrip.People.Count);
            Assert.Equal(v1.People[0].Name, v1RoundTrip.People[0].Name);

            // Verify Age empty
            Assert.Equal(0, v1RoundTrip.People[0].Age);

            // Read with TreeSerializationSettings.Strict and verify error
            Assert.Throws<IOException>(() => v1RoundTrip.Load(filePath, TreeFormat.Binary, new BSOA.IO.TreeSerializationSettings() { Strict = true }));
        }
    }
}
