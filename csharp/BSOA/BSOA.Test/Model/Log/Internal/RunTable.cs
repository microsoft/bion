// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Test.Model.Log
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Run'
    /// </summary>
    internal partial class RunTable : Table<Run>
    {
        internal RunDatabase Database;

        internal IColumn<NumberList<int>> Results;
        internal IColumn<NumberList<int>> Rules;

        public RunTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (RunDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            base.GetOrBuildColumns();

            Results = GetOrBuild(nameof(Results), () => (IColumn<NumberList<int>>)new RefListColumn(nameof(RunDatabase.Result)));
            Rules = GetOrBuild(nameof(Rules), () => (IColumn<NumberList<int>>)new RefListColumn(nameof(RunDatabase.Rule)));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
