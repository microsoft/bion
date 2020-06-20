// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'ArtifactContent'
    /// </summary>
    public partial class ArtifactContent : IRow, IEquatable<ArtifactContent>
    {
        private ArtifactContentTable _table;
        private int _index;

        public ArtifactContent() : this(TinyDatabase.Current.ArtifactContent)
        { }

        public ArtifactContent(TinyLog root) : this(root.Database.ArtifactContent)
        { }

        internal ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }
        
        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactContent(
            string text,
            string binary
        ) 
            : this(TinyDatabase.Current.ArtifactContent)
        {
            Text = text;
            Binary = binary;
        }

        public ArtifactContent(ArtifactContent other) 
            : this(TinyDatabase.Current.ArtifactContent)
        {
            Text = other.Text;
            Binary = other.Binary;
        }

        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        public string Binary
        {
            get => _table.Binary[_index];
            set => _table.Binary[_index] = value;
        }

        #region IEquatable<ArtifactContent>
        public bool Equals(ArtifactContent other)
        {
            if (other == null) { return false; }

            if (this.Text != other.Text) { return false; }
            if (this.Binary != other.Binary) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Text != default(string))
                {
                    result = (result * 31) + Text.GetHashCode();
                }

                if (Binary != default(string))
                {
                    result = (result * 31) + Binary.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArtifactContent);
        }

        public static bool operator ==(ArtifactContent left, ArtifactContent right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ArtifactContent left, ArtifactContent right)
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
    }
}
