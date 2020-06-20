// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Conversion'
    /// </summary>
    internal partial class ConversionTable : Table<Conversion>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Tool;
        internal RefColumn Invocation;
        internal RefListColumn AnalysisToolLogFiles;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ConversionTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tool = AddColumn(nameof(Tool), new RefColumn(nameof(SarifLogDatabase.Tool)));
            Invocation = AddColumn(nameof(Invocation), new RefColumn(nameof(SarifLogDatabase.Invocation)));
            AnalysisToolLogFiles = AddColumn(nameof(AnalysisToolLogFiles), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Conversion Get(int index)
        {
            return (index == -1 ? null : new Conversion(this, index));
        }
    }
}
