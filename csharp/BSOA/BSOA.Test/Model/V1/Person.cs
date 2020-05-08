using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  Person is an example of an SoA item type.
    /// </summary>
    public class Person : IRow
    {
        private PersonTable Table { get; set; }
        private int Index { get; set; }

        public Person() : this(PersonDatabase.Current)
        { }

        public Person(PersonDatabase database) : this(database.Person)
        { }

        public Person(PersonTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Person(PersonTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        ITable IRow.Table => Table;
        int IRow.Index => Index;

        void IRow.Reset(ITable table, int index)
        {
            Table = (PersonTable)table;
            Index = index;
        }

        // Properties for each column get and set array entries in the columns
        public byte Age
        {
            get => Table.Age[Index];
            set => Table.Age[Index] = value;
        }

        public string Name
        {
            get => Table.Name[Index];
            set => Table.Name[Index] = value;
        }

        // Not required; these overrides facilitate unit tests
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Person)) { return false; }

            Person other = obj as Person;
            return this.Age.Equals(other.Age) && this.Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return this.Age.GetHashCode() ^ this.Name.GetHashCode();
        }
    }
}
