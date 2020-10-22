// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Benchmarks.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Result'
    /// </summary>
    internal partial class ResultTable : Table<Result>
    {
        internal RunDatabase Database;

        internal IColumn<string> RuleId;
        internal IColumn<string> Guid;
        internal IColumn<bool> IsActive;
        internal IColumn<string> Message;
        internal IColumn<int> StartLine;
        internal IColumn<DateTime> WhenDetectedUtc;
        internal IColumn<int> BaselineState;
        internal IColumn<IDictionary<String, String>> Properties;
        internal IColumn<IList<int>> Tags;

        internal ResultTable(RunDatabase database) : base()
        {
            Database = database;

            RuleId = AddColumn(nameof(RuleId), database.BuildColumn<string>(nameof(Result), nameof(RuleId), default));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<string>(nameof(Result), nameof(Guid), default));
            IsActive = AddColumn(nameof(IsActive), database.BuildColumn<bool>(nameof(Result), nameof(IsActive), default));
            Message = AddColumn(nameof(Message), database.BuildColumn<string>(nameof(Result), nameof(Message), default));
            StartLine = AddColumn(nameof(StartLine), database.BuildColumn<int>(nameof(Result), nameof(StartLine), default));
            WhenDetectedUtc = AddColumn(nameof(WhenDetectedUtc), database.BuildColumn<DateTime>(nameof(Result), nameof(WhenDetectedUtc), default));
            BaselineState = AddColumn(nameof(BaselineState), database.BuildColumn<int>(nameof(Result), nameof(BaselineState), (int)default(BaselineState)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, String>>(nameof(Result), nameof(Properties), default));
            Tags = AddColumn(nameof(Tags), database.BuildColumn<IList<int>>(nameof(Result), nameof(Tags), default));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
