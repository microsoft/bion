// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExternalPropertyFileReference'
    /// </summary>
    internal partial class ExternalPropertyFileReferenceTable : Table<ExternalPropertyFileReference>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Location;
        internal IColumn<string> Guid;
        internal IColumn<int> ItemCount;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ExternalPropertyFileReferenceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>());
            ItemCount = AddColumn(nameof(ItemCount), ColumnFactory.Build<int>(-1));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ExternalPropertyFileReference Get(int index)
        {
            return (index == -1 ? null : new ExternalPropertyFileReference(this, index));
        }
    }
}
