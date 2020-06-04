using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Root' entity.
    /// </summary>
    internal partial class RootTable : Table<Root>
    {
        internal SarifLogBsoa Database;

        internal RefListColumn Runs;

        public RootTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Runs = AddColumn(nameof(Runs), new RefListColumn(nameof(SarifLogBsoa.Run)));
        }

        public override Root Get(int index)
        {
            return (index == -1 ? null : new Root(this, index));
        }
    }
}
