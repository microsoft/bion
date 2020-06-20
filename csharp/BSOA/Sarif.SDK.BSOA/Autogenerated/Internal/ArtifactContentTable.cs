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

        internal IColumn<String> Text;
        internal IColumn<String> Binary;
        internal RefColumn Rendered;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ArtifactContentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), database.BuildColumn<String>(nameof(ArtifactContent), nameof(Text), default));
            Binary = AddColumn(nameof(Binary), database.BuildColumn<String>(nameof(ArtifactContent), nameof(Binary), default));
            Rendered = AddColumn(nameof(Rendered), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ArtifactContent), nameof(Properties), default));
        }

        public override ArtifactContent Get(int index)
        {
            return (index == -1 ? null : new ArtifactContent(this, index));
        }
    }
}
