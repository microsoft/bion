// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'TinyLog'
    /// </summary>
    internal partial class TinyLogTable : Table<TinyLog>
    {
        internal TinyDatabase Database;

        internal RefListColumn Regions;

        internal TinyLogTable(TinyDatabase database) : base()
        {
            Database = database;

            Regions = AddColumn(nameof(Regions), new RefListColumn(nameof(TinyDatabase.Region)));
        }

        public override TinyLog Get(int index)
        {
            return (index == -1 ? null : new TinyLog(this, index));
        }
    }
}
