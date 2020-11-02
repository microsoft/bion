// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Test.Model.Log
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Run'
    /// </summary>
    internal partial class RunDatabase : Database
    {
        internal ResultTable Result;
        internal RuleTable Rule;
        internal RunTable Run;

        public RunDatabase() : base("Run")
        {
            _lastCreated = new WeakReference<RunDatabase>(this);
            GetOrBuildTables();
        }

        public override void GetOrBuildTables()
        {
            Result = GetOrBuild(nameof(Result), () => new ResultTable(this));
            Rule = GetOrBuild(nameof(Rule), () => new RuleTable(this));
            Run = GetOrBuild(nameof(Run), () => new RunTable(this));
        }

        [ThreadStatic]
        private static WeakReference<RunDatabase> _lastCreated;

        internal static RunDatabase Current
        {
            get
            {
                RunDatabase db;
                if (_lastCreated == null || !_lastCreated.TryGetTarget(out db)) { db = new RunDatabase(); }
                return db;
            }
        }
    }
}
