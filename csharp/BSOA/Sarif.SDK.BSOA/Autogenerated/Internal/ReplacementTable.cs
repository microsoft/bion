// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Replacement'
    /// </summary>
    internal partial class ReplacementTable : Table<Replacement>
    {
        internal SarifLogDatabase Database;

        internal RefColumn DeletedRegion;
        internal RefColumn InsertedContent;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ReplacementTable(SarifLogDatabase database) : base()
        {
            Database = database;

            DeletedRegion = AddColumn(nameof(DeletedRegion), new RefColumn(nameof(SarifLogDatabase.Region)));
            InsertedContent = AddColumn(nameof(InsertedContent), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override Replacement Get(int index)
        {
            return (index == -1 ? null : new Replacement(this, index));
        }
    }
}
