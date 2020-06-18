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
    ///  GENERATED: BSOA Entity for 'VersionControlDetails'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class VersionControlDetails : PropertyBagHolder, ISarifNode, IRow
    {
        private VersionControlDetailsTable _table;
        private int _index;

        public VersionControlDetails() : this(SarifLogDatabase.Current.VersionControlDetails)
        { }

        public VersionControlDetails(SarifLog root) : this(root.Database.VersionControlDetails)
        { }

        internal VersionControlDetails(VersionControlDetailsTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal VersionControlDetails(VersionControlDetailsTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public VersionControlDetails(
            Uri repositoryUri,
            string revisionId,
            string branch,
            string revisionTag,
            DateTime asOfTimeUtc,
            ArtifactLocation mappedTo,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.VersionControlDetails)
        {
            RepositoryUri = repositoryUri;
            RevisionId = revisionId;
            Branch = branch;
            RevisionTag = revisionTag;
            AsOfTimeUtc = asOfTimeUtc;
            MappedTo = mappedTo;
            Properties = properties;
        }

        public VersionControlDetails(VersionControlDetails other) 
            : this(SarifLogDatabase.Current.VersionControlDetails)
        {
            RepositoryUri = other.RepositoryUri;
            RevisionId = other.RevisionId;
            Branch = other.Branch;
            RevisionTag = other.RevisionTag;
            AsOfTimeUtc = other.AsOfTimeUtc;
            MappedTo = other.MappedTo;
            Properties = other.Properties;
        }

        public Uri RepositoryUri
        {
            get => _table.RepositoryUri[_index];
            set => _table.RepositoryUri[_index] = value;
        }

        public string RevisionId
        {
            get => _table.RevisionId[_index];
            set => _table.RevisionId[_index] = value;
        }

        public string Branch
        {
            get => _table.Branch[_index];
            set => _table.Branch[_index] = value;
        }

        public string RevisionTag
        {
            get => _table.RevisionTag[_index];
            set => _table.RevisionTag[_index] = value;
        }

        public DateTime AsOfTimeUtc
        {
            get => _table.AsOfTimeUtc[_index];
            set => _table.AsOfTimeUtc[_index] = value;
        }

        public ArtifactLocation MappedTo
        {
            get => _table.Database.ArtifactLocation.Get(_table.MappedTo[_index]);
            set => _table.MappedTo[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<VersionControlDetails>
        public bool Equals(VersionControlDetails other)
        {
            if (other == null) { return false; }

            if (this.RepositoryUri != other.RepositoryUri) { return false; }
            if (this.RevisionId != other.RevisionId) { return false; }
            if (this.Branch != other.Branch) { return false; }
            if (this.RevisionTag != other.RevisionTag) { return false; }
            if (this.AsOfTimeUtc != other.AsOfTimeUtc) { return false; }
            if (this.MappedTo != other.MappedTo) { return false; }
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
                if (RepositoryUri != default(Uri))
                {
                    result = (result * 31) + RepositoryUri.GetHashCode();
                }

                if (RevisionId != default(string))
                {
                    result = (result * 31) + RevisionId.GetHashCode();
                }

                if (Branch != default(string))
                {
                    result = (result * 31) + Branch.GetHashCode();
                }

                if (RevisionTag != default(string))
                {
                    result = (result * 31) + RevisionTag.GetHashCode();
                }

                if (AsOfTimeUtc != default(DateTime))
                {
                    result = (result * 31) + AsOfTimeUtc.GetHashCode();
                }

                if (MappedTo != default(ArtifactLocation))
                {
                    result = (result * 31) + MappedTo.GetHashCode();
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
            return Equals(obj as VersionControlDetails);
        }

        public static bool operator ==(VersionControlDetails left, VersionControlDetails right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(VersionControlDetails left, VersionControlDetails right)
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
            _table = (VersionControlDetailsTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.VersionControlDetails;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public VersionControlDetails DeepClone()
        {
            return (VersionControlDetails)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new VersionControlDetails(this);
        }
        #endregion

        public static IEqualityComparer<VersionControlDetails> ValueComparer => EqualityComparer<VersionControlDetails>.Default;
        public bool ValueEquals(VersionControlDetails other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
