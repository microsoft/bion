// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'File'
    /// </summary>
    public partial class File : IRow<File>, IEquatable<File>
    {
        private FileTable _table;
        private int _index;

        public File() : this(FileSystemDatabase.Current.File)
        { }

        public File(FileSystem root) : this(root.Database.File)
        { }

        public File(FileSystem root, File other) : this(root.Database.File)
        {
            CopyFrom(other);
        }

        internal File(FileSystemDatabase database, File other) : this(database.File)
        {
            CopyFrom(other);
        }

        internal File(FileTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal File(FileTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public int ParentFolderIndex
        {
            get => _table.ParentFolderIndex[_index];
            set => _table.ParentFolderIndex[_index] = value;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        public DateTime LastModifiedUtc
        {
            get => _table.LastModifiedUtc[_index];
            set => _table.LastModifiedUtc[_index] = value;
        }

        public DateTime CreatedUtc
        {
            get => _table.CreatedUtc[_index];
            set => _table.CreatedUtc[_index] = value;
        }

        public System.IO.FileAttributes Attributes
        {
            get => (System.IO.FileAttributes)_table.Attributes[_index];
            set => _table.Attributes[_index] = (int)value;
        }

        public long Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        #region IEquatable<File>
        public bool Equals(File other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.ParentFolderIndex, other.ParentFolderIndex)) { return false; }
            if (!object.Equals(this.Name, other.Name)) { return false; }
            if (!object.Equals(this.LastModifiedUtc, other.LastModifiedUtc)) { return false; }
            if (!object.Equals(this.CreatedUtc, other.CreatedUtc)) { return false; }
            if (!object.Equals(this.Attributes, other.Attributes)) { return false; }
            if (!object.Equals(this.Length, other.Length)) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (ParentFolderIndex != default(int))
                {
                    result = (result * 31) + ParentFolderIndex.GetHashCode();
                }

                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (LastModifiedUtc != default(DateTime))
                {
                    result = (result * 31) + LastModifiedUtc.GetHashCode();
                }

                if (CreatedUtc != default(DateTime))
                {
                    result = (result * 31) + CreatedUtc.GetHashCode();
                }

                if (Attributes != default(System.IO.FileAttributes))
                {
                    result = (result * 31) + Attributes.GetHashCode();
                }

                if (Length != default(long))
                {
                    result = (result * 31) + Length.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as File);
        }

        public static bool operator ==(File left, File right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(File left, File right)
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
            _table = (FileTable)table;
            _index = index;
        }

        public void CopyFrom(File other)
        {
            ParentFolderIndex = other.ParentFolderIndex;
            Name = other.Name;
            LastModifiedUtc = other.LastModifiedUtc;
            CreatedUtc = other.CreatedUtc;
            Attributes = other.Attributes;
            Length = other.Length;
        }

        internal static File Copy(FileSystemDatabase database, File other)
        {
            return (other == null ? null : new File(database, other));
        }
        #endregion
    }
}
