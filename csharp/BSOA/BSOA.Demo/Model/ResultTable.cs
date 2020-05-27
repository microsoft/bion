using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Result' entity.
    /// </summary>
    public partial class ResultTable : Table<Result>
    {
        internal SarifLogBsoa Database;

        internal IColumn<int> BaselineState;
        internal IColumn<string> RuleId;
        internal IColumn<int> RuleIndex;
        internal RefColumn Message;
        internal RefListColumn Locations;
        internal IColumn<string> Guid;

        public ResultTable(SarifLogBsoa database) : base()
        {
            Database = database;

            BaselineState = AddColumn(nameof(BaselineState), ColumnFactory.Build<int>((int)Microsoft.CodeAnalysis.Sarif.BaselineState.None));
            RuleId = AddColumn(nameof(RuleId), ColumnFactory.Build<string>(null));
            RuleIndex = AddColumn(nameof(RuleIndex), ColumnFactory.Build<int>(-1));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogBsoa.Message)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogBsoa.Location)));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(null));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
