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
    ///  GENERATED: BSOA Entity for 'Attachment'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Attachment : PropertyBagHolder, ISarifNode, IRow
    {
        private AttachmentTable _table;
        private int _index;

        public Attachment() : this(SarifLogDatabase.Current.Attachment)
        { }

        public Attachment(SarifLog root) : this(root.Database.Attachment)
        { }

        internal Attachment(AttachmentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Attachment(AttachmentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Attachment(
            Message description,
            ArtifactLocation artifactLocation,
            IList<Region> regions,
            IList<Rectangle> rectangles,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Attachment)
        {
            Description = description;
            ArtifactLocation = artifactLocation;
            Regions = regions;
            Rectangles = rectangles;
            Properties = properties;
        }

        public Attachment(Attachment other) 
            : this(SarifLogDatabase.Current.Attachment)
        {
            Description = other.Description;
            ArtifactLocation = other.ArtifactLocation;
            Regions = other.Regions;
            Rectangles = other.Rectangles;
            Properties = other.Properties;
        }

        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        public ArtifactLocation ArtifactLocation
        {
            get => _table.Database.ArtifactLocation.Get(_table.ArtifactLocation[_index]);
            set => _table.ArtifactLocation[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public IList<Region> Regions
        {
            get => _table.Database.Region.List(_table.Regions[_index]);
            set => _table.Database.Region.List(_table.Regions[_index]).SetTo(value);
        }

        public IList<Rectangle> Rectangles
        {
            get => _table.Database.Rectangle.List(_table.Rectangles[_index]);
            set => _table.Database.Rectangle.List(_table.Rectangles[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Attachment>
        public bool Equals(Attachment other)
        {
            if (other == null) { return false; }

            if (this.Description != other.Description) { return false; }
            if (this.ArtifactLocation != other.ArtifactLocation) { return false; }
            if (this.Regions != other.Regions) { return false; }
            if (this.Rectangles != other.Rectangles) { return false; }
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
                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
                }

                if (ArtifactLocation != default(ArtifactLocation))
                {
                    result = (result * 31) + ArtifactLocation.GetHashCode();
                }

                if (Regions != default(IList<Region>))
                {
                    result = (result * 31) + Regions.GetHashCode();
                }

                if (Rectangles != default(IList<Rectangle>))
                {
                    result = (result * 31) + Rectangles.GetHashCode();
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
            return Equals(obj as Attachment);
        }

        public static bool operator ==(Attachment left, Attachment right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Attachment left, Attachment right)
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
            _table = (AttachmentTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Attachment;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Attachment DeepClone()
        {
            return (Attachment)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Attachment(this);
        }
        #endregion

        public static IEqualityComparer<Attachment> ValueComparer => EqualityComparer<Attachment>.Default;
        public bool ValueEquals(Attachment other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
