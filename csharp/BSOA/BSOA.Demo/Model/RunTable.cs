using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Run' entity.
    /// </summary>
    public partial class RunTable : Table<Run>
    {
        internal SarifLogBsoa Database;

        internal RefColumn Tool;
        internal RefListColumn Artifacts;
        internal RefListColumn Results;

        public RunTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Tool = AddColumn(nameof(Tool), new RefColumn(nameof(SarifLogBsoa.Tool)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogBsoa.Artifact)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogBsoa.Result)));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
