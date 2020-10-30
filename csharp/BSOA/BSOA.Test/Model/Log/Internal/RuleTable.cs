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
    ///  BSOA GENERATED Table for 'Rule'
    /// </summary>
    internal partial class RuleTable : Table<Rule>
    {
        internal RunDatabase Database;

        internal IColumn<string> Id;
        internal IColumn<String> Guid;
        internal IColumn<Uri> HelpUri;
        internal IColumn<NumberList<int>> RelatedRules;

        internal RuleTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (RunDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            Id = GetOrBuild(nameof(Id), () => Database.BuildColumn<string>(nameof(Rule), nameof(Id), default));
            Guid = GetOrBuild(nameof(Guid), () => Database.BuildColumn<String>(nameof(Rule), nameof(Guid), default));
            HelpUri = GetOrBuild(nameof(HelpUri), () => Database.BuildColumn<Uri>(nameof(Rule), nameof(HelpUri), default));
            RelatedRules = GetOrBuild(nameof(RelatedRules), () => new RefListColumn(nameof(RunDatabase.Rule)));
        }

        public override Rule Get(int index)
        {
            return (index == -1 ? null : new Rule(this, index));
        }
    }
}
