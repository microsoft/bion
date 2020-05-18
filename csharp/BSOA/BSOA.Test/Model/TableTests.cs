using BSOA.Test.Model.V1;
using Xunit;

namespace BSOA.Test.Model
{
    public class TableTests
    {
        [Fact]
        public void Table_AsColumn()
        {
            // Tables are usable as Columns, allowing nesting a full table of columns as a single named column.
            // Test Table ILimitedList members by using the Column.Basics testing.

            // But:
            //  - Default value is a new instance (not null, an instance with all default values)
            //  - Cannot set a value to null.

            V1.PersonDatabase db = new V1.PersonDatabase();
            V1.PersonTable other = db.Person;

            Column.Basics<V1.Person>(
                () => new PersonTable(new PersonDatabase()),
                new Person(other),
                new Person(other) { Age = 39, Name = "Scott" },
                (i) => new Person(other) { Age = (byte)(i % byte.MaxValue) }
            );
        }
    }
}
