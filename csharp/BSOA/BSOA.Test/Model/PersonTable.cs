using BSOA.Model;

namespace BSOA.Test.Model
{
    /// <summary>
    ///  PersonTable is an example of a SoA table type.
    /// </summary>
    public class PersonTable : Table<Person>
    {
        // Table needs hardcoded properties for each column
        internal NumberColumn<byte> Age;
        internal StringColumn Name;

        // Table constructor creates columns with the desired types and defaults.
        // Column names will be serialized and must be kept stable to maintain file compatibility.
        public PersonTable() : base()
        {
            // NOTE: All columns must be passed to 'AddColumn' so the base class can serialize them
            this.Age = AddColumn(nameof(Age), new NumberColumn<byte>(0));
            this.Name = AddColumn(nameof(Name), new StringColumn());
        }

        // Table provides indexer calling correct item constructor
        public override Person this[int index] => new Person(this, index);
    }
}
