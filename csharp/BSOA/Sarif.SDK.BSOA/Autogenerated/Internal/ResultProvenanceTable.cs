// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ResultProvenance'
    /// </summary>
    internal partial class ResultProvenanceTable : Table<ResultProvenance>
    {
        internal SarifLogDatabase Database;

        internal IColumn<DateTime> FirstDetectionTimeUtc;
        internal IColumn<DateTime> LastDetectionTimeUtc;
        internal IColumn<String> FirstDetectionRunGuid;
        internal IColumn<String> LastDetectionRunGuid;
        internal IColumn<int> InvocationIndex;
        internal RefListColumn ConversionSources;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ResultProvenanceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            FirstDetectionTimeUtc = AddColumn(nameof(FirstDetectionTimeUtc), database.BuildColumn<DateTime>(nameof(ResultProvenance), nameof(FirstDetectionTimeUtc), default));
            LastDetectionTimeUtc = AddColumn(nameof(LastDetectionTimeUtc), database.BuildColumn<DateTime>(nameof(ResultProvenance), nameof(LastDetectionTimeUtc), default));
            FirstDetectionRunGuid = AddColumn(nameof(FirstDetectionRunGuid), database.BuildColumn<String>(nameof(ResultProvenance), nameof(FirstDetectionRunGuid), default));
            LastDetectionRunGuid = AddColumn(nameof(LastDetectionRunGuid), database.BuildColumn<String>(nameof(ResultProvenance), nameof(LastDetectionRunGuid), default));
            InvocationIndex = AddColumn(nameof(InvocationIndex), database.BuildColumn<int>(nameof(ResultProvenance), nameof(InvocationIndex), -1));
            ConversionSources = AddColumn(nameof(ConversionSources), new RefListColumn(nameof(SarifLogDatabase.PhysicalLocation)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ResultProvenance), nameof(Properties), default));
        }

        public override ResultProvenance Get(int index)
        {
            return (index == -1 ? null : new ResultProvenance(this, index));
        }
    }
}
