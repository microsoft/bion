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
        internal ResultTable Result { get; }
        internal RuleTable Rule { get; }
        internal RunTable Run { get; }

        public RunDatabase() : base("Run")
        {
            _lastCreated = new WeakReference<RunDatabase>(this);

            Result = AddTable(nameof(Result), new ResultTable(this));
            Rule = AddTable(nameof(Rule), new RuleTable(this));
            Run = AddTable(nameof(Run), new RunTable(this));
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
