// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Test.Model.Log
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Run'
    /// </summary>
    internal partial class RunTable : Table<Run>
    {
        internal RunDatabase Database;

        internal RefListColumn Results;
        internal RefListColumn Rules;

        internal RunTable(RunDatabase database) : base()
        {
            Database = database;

            Results = AddColumn(nameof(Results), new RefListColumn(nameof(RunDatabase.Result)));
            Rules = AddColumn(nameof(Rules), new RefListColumn(nameof(RunDatabase.Rule)));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
