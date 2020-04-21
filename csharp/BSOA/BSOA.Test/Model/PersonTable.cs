using BSOA.Model;

namespace BSOA.Test.Model
{
    /// <summary>
    ///  PersonTable is an example of a SoA table type.
    /// </summary>
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

        // Table provides indexer calling correct item constructor
        public override Person this[int index] => new Person(this, index);
    }
}
