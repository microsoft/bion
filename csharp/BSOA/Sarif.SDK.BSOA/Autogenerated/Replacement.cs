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
    ///  GENERATED: BSOA Entity for 'Replacement'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Replacement : PropertyBagHolder, ISarifNode, IRow
    {
        private ReplacementTable _table;
        private int _index;

        public Replacement() : this(SarifLogDatabase.Current.Replacement)
        { }

        public Replacement(SarifLog root) : this(root.Database.Replacement)
        { }

        internal Replacement(ReplacementTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Replacement(ReplacementTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Replacement(
            Region deletedRegion,
            ArtifactContent insertedContent,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Replacement)
        {
            DeletedRegion = deletedRegion;
            InsertedContent = insertedContent;
            Properties = properties;
        }

        public Replacement(Replacement other) 
            : this(SarifLogDatabase.Current.Replacement)
        {
            DeletedRegion = other.DeletedRegion;
            InsertedContent = other.InsertedContent;
            Properties = other.Properties;
        }

        public Region DeletedRegion
        {
            get => _table.Database.Region.Get(_table.DeletedRegion[_index]);
            set => _table.DeletedRegion[_index] = _table.Database.Region.LocalIndex(value);
        }

        public ArtifactContent InsertedContent
        {
            get => _table.Database.ArtifactContent.Get(_table.InsertedContent[_index]);
            set => _table.InsertedContent[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Replacement>
        public bool Equals(Replacement other)
        {
            if (other == null) { return false; }

            if (this.DeletedRegion != other.DeletedRegion) { return false; }
            if (this.InsertedContent != other.InsertedContent) { return false; }
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
                if (DeletedRegion != default(Region))
                {
                    result = (result * 31) + DeletedRegion.GetHashCode();
                }

                if (InsertedContent != default(ArtifactContent))
                {
                    result = (result * 31) + InsertedContent.GetHashCode();
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
            return Equals(obj as Replacement);
        }

        public static bool operator ==(Replacement left, Replacement right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Replacement left, Replacement right)
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
            _table = (ReplacementTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Replacement;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Replacement DeepClone()
        {
            return (Replacement)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Replacement(this);
        }
        #endregion

        public static IEqualityComparer<Replacement> ValueComparer => EqualityComparer<Replacement>.Default;
        public bool ValueEquals(Replacement other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
