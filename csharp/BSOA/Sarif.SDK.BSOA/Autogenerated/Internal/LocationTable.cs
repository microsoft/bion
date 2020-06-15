// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Location'
    /// </summary>
    internal partial class LocationTable : Table<Location>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Id;
        internal RefColumn PhysicalLocation;
        internal RefListColumn LogicalLocations;
        internal RefColumn Message;
        internal RefListColumn Annotations;
        internal RefListColumn Relationships;
        internal IColumn<IDictionary<string, string>> Properties;

        internal LocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<int>(-1));
            PhysicalLocation = AddColumn(nameof(PhysicalLocation), new RefColumn(nameof(SarifLogDatabase.PhysicalLocation)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.LogicalLocation)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Annotations = AddColumn(nameof(Annotations), new RefListColumn(nameof(SarifLogDatabase.Region)));
            Relationships = AddColumn(nameof(Relationships), new RefListColumn(nameof(SarifLogDatabase.LocationRelationship)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Location Get(int index)
        {
            return (index == -1 ? null : new Location(this, index));
        }
    }
}
