using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Team'
    /// </summary>
    public partial class Team : IRow
    {
        private TeamTable _table;
        private int _index;

        internal Team(TeamTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Team(TeamTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Team(CompanyDatabase database) : this(database.Team)
        { }

        public Team() : this(CompanyDatabase.Current)
        { }

        // <Columns>
        //   <SimpleColumn>
        public DateTime WhenFormed
        {
            get => _table.WhenFormed[_index];
            set => _table.WhenFormed[_index] = value;
        }
        //   </SimpleColumn>

        //   <EnumColumn>
        public SecurityPolicy JoinPolicy
        {
            get => (SecurityPolicy)_table.JoinPolicy[_index];
            set => _table.JoinPolicy[_index] = (byte)value;
        }
        //   </EnumColumn>

        //   <RefColumn>
        public Employee Manager
        {
            get => _table.Database.Employee.Get(_table.Manager[_index]);
            set => _table.Manager[_index] = _table.Database.Employee.LocalIndex(value);
        }
        //   </RefColumn>

        //   <RefListColumn>
        public IList<Employee> Members
        {
            get => _table.Database.Employee.List(_table.Members[_index]);
        }
        //   </RefListColumn>
        // </Columns>

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (TeamTable)table;
            _index = index;
        }
        #endregion
    }
}
