// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactContent'
    /// </summary>
    internal partial class ArtifactContentTable : Table<ArtifactContent>
    {
        internal TinyDatabase Database;

        internal IColumn<string> Text;
        internal IColumn<string> Binary;

        internal ArtifactContentTable(TinyDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(null));
            Binary = AddColumn(nameof(Binary), ColumnFactory.Build<string>(null));
        }

        public override ArtifactContent Get(int index)
        {
            return (index == -1 ? null : new ArtifactContent(this, index));
        }
    }
}
