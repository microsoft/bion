// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Region'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Region : PropertyBagHolder, ISarifNode, IRow
    {
        private RegionTable _table;
        private int _index;

        public Region() : this(SarifLogDatabase.Current.Region)
        { }

        public Region(SarifLog root) : this(root.Database.Region)
        { }

        internal Region(RegionTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
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
            int charOffset,
            int charLength,
            int byteOffset,
            int byteLength,
            ArtifactContent snippet,
            Message message,
            string sourceLanguage,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Region)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
            CharOffset = charOffset;
            CharLength = charLength;
            ByteOffset = byteOffset;
            ByteLength = byteLength;
            Snippet = snippet;
            Message = message;
            SourceLanguage = sourceLanguage;
            Properties = properties;
        }

        public Region(Region other) 
            : this(SarifLogDatabase.Current.Region)
        {
            StartLine = other.StartLine;
            StartColumn = other.StartColumn;
            EndLine = other.EndLine;
            EndColumn = other.EndColumn;
            CharOffset = other.CharOffset;
            CharLength = other.CharLength;
            ByteOffset = other.ByteOffset;
            ByteLength = other.ByteLength;
            Snippet = other.Snippet;
            Message = other.Message;
            SourceLanguage = other.SourceLanguage;
            Properties = other.Properties;
        }

        partial void Init();

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

        public int CharOffset
        {
            get => _table.CharOffset[_index];
            set => _table.CharOffset[_index] = value;
        }

        public int CharLength
        {
            get => _table.CharLength[_index];
            set => _table.CharLength[_index] = value;
        }

        public int ByteOffset
        {
            get => _table.ByteOffset[_index];
            set => _table.ByteOffset[_index] = value;
        }

        public int ByteLength
        {
            get => _table.ByteLength[_index];
            set => _table.ByteLength[_index] = value;
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

        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Region>
        public bool Equals(Region other)
        {
            if (other == null) { return false; }

            if (this.StartLine != other.StartLine) { return false; }
            if (this.StartColumn != other.StartColumn) { return false; }
            if (this.EndLine != other.EndLine) { return false; }
            if (this.EndColumn != other.EndColumn) { return false; }
            if (this.CharOffset != other.CharOffset) { return false; }
            if (this.CharLength != other.CharLength) { return false; }
            if (this.ByteOffset != other.ByteOffset) { return false; }
            if (this.ByteLength != other.ByteLength) { return false; }
            if (this.Snippet != other.Snippet) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.SourceLanguage != other.SourceLanguage) { return false; }
            if (this.Properties != other.Properties) { return false; }

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

                if (CharOffset != default(int))
                {
                    result = (result * 31) + CharOffset.GetHashCode();
                }

                if (CharLength != default(int))
                {
                    result = (result * 31) + CharLength.GetHashCode();
                }

                if (ByteOffset != default(int))
                {
                    result = (result * 31) + ByteOffset.GetHashCode();
                }

                if (ByteLength != default(int))
                {
                    result = (result * 31) + ByteLength.GetHashCode();
                }

                if (Snippet != default(ArtifactContent))
                {
                    result = (result * 31) + Snippet.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (SourceLanguage != default(string))
                {
                    result = (result * 31) + SourceLanguage.GetHashCode();
                }

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Region;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Region DeepClone()
        {
            return (Region)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Region(this);
        }
        #endregion

        public static IEqualityComparer<Region> ValueComparer => EqualityComparer<Region>.Default;
        public bool ValueEquals(Region other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
