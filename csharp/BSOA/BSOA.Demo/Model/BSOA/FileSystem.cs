// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'FileSystem'
    /// </summary>
    public partial class FileSystem : IRow<FileSystem>, IEquatable<FileSystem>
    {
        private FileSystemTable _table;
        private int _index;

        internal FileSystemDatabase Database => _table.Database;
        public IDatabase DB => _table.Database;

        public FileSystem() : this(new FileSystemDatabase().FileSystem)
        { }

        public FileSystem(FileSystem other) : this(new FileSystemDatabase().FileSystem)
        {
            CopyFrom(other);
        }

        internal FileSystem(FileSystemTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal FileSystem(FileSystemTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

        public IList<Folder> Folders
        {
            get => TypedList<Folder>.Get(_table.Database.Folder, _table.Folders, _index);
            set => TypedList<Folder>.Set(_table.Database.Folder, _table.Folders, _index, value);
        }

        public IList<File> Files
        {
            get => TypedList<File>.Get(_table.Database.File, _table.Files, _index);
            set => TypedList<File>.Set(_table.Database.File, _table.Files, _index, value);
        }

        #region IEquatable<FileSystem>
        public bool Equals(FileSystem other)
        {
            if (other == null) { return false; }

            if (!object.Equals(this.Folders, other.Folders)) { return false; }
            if (!object.Equals(this.Files, other.Files)) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Folders != default(IList<Folder>))
                {
                    result = (result * 31) + Folders.GetHashCode();
                }

                if (Files != default(IList<File>))
                {
                    result = (result * 31) + Files.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileSystem);
        }

        public static bool operator ==(FileSystem left, FileSystem right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(FileSystem left, FileSystem right)
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
            _table = (FileSystemTable)table;
            _index = index;
        }

        public void CopyFrom(FileSystem other)
        {
            Folders = other.Folders?.Select((item) => Folder.Copy(_table.Database, item)).ToList();
            Files = other.Files?.Select((item) => File.Copy(_table.Database, item)).ToList();
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

        public static FileSystem ReadBsoa(System.IO.Stream stream)
        {
            using (BinaryTreeReader reader = new BinaryTreeReader(stream))
            {
                FileSystem result = new FileSystem();
                result.DB.Read(reader);
                return result;
            }
        }

        public static FileSystem ReadBsoa(string filePath)
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
                FileSystem result = new FileSystem();
                result.DB.Read(reader);
                return reader.Tree;
            }
        }
        #endregion
    }
}
