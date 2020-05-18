using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    public readonly struct LogicalLocation
    {
        internal readonly LogicalLocationTable _table;
        internal readonly int _index;

        public LogicalLocation(LogicalLocationTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public LogicalLocation(LogicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public LogicalLocation(SarifLogBsoa database) : this(database.LogicalLocation)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        public string DecoratedName
        {
            get => _table.DecoratedName[_index];
            set => _table.DecoratedName[_index] = value;
        }

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }
    }

    public class LogicalLocationTable : Table<LogicalLocation>
    {
        internal SarifLogBsoa Database;

        internal StringColumn Name;
        internal NumberColumn<int> Index;
        internal StringColumn FullyQualifiedName;
        internal StringColumn DecoratedName;
        internal NumberColumn<int> ParentIndex;
        internal StringColumn Kind;

        // Properties

        public LogicalLocationTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.Name = AddColumn(nameof(Name), new StringColumn());
            this.Index = AddColumn(nameof(Index), new NumberColumn<int>(-1));
            this.FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), new StringColumn());
            this.DecoratedName = AddColumn(nameof(DecoratedName), new StringColumn());
            this.ParentIndex = AddColumn(nameof(ParentIndex), new NumberColumn<int>(-1));
            this.Kind = AddColumn(nameof(Kind), new StringColumn());
        }

        public override LogicalLocation this[int index] => new LogicalLocation(this, index);
    }

}
