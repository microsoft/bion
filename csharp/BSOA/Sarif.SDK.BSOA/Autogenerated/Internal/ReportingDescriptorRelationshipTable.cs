// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ReportingDescriptorRelationship'
    /// </summary>
    internal partial class ReportingDescriptorRelationshipTable : Table<ReportingDescriptorRelationship>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Target;
        internal IColumn<IList<String>> Kinds;
        internal RefColumn Description;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ReportingDescriptorRelationshipTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Target = AddColumn(nameof(Target), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Kinds = AddColumn(nameof(Kinds), database.BuildColumn<IList<String>>(nameof(ReportingDescriptorRelationship), nameof(Kinds), default));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ReportingDescriptorRelationship), nameof(Properties), default));
        }

        public override ReportingDescriptorRelationship Get(int index)
        {
            return (index == -1 ? null : new ReportingDescriptorRelationship(this, index));
        }
    }
}
