// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'GraphTraversal'
    /// </summary>
    internal partial class GraphTraversalTable : Table<GraphTraversal>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> RunGraphIndex;
        internal IColumn<int> ResultGraphIndex;
        internal RefColumn Description;
        internal IColumn<IDictionary<String, MultiformatMessageString>> InitialState;
        internal IColumn<IDictionary<String, MultiformatMessageString>> ImmutableState;
        internal RefListColumn EdgeTraversals;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal GraphTraversalTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RunGraphIndex = AddColumn(nameof(RunGraphIndex), database.BuildColumn<int>(nameof(GraphTraversal), nameof(RunGraphIndex), -1));
            ResultGraphIndex = AddColumn(nameof(ResultGraphIndex), database.BuildColumn<int>(nameof(GraphTraversal), nameof(ResultGraphIndex), -1));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            InitialState = AddColumn(nameof(InitialState), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(GraphTraversal), nameof(InitialState), default));
            ImmutableState = AddColumn(nameof(ImmutableState), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(GraphTraversal), nameof(ImmutableState), default));
            EdgeTraversals = AddColumn(nameof(EdgeTraversals), new RefListColumn(nameof(SarifLogDatabase.EdgeTraversal)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(GraphTraversal), nameof(Properties), default));
        }

        public override GraphTraversal Get(int index)
        {
            return (index == -1 ? null : new GraphTraversal(this, index));
        }
    }
}
