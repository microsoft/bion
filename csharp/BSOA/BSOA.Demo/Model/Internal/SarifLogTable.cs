using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'SarifLog'
    /// </summary>
    internal partial class SarifLogTable : Table<SarifLog>
    {
        internal SarifLogDatabase Database;

        internal RefListColumn Runs;

        internal SarifLogTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Runs = AddColumn(nameof(Runs), new RefListColumn(nameof(SarifLogDatabase.Run)));
        }

        public override SarifLog Get(int index)
        {
            return (index == -1 ? null : new SarifLog(this, index));
        }
    }
}
