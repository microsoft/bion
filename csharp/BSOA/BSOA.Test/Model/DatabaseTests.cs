using BSOA.Test.Components;
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
    }
}
