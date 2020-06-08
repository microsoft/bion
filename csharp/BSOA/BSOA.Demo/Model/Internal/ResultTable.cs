using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Result'
    /// </summary>
    internal partial class ResultTable : Table<Result>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> BaselineState;
        internal IColumn<string> RuleId;
        internal IColumn<int> RuleIndex;
        internal RefColumn Message;
        internal RefListColumn Locations;
        internal IColumn<string> Guid;

        internal ResultTable(SarifLogDatabase database) : base()
        {
            Database = database;

            BaselineState = AddColumn(nameof(BaselineState), ColumnFactory.Build<int>((int)Microsoft.CodeAnalysis.Sarif.BaselineState.None));
            RuleId = AddColumn(nameof(RuleId), ColumnFactory.Build<string>(null));
            RuleIndex = AddColumn(nameof(RuleIndex), ColumnFactory.Build<int>(-1));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(null));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
