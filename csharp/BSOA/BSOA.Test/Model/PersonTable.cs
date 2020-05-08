using BSOA.Column;
using BSOA.Model;

namespace BSOA.Test.Model
{
    /// <summary>
    ///  PersonTable is an example of a SoA table type.
    /// </summary>
    /// <remarks>
    ///  - Has strongly typed column properties for instant access by item struct properties.
    ///  - Controls column construction (types and defaults).
    ///  - Calls AddColumn on columns (so base class can manage serialization)
    ///  - Provides indexer (controls construction of item instances).
    /// </remarks>
    public class PersonTable : Table<Person>
    {
        // Tables need Database reference (so items with only table reference can refer to items in other tables)
        internal PersonDatabase Database;

        // Table needs hardcoded properties for each column
        internal NumberColumn<byte> Age;
        internal StringColumn Name;

        // Table constructor creates columns with the desired types and defaults.
        // Column names will be serialized and must be kept stable to maintain file compatibility.
        public PersonTable(PersonDatabase database) : base()
        {
            Database = database;

            // NOTE: All columns must be passed to 'AddColumn' so the base class can serialize them
            Age = AddColumn(nameof(Age), new NumberColumn<byte>(0));
            Name = AddColumn(nameof(Name), new StringColumn());
        }

        protected override Person Get(int index)
        {
            return new Person(this, index);
        }
    }
}
