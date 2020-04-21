using BSOA.Test.Components;
using Xunit;

namespace BSOA.Test.Model
{
    public class TableTests
    {
        [Fact]
        public void Table_Basics()
        {
            PersonTable table = new PersonTable();
            Person one = new Person(table) { Age = 39, Name = "Scott" };
            Person two = new Person(table) { Age = 36, Name = "Adam" };

            // Use ReadOnlyList.VerifySame to check count, enumerators, and indexer
            ReadOnlyList.VerifySame(table, TreeSerializable.RoundTripBinary(table));
        }
    }
}
