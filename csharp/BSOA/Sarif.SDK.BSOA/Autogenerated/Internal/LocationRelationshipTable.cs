// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'LocationRelationship'
    /// </summary>
    internal partial class LocationRelationshipTable : Table<LocationRelationship>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Target;
        internal IColumn<IList<String>> Kinds;
        internal RefColumn Description;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal LocationRelationshipTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Target = AddColumn(nameof(Target), database.BuildColumn<int>(nameof(LocationRelationship), nameof(Target), default));
            Kinds = AddColumn(nameof(Kinds), database.BuildColumn<IList<String>>(nameof(LocationRelationship), nameof(Kinds), default));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(LocationRelationship), nameof(Properties), default));
        }

        public override LocationRelationship Get(int index)
        {
            return (index == -1 ? null : new LocationRelationship(this, index));
        }
    }
}
