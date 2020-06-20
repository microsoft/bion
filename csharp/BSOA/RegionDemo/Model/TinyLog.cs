// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using BSOA.IO;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Root Entity for 'TinyLog'
    /// </summary>
    public partial class TinyLog : IRow
    {
        private TinyLogTable _table;
        private int _index;

        internal TinyDatabase Database => _table.Database;
        public ITreeSerializable DB => _table.Database;

        public TinyLog() : this(new TinyDatabase().TinyLog)
        { }

        internal TinyLog(TinyLogTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal TinyLog(TinyLogTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public IList<Region> Regions
        {
            get => _table.Database.Region.List(_table.Regions[_index]);
            set => _table.Database.Region.List(_table.Regions[_index]).SetTo(value);
        }


        #region IEquatable<TinyLog>
        public bool Equals(TinyLog other)
        {
            if (other == null) { return false; }

            if (this.Regions != other.Regions) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Regions != default(IList<Region>))
                {
                    result = (result * 31) + Regions.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TinyLog);
        }

        public static bool operator ==(TinyLog left, TinyLog right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(TinyLog left, TinyLog right)
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

        public static TinyLog ReadBsoa(Stream stream)
        {
            using (BinaryTreeReader reader = new BinaryTreeReader(stream))
            {
                TinyLog result = new TinyLog();
                result.DB.Read(reader);
                return result;
            }
        }

        public static TinyLog ReadBsoa(string filePath)
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
                TinyLog result = new TinyLog();
                result.DB.Read(reader);
                return reader.Tree;
            }
        }
        #endregion
    }
}
