using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    public struct PhysicalLocation
    {
        internal PhysicalLocationTable _table;
        internal int _index;

        public PhysicalLocation(PhysicalLocationTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public PhysicalLocation(PhysicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public PhysicalLocation(SarifLogBsoa database) : this(database.PhysicalLocation)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public ArtifactLocation ArtifactLocation
        {
            get => _table.Database.ArtifactLocation[_table.ArtifactLocation[_index]];
            set => _table.ArtifactLocation[_index] = value._index;
        }

        public Region Region
        {
            get => _table.Database.Region[_table.Region[_index]];
            set => _table.Region[_index] = value._index;
        }

        public Region ContextRegion
        {
            get => _table.Database.Region[_table.ContextRegion[_index]];
            set => _table.ContextRegion[_index] = value._index;
        }
    }

    public class PhysicalLocationTable : Table<PhysicalLocation>
    {
        internal SarifLogBsoa Database;

        internal RefColumn ArtifactLocation;
        internal RefColumn Region;
        internal RefColumn ContextRegion;

        // Address Address
        // Properties

        public PhysicalLocationTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(database.ArtifactLocation)));
            this.Region = AddColumn(nameof(Region), new RefColumn(nameof(database.Region)));
            this.ContextRegion = AddColumn(nameof(ContextRegion), new RefColumn(nameof(database.Region)));
        }

        public override PhysicalLocation this[int index] => new PhysicalLocation(this, index);
    }

}
