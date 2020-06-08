using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactContent'
    /// </summary>
    internal partial class ArtifactContentTable : Table<ArtifactContent>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Text;
        internal IColumn<string> Binary;

        internal ArtifactContentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(null));
            Binary = AddColumn(nameof(Binary), ColumnFactory.Build<string>(null));
        }

        public override ArtifactContent Get(int index)
        {
            return (index == -1 ? null : new ArtifactContent(this, index));
        }
    }
}
