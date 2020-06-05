using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Run'
    /// </summary>
    internal partial class RunTable : Table<Run>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Tool;
        internal RefListColumn Artifacts;
        internal RefListColumn Results;

        internal RunTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tool = AddColumn(nameof(Tool), new RefColumn(nameof(SarifLogDatabase.Tool)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogDatabase.Artifact)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogDatabase.Result)));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
