// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Location'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Location : PropertyBagHolder, ISarifNode, IRow
    {
        private LocationTable _table;
        private int _index;

        internal Location(LocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Location(LocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Location(SarifLogBsoa database) : this(database.Location)
        { }

        public Location() : this(SarifLogBsoa.Current)
        { }

        public Location(
			int id,
			PhysicalLocation physicalLocation,
			IList<LogicalLocation> logicalLocations,
			Message message,
			IList<Region> annotations
        ) : this(SarifLogBsoa.Current)
        {
			Id = id;
			PhysicalLocation = physicalLocation;
			LogicalLocations = logicalLocations;
			Message = message;
			Annotations = annotations;
        }

        public Location(Location other)
        {
			Id = other.Id;
			PhysicalLocation = other.PhysicalLocation;
			LogicalLocations = other.LogicalLocations;
			Message = other.Message;
			Annotations = other.Annotations;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public PhysicalLocation PhysicalLocation
        {
            get => _table.Database.PhysicalLocation.Get(_table.PhysicalLocation[_index]);
            set => _table.PhysicalLocation[_index] = _table.Database.PhysicalLocation.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<LogicalLocation> LogicalLocations
        {
            get => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]);
            set => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]).SetTo(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Region> Annotations
        {
            get => _table.Database.Region.List(_table.Annotations[_index]);
            set => _table.Database.Region.List(_table.Annotations[_index]).SetTo(value);
        }

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

        //public static IEqualityComparer<Location> ValueComparer => LocationEqualityComparer.Instance;
        //public bool ValueEquals(Location other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
