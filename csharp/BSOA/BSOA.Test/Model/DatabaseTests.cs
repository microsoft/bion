using BSOA.IO;
using BSOA.Json;
using BSOA.Test.Components;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace BSOA.Test.Model
{
    public class DatabaseTests
    {
        [Fact]
        public void Database_Basics()
        {
            V1.PersonDatabase database = new V1.PersonDatabase();
            database.Person.Add(new V1.Person() { Age = 39, Name = "Scott" });
            database.Person.Add(new V1.Person() { Age = 36, Name = "Adam" });

            // Use ReadOnlyList.VerifySame to check count, enumerators, and indexer
            V1.PersonDatabase roundTripped = TreeSerializer.RoundTrip(database, TreeFormat.Binary);
            ReadOnlyList.VerifySame(database.Person, roundTripped.Person);

            // Try loading database with size diagnostics
            TreeDiagnostics diagnostics = TreeSerializer.Diagnostics(database, TreeFormat.Binary);

            // Verify table and column names in diagnostics
            StringBuilder textBuilder = new StringBuilder();
            diagnostics.Write(new StringWriter(textBuilder), -1);
            string text = textBuilder.ToString();
            Assert.Contains("Person", text);
            Assert.Contains("Age", text);
            Assert.Contains("Name", text);

            // Verify one table, two columns, Write doesn't throw
            Assert.Single(diagnostics.Children);
            Assert.Equal(nameof(database.Person), diagnostics.Children[0].Name);
            Assert.Equal("Columns", diagnostics.Children[0].Children[0].Name);
            Assert.Equal(2, diagnostics.Children[0].Children[0].Children.Count);
            diagnostics.Write(Console.Out, 3);

            // Verify Trim doesn't throw (results not visible)
            database.Trim();
            ReadOnlyList.VerifySame(database.Person, roundTripped.Person);

            // Verify Database.Clear works
            database.Clear();
            Assert.Empty(database.Person);
            Assert.Equal(database.Person[0], database.Person.Add());
        }

        [Fact]
        public void Database_ReplaceColumn()
        {
            // Test saving a database and then loading it into a different object model with added and removed columns.

            V1.PersonDatabase v1 = new V1.PersonDatabase();
            v1.Person.Add(new V1.Person() { Age = 39, Name = "Scott" });
            v1.Person.Add(new V1.Person() { Age = 36, Name = "Adam" });

            string filePath = "People.bsoa.bin";

            // Save V1 PersonDatabase (Age and Name)
            v1.Save(filePath, TreeFormat.Binary);

            // Load as V2 PersonDatabase (BirthDate and Name)
            V2.PersonDatabase v2 = new V2.PersonDatabase();
            v2.Load(filePath, TreeFormat.Binary);

            // Verify row count the same, Name loaded properly
            Assert.Equal(v1.Person.Count, v2.Person.Count);
            Assert.Equal(v1.Person[0].Name, v2.Person[0].Name);

            DateTime birthdate = DateTime.Parse("1981-01-01").ToUniversalTime();
            v2.Person[0].Birthdate = birthdate;

            // Verify new database serializes new column
            V2.PersonDatabase v2RoundTrip = new V2.PersonDatabase();

            v2.Save(filePath, TreeFormat.Binary);
            v2RoundTrip.Load(filePath, TreeFormat.Binary);

            Assert.Equal(birthdate, v2RoundTrip.Person[0].Birthdate);
            ReadOnlyList.VerifySame(v2.Person, v2RoundTrip.Person);

            // Load *new format* into V1 object model
            V1.PersonDatabase v1RoundTrip = new V1.PersonDatabase();
            v1RoundTrip.Load(filePath, TreeFormat.Binary);

            // Verify unchanged columns come back
            Assert.Equal(v1.Person.Count, v1RoundTrip.Person.Count);
            Assert.Equal(v1.Person[0].Name, v1RoundTrip.Person[0].Name);

            // Verify Age empty
            Assert.Equal(0, v1RoundTrip.Person[0].Age);

            // Read with TreeSerializationSettings.Strict and verify error
            Assert.Throws<IOException>(() => v1RoundTrip.Load(filePath, TreeFormat.Binary, new BSOA.IO.TreeSerializationSettings() { Strict = true }));
        }

        [Fact]
        public void Database_NewtonsoftCompatibility()
        {
            // Test that basic Database, Table, and Row types can successfully roundtrip via Newtonsoft.Json serialization by default.
            
            // This requires:
            //  - Tables exposed by Database (or collections which represent them are exposed)
            //  - Table pre-constructed by Database constructor.
            //  - Table has Add(T item).
            //  - Item has an empty constructor.
            //  - Item empty constructor chooses the right Database/Table or cross-table copy works correctly.
            //  - Item properties are settable and types are understandable to Newtonsoft.Json.

            V1.PersonDatabase v1 = new V1.PersonDatabase();
            v1.Person.Add(new V1.Person() { Age = 39, Name = "Scott" });
            v1.Person.Add(new V1.Person() { Age = 36, Name = "Adam" });

            string serializeToPath = "PersonDatabase.V1.json";
            AsJson.Save(serializeToPath, v1);

            V1.PersonDatabase roundTrip = AsJson.Load<V1.PersonDatabase>(serializeToPath);
            ReadOnlyList.VerifySame(v1.Person, roundTrip.Person);

            AsJson.Save(serializeToPath, v1, verbose: true);
            roundTrip = AsJson.Load<V1.PersonDatabase>(serializeToPath);
            ReadOnlyList.VerifySame(v1.Person, roundTrip.Person);
        }
    }
}
