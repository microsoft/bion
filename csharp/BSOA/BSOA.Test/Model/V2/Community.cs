// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using BSOA.IO;
using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'Community'
    /// </summary>
    public partial class Community : IRow
    {
        private CommunityTable _table;
        private int _index;

        internal PersonDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;

        public Community() : this(new PersonDatabase().Community)
        { }

        internal Community(CommunityTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Community(CommunityTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public IList<Person> People
        {
            get => _table.Database.Person.List(_table.People[_index]);
            set => _table.Database.Person.List(_table.People[_index]).SetTo(value);
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
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region Easy Serialization
        public void WriteBsoa(Stream stream)
        {
            using (BinaryTreeWriter writer = new BinaryTreeWriter(stream))
            {
                DB.Write(writer);
            }
        }

        public void WriteBsoa(string filePath)
        {
            WriteBsoa(File.Create(filePath));
        }

        public static Community ReadBsoa(Stream stream)
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
            return ReadBsoa(File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(string filePath)
        {
            return Diagnostics(File.OpenRead(filePath));
        }

        public static TreeDiagnostics Diagnostics(Stream stream)
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
