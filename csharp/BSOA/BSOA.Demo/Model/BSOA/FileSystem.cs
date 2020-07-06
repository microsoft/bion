// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.IO;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'FileSystem'
    /// </summary>
    public partial class FileSystem : IRow
    {
        private FileSystemTable _table;
        private int _index;

        internal FileSystemDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;

        public FileSystem() : this(new FileSystemDatabase().FileSystem)
        { }

        internal FileSystem(FileSystemTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal FileSystem(FileSystemTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public IList<Folder> Folders
        {
            get => _table.Database.Folder.List(_table.Folders[_index]);
            set => _table.Database.Folder.List(_table.Folders[_index]).SetTo(value);
        }

        public IList<File> Files
        {
            get => _table.Database.File.List(_table.Files[_index]);
            set => _table.Database.File.List(_table.Files[_index]).SetTo(value);
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

        void IRow.Next()
        {
            _index++;
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
