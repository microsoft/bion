namespace BSOA.Test.Model
{
    /// <summary>
    ///  Person is an example of an SoA item type.
    /// </summary>
    public class Person
    {
        // Item fields are the containing table and index in the table
        private PersonTable Table { get; }
        private int Index { get; }

        // Constructor to reference an existing item
        public Person(PersonTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        // Constructor to add a new item
        public Person(PersonTable table) : this(table, table.Count)
        {
            table.Add();
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
