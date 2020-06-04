using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Root'
    /// </summary>
    internal partial class Root : IRow, IEquatable<Root>
    {
        private RootTable _table;
        private int _index;

        internal Root(RootTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Root(RootTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Root(PersonDatabase database) : this(database.Root)
        { }

        public Root() : this(PersonDatabase.Current)
        { }

        public Root(
            IList<Person> people
        ) : this(PersonDatabase.Current)
        {
            People = people;
        }

        public Root(Root other)
        {
            People = other.People;
        }

        public IList<Person> People
        {
            get => _table.Database.Person.List(_table.People[_index]);
            set => _table.Database.Person.List(_table.People[_index]).SetTo(value);
        }

        #region IEquatable<Root>
        public bool Equals(Root other)
        {
            if (other == null) { return false; }

            if (this.People != other.People) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (People != default(IList<Person>))
                {
                    result = (result * 31) + People.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Root);
        }

        public static bool operator ==(Root left, Root right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Root left, Root right)
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
            _table = (RootTable)table;
            _index = index;
        }
        #endregion
    }
}
