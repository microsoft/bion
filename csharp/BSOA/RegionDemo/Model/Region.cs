// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Region'
    /// </summary>
    public partial class Region : IRow, IEquatable<Region>
    {
        private RegionTable _table;
        private int _index;

        public Region() : this(TinyDatabase.Current.Region)
        { }

        public Region(TinyLog root) : this(root.Database.Region)
        { }

        internal Region(RegionTable table) : this(table, table.Count)
        {
            table.Add();
        }
        
        internal Region(RegionTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Region(
            int startLine,
            int startColumn,
            int endLine,
            int endColumn,
            ArtifactContent snippet,
            Message message
        ) 
            : this(TinyDatabase.Current.Region)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
            Snippet = snippet;
            Message = message;
        }

        public Region(Region other) 
            : this(TinyDatabase.Current.Region)
        {
            StartLine = other.StartLine;
            StartColumn = other.StartColumn;
            EndLine = other.EndLine;
            EndColumn = other.EndColumn;
            Snippet = other.Snippet;
            Message = other.Message;
        }

        public int StartLine
        {
            get => _table.StartLine[_index];
            set => _table.StartLine[_index] = value;
        }

        public int StartColumn
        {
            get => _table.StartColumn[_index];
            set => _table.StartColumn[_index] = value;
        }

        public int EndLine
        {
            get => _table.EndLine[_index];
            set => _table.EndLine[_index] = value;
        }

        public int EndColumn
        {
            get => _table.EndColumn[_index];
            set => _table.EndColumn[_index] = value;
        }

        public ArtifactContent Snippet
        {
            get => _table.Database.ArtifactContent.Get(_table.Snippet[_index]);
            set => _table.Snippet[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        #region IEquatable<Region>
        public bool Equals(Region other)
        {
            if (other == null) { return false; }

            if (this.StartLine != other.StartLine) { return false; }
            if (this.StartColumn != other.StartColumn) { return false; }
            if (this.EndLine != other.EndLine) { return false; }
            if (this.EndColumn != other.EndColumn) { return false; }
            if (this.Snippet != other.Snippet) { return false; }
            if (this.Message != other.Message) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (StartLine != default(int))
                {
                    result = (result * 31) + StartLine.GetHashCode();
                }

                if (StartColumn != default(int))
                {
                    result = (result * 31) + StartColumn.GetHashCode();
                }

                if (EndLine != default(int))
                {
                    result = (result * 31) + EndLine.GetHashCode();
                }

                if (EndColumn != default(int))
                {
                    result = (result * 31) + EndColumn.GetHashCode();
                }

                if (Snippet != default(ArtifactContent))
                {
                    result = (result * 31) + Snippet.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Region);
        }

        public static bool operator ==(Region left, Region right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Region left, Region right)
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
            _table = (RegionTable)table;
            _index = index;
        }
        #endregion
    }
}
