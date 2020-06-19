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
        internal IColumn<string> FirstDetectionRunGuid;
        internal IColumn<string> LastDetectionRunGuid;
        internal IColumn<int> InvocationIndex;
        internal RefListColumn ConversionSources;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ResultProvenanceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            FirstDetectionTimeUtc = AddColumn(nameof(FirstDetectionTimeUtc), ColumnFactory.Build<DateTime>(default));
            LastDetectionTimeUtc = AddColumn(nameof(LastDetectionTimeUtc), ColumnFactory.Build<DateTime>(default));
            FirstDetectionRunGuid = AddColumn(nameof(FirstDetectionRunGuid), ColumnFactory.Build<string>(default));
            LastDetectionRunGuid = AddColumn(nameof(LastDetectionRunGuid), ColumnFactory.Build<string>(default));
            InvocationIndex = AddColumn(nameof(InvocationIndex), ColumnFactory.Build<int>(-1));
            ConversionSources = AddColumn(nameof(ConversionSources), new RefListColumn(nameof(SarifLogDatabase.PhysicalLocation)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ResultProvenance Get(int index)
        {
            return (index == -1 ? null : new ResultProvenance(this, index));
        }
    }
}
