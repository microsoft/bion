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
    ///  GENERATED: BSOA Entity for 'Location'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Location : PropertyBagHolder, ISarifNode, IRow
    {
        private LocationTable _table;
        private int _index;

        public Location() : this(SarifLogDatabase.Current.Location)
        { }

        public Location(SarifLog root) : this(root.Database.Location)
        { }

        internal Location(LocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Location(LocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Location(
            int id,
            PhysicalLocation physicalLocation,
            IList<LogicalLocation> logicalLocations,
            Message message,
            IList<Region> annotations,
            IList<LocationRelationship> relationships,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Location)
        {
            Id = id;
            PhysicalLocation = physicalLocation;
            LogicalLocations = logicalLocations;
            Message = message;
            Annotations = annotations;
            Relationships = relationships;
            Properties = properties;
        }

        public Location(Location other) 
            : this(SarifLogDatabase.Current.Location)
        {
            Id = other.Id;
            PhysicalLocation = other.PhysicalLocation;
            LogicalLocations = other.LogicalLocations;
            Message = other.Message;
            Annotations = other.Annotations;
            Relationships = other.Relationships;
            Properties = other.Properties;
        }

        public int Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        public PhysicalLocation PhysicalLocation
        {
            get => _table.Database.PhysicalLocation.Get(_table.PhysicalLocation[_index]);
            set => _table.PhysicalLocation[_index] = _table.Database.PhysicalLocation.LocalIndex(value);
        }

        public IList<LogicalLocation> LogicalLocations
        {
            get => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]);
            set => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]).SetTo(value);
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IList<Region> Annotations
        {
            get => _table.Database.Region.List(_table.Annotations[_index]);
            set => _table.Database.Region.List(_table.Annotations[_index]).SetTo(value);
        }

        public IList<LocationRelationship> Relationships
        {
            get => _table.Database.LocationRelationship.List(_table.Relationships[_index]);
            set => _table.Database.LocationRelationship.List(_table.Relationships[_index]).SetTo(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Location>
        public bool Equals(Location other)
        {
            if (other == null) { return false; }

            if (this.Id != other.Id) { return false; }
            if (this.PhysicalLocation != other.PhysicalLocation) { return false; }
            if (this.LogicalLocations != other.LogicalLocations) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.Annotations != other.Annotations) { return false; }
            if (this.Relationships != other.Relationships) { return false; }
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
                if (Id != default(int))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (PhysicalLocation != default(PhysicalLocation))
                {
                    result = (result * 31) + PhysicalLocation.GetHashCode();
                }

                if (LogicalLocations != default(IList<LogicalLocation>))
                {
                    result = (result * 31) + LogicalLocations.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Annotations != default(IList<Region>))
                {
                    result = (result * 31) + Annotations.GetHashCode();
                }

                if (Relationships != default(IList<LocationRelationship>))
                {
                    result = (result * 31) + Relationships.GetHashCode();
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
            return Equals(obj as Location);
        }

        public static bool operator ==(Location left, Location right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
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
            _table = (LocationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Location;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Location DeepClone()
        {
            return (Location)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Location(this);
        }
        #endregion

        public static IEqualityComparer<Location> ValueComparer => EqualityComparer<Location>.Default;
        public bool ValueEquals(Location other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
