using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  Person is an example of an SoA item type.
    /// </summary>
    public class Person : IRow
    {
        private PersonTable _table;
        private int _index;

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
            this._table = table;
            this._index = index;
        }

        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (PersonTable)table;
            _index = index;
        }

        // Properties for each column get and set array entries in the columns
        public byte Age
        {
            get => _table.Age[_index];
            set => _table.Age[_index] = value;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        // Not required; these overrides facilitate unit tests
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj)) { return true; }
            if (obj == null || !(obj is Person)) { return false; }

            Person other = obj as Person;
            return this.Age.Equals(other.Age) && string.Equals(this.Name, other.Name);
        }

        public override int GetHashCode()
        {
            return this.Age.GetHashCode() ^ (this.Name?.GetHashCode() ?? 0);
        }
    }
}
