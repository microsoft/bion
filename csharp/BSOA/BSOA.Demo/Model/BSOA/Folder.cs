// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Folder'
    /// </summary>
    public partial class Folder : IRow<Folder>, IEquatable<Folder>
    {
        private readonly FolderTable _table;
        private readonly int _index;

        public Folder() : this(FileSystemDatabase.Current.Folder)
        { }

        public Folder(FileSystem root) : this(root.Database.Folder)
        { }

        public Folder(FileSystem root, Folder other) : this(root.Database.Folder)
        {
            CopyFrom(other);
        }

        internal Folder(FolderTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Folder(FolderTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IEquatable<Folder>
        public bool Equals(Folder other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.ParentIndex, other.ParentIndex)) { return false; }
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
                if (ParentIndex != default(int))
                {
                    result = (result * 31) + ParentIndex.GetHashCode();
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
            return Equals(obj as Folder);
        }

        public static bool operator ==(Folder left, Folder right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Folder left, Folder right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow<Folder>.Table => _table;
        int IRow<Folder>.Index => _index;

        public void CopyFrom(Folder other)
        {
            ParentIndex = other.ParentIndex;
            Name = other.Name;
        }
        #endregion
    }
}
