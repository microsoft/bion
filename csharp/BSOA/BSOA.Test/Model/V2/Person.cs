// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Person'
    /// </summary>
    public partial class Person : IRow<Person>, IEquatable<Person>
    {
        private PersonTable _table;
        private int _index;

        public Person() : this(PersonDatabase.Current.Person)
        { }

        public Person(Community root) : this(root.Database.Person)
        { }

        public Person(Community root, Person other) : this(root.Database.Person)
        {
            CopyFrom(other);
        }

        internal Person(PersonDatabase database, Person other) : this(database.Person)
        {
            CopyFrom(other);
        }

        internal Person(PersonTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Person(PersonTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public DateTime Birthdate
        {
            get => _table.Birthdate[_index, this];
            set => _table.Birthdate[_index, this] = value;
        }

        public string Name
        {
            get => _table.Name[_index, this];
            set => _table.Name[_index, this] = value;
        }

        #region IEquatable<Person>
        public bool Equals(Person other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.Birthdate, other.Birthdate)) { return false; }
            if (!object.Equals(this.Name, other.Name)) { return false; }

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

        void IRow.Remap(ITable table, int index)
        {
            _table = (PersonTable)table;
            _index = index;
        }

        public void CopyFrom(Person other)
        {
            Birthdate = other.Birthdate;
            Name = other.Name;
        }

        internal static Person Copy(PersonDatabase database, Person other)
        {
            return (other == null ? null : new Person(database, other));
        }
        #endregion
    }
}
