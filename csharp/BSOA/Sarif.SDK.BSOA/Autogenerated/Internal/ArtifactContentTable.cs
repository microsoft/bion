// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactContent'
    /// </summary>
    internal partial class ArtifactContentTable : Table<ArtifactContent>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Text;
        internal IColumn<string> Binary;
        internal RefColumn Rendered;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ArtifactContentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(default));
            Binary = AddColumn(nameof(Binary), ColumnFactory.Build<string>(default));
            Rendered = AddColumn(nameof(Rendered), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ArtifactContent Get(int index)
        {
            return (index == -1 ? null : new ArtifactContent(this, index));
        }
    }
}
