// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'Community'
    /// </summary>
    public partial class Community : IRow<Community>, IEquatable<Community>
    {
        private CommunityTable _table;
        private int _index;

        internal PersonDatabase Database => _table.Database;
        public IDatabase DB => _table.Database;

        public Community() : this(new PersonDatabase().Community)
        { }

        public Community(Community other) : this(new PersonDatabase().Community)
        {
            CopyFrom(other);
        }

        internal Community(CommunityTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Community(CommunityTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        private TypedList<Person> _people;
        public IList<Person> People
        {
            get
            {
                if (_people == null) { _people = TypedList<Person>.Get(_table.Database.Person, _table.People, _index); }
                return _people;
            }
            set
            {
                TypedList<Person>.Set(_table.Database.Person, _table.People, _index, value);
                _people = null;
            }
        }

        #region IEquatable<Community>
        public bool Equals(Community other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.People, other.People)) { return false; }

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
            return Equals(obj as Community);
        }

        public static bool operator ==(Community left, Community right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Community left, Community right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow<Community>.Table => _table;
        int IRow<Community>.Index => _index;

        void IRow<Community>.Remap(ITable table, int index)
        {
            _table = (CommunityTable)table;
            _index = index;
        }

        public void CopyFrom(Community other)
        {
            People = other.People?.Select((item) => Person.Copy(_table.Database, item)).ToList();
        }
        #endregion

        #region Easy Serialization
        public void WriteBsoa(System.IO.Stream stream)
        {
            using (BinaryTreeWriter writer = new BinaryTreeWriter(stream))
            {
                DB.Write(writer);
            }
        }

        public void WriteBsoa(string filePath)
        {
            WriteBsoa(System.IO.File.Create(filePath));
        }

        public static Community ReadBsoa(System.IO.Stream stream)
        {
            using (BinaryTreeReader reader = new BinaryTreeReader(stream))
            {
                Community result = new Community();
                result.DB.Read(reader);
                return result;
            }
        }

        public static Community ReadBsoa(string filePath)
        {
            return ReadBsoa(System.IO.File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(string filePath)
        {
            return Diagnostics(System.IO.File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(System.IO.Stream stream)
        {
            using (BinaryTreeReader btr = new BinaryTreeReader(stream))
            using (TreeDiagnosticsReader reader = new TreeDiagnosticsReader(btr))
            {
                Community result = new Community();
                result.DB.Read(reader);
                return reader.Tree;
            }
        }
        #endregion
    }
}
