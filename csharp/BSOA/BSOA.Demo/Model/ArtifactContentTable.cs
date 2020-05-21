using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'ArtifactContent' entity.
    /// </summary>
    public partial class ArtifactContentTable : Table<ArtifactContent>
    {
        internal SarifLogBsoa Database;

        internal IColumn<string> Text;
        internal IColumn<string> Binary;

        public ArtifactContentTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>());
            Binary = AddColumn(nameof(Binary), ColumnFactory.Build<string>());
        }

        public override ArtifactContent Get(int index)
        {
            return (index == -1 ? null : new ArtifactContent(this, index));
        }
    }
}
