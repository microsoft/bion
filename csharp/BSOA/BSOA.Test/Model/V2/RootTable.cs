using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Root' entity.
    /// </summary>
    internal partial class RootTable : Table<Root>
    {
        internal PersonDatabase Database;

        internal RefListColumn People;

        public RootTable(PersonDatabase database) : base()
        {
            Database = database;

            People = AddColumn(nameof(People), new RefListColumn(nameof(PersonDatabase.Person)));
        }

        public override Root Get(int index)
        {
            return (index == -1 ? null : new Root(this, index));
        }
    }
}
