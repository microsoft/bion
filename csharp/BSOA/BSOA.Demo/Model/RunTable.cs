using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Run' entity.
    /// </summary>
    public partial class RunTable : Table<Run>
    {
        internal SarifLogBsoa Database;

        internal RefListColumn Results;
        internal RefListColumn Artifacts;

        public RunTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogBsoa.Result)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogBsoa.Artifact)));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
