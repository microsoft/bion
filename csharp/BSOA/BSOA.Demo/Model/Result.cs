using BSOA.Column;
using BSOA.Model;
using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    public struct Result
    {
        internal ResultTable _table;
        internal int _index;

        public Result(ResultTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Result(ResultTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Result(SarifLogBsoa database) : this(database.Result)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public BaselineState BaselineState
        {
            get => _table.BaselineState[_index];
            set => _table.BaselineState[_index] = value;
        }

        public string RuleId
        {
            get => _table.RuleId[_index];
            set => _table.RuleId[_index] = value;
        }

        public int RuleIndex
        {
            get => _table.RuleIndex[_index];
            set => _table.RuleIndex[_index] = value;
        }

        public Message Message
        {
            get => _table.Database.Message[_table.Message[_index]];
            set => _table.Message[_index] = value._index;
        }

        public IList<Location> Locations
        {
            get => new MutableSliceWrapper<Location, LocationTable>(_table.Locations[_index], _table.Database.Location, (table, index) => new Location(table, index), (item) => item._index);
            set => new MutableSliceWrapper<Location, LocationTable>(_table.Locations[_index], _table.Database.Location, (table, index) => new Location(table, index), (item) => item._index).SetTo(value);
        }

        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }
    }

    public class ResultTable : Table<Result>
    {
        internal SarifLogBsoa Database;

        // Add via 'bcolumn' snippet
        internal EnumColumn<BaselineState, int> BaselineState;
        internal StringColumn RuleId;
        internal NumberColumn<int> RuleIndex;
        internal RefColumn Message;
        internal RefListColumn Locations;
        internal StringColumn Guid;
        
        // *Many* other Result properties not used in Spam Results
        // Used but missing: PartialFingerprints, Properties

        public ResultTable(SarifLogBsoa database) : base()
        {
            this.Database = database;
            this.BaselineState = AddColumn(nameof(BaselineState), new EnumColumn<BaselineState, int>(Microsoft.CodeAnalysis.Sarif.BaselineState.None));
            this.RuleId = AddColumn(nameof(RuleId), new StringColumn());
            this.RuleIndex = AddColumn(nameof(RuleIndex), new NumberColumn<int>(-1));
            this.Message = AddColumn(nameof(Message), new RefColumn(nameof(database.Message)));
            this.Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(database.Location)));
            this.Guid = AddColumn(nameof(Guid), new StringColumn());
        }

        public override Result this[int index] => new Result(this, index);
    }
}
