using System;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  Person (V2) shows a change to Person - replacing 'Age' with 'Birthdate'
    /// </summary>
    public class Person
    {
        private PersonTable Table { get; }
        private int Index { get; }

        public Person(PersonTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        public Person(PersonTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Person(PersonDatabase database) : this(database.Person)
        { }

        // Properties for each column get and set array entries in the columns
        
        //public byte Age
        //{
        //    get => Table.Age[Index];
        //    set => Table.Age[Index] = value;
        //}

        public DateTime Birthdate
        {
            get => Table.Birthdate[Index];
            set => Table.Birthdate[Index] = value;
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
            return this.Birthdate.Equals(other.Birthdate) && this.Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return this.Birthdate.GetHashCode() ^ this.Name.GetHashCode();
        }
    }
}
