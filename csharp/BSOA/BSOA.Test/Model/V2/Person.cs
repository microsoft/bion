using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Person'
    /// </summary>
    public partial class Person : IRow, IEquatable<Person>
    {
        private PersonTable _table;
        private int _index;

        internal Person(PersonTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Person(PersonTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Person(PersonDatabase database) : this(database.Person)
        { }

        public Person() : this(PersonDatabase.Current)
        { }

        public Person(
            DateTime birthdate,
            string name
        ) : this(PersonDatabase.Current)
        {
            Birthdate = birthdate;
            Name = name;
        }

        public Person(Person other)
        {
            Birthdate = other.Birthdate;
            Name = other.Name;
        }

        public DateTime Birthdate
        {
            get => _table.Birthdate[_index];
            set => _table.Birthdate[_index] = value;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IEquatable<Person>
        public bool Equals(Person other)
        {
            if (other == null) { return false; }

            if (this.Birthdate != other.Birthdate) { return false; }
            if (this.Name != other.Name) { return false; }
            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Birthdate != default(DateTime))
                {
                    result = (result * 31) + Birthdate.GetHashCode();
                }

                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public static bool operator ==(Person left, Person right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Person left, Person right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (PersonTable)table;
            _index = index;
        }
        #endregion
    }
}
