// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Benchmarks.Model
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

        internal RuleTable(RunDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), database.BuildColumn<string>(nameof(Rule), nameof(Id), default));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(Rule), nameof(Guid), default));
            HelpUri = AddColumn(nameof(HelpUri), database.BuildColumn<Uri>(nameof(Rule), nameof(HelpUri), default));
        }

        public override Rule Get(int index)
        {
            return (index == -1 ? null : new Rule(this, index));
        }
    }
}
