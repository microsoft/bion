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
    ///  BSOA GENERATED Table for 'Result'
    /// </summary>
    internal partial class ResultTable : Table<Result>
    {
        internal RunDatabase Database;

        internal IColumn<string> RuleId;
        internal IColumn<int> Rule;
        internal IColumn<string> Guid;
        internal IColumn<bool> IsActive;
        internal IColumn<string> Message;
        internal IColumn<int> StartLine;
        internal IColumn<DateTime> WhenDetectedUtc;
        internal IColumn<int> BaselineState;
        internal IColumn<IDictionary<String, String>> Properties;
        internal IColumn<IList<int>> Tags;

        public ResultTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (RunDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            RuleId = GetOrBuild(nameof(RuleId), () => Database.BuildColumn<string>(nameof(Result), nameof(RuleId), default));
            Rule = GetOrBuild(nameof(Rule), () => (IColumn<int>)new RefColumn(nameof(RunDatabase.Rule)));
            Guid = GetOrBuild(nameof(Guid), () => Database.BuildColumn<string>(nameof(Result), nameof(Guid), default));
            IsActive = GetOrBuild(nameof(IsActive), () => Database.BuildColumn<bool>(nameof(Result), nameof(IsActive), default));
            Message = GetOrBuild(nameof(Message), () => Database.BuildColumn<string>(nameof(Result), nameof(Message), default));
            StartLine = GetOrBuild(nameof(StartLine), () => Database.BuildColumn<int>(nameof(Result), nameof(StartLine), default));
            WhenDetectedUtc = GetOrBuild(nameof(WhenDetectedUtc), () => Database.BuildColumn<DateTime>(nameof(Result), nameof(WhenDetectedUtc), default));
            BaselineState = GetOrBuild(nameof(BaselineState), () => Database.BuildColumn<int>(nameof(Result), nameof(BaselineState), (int)default(BaselineState)));
            Properties = GetOrBuild(nameof(Properties), () => Database.BuildColumn<IDictionary<String, String>>(nameof(Result), nameof(Properties), default));
            Tags = GetOrBuild(nameof(Tags), () => Database.BuildColumn<IList<int>>(nameof(Result), nameof(Tags), default));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
