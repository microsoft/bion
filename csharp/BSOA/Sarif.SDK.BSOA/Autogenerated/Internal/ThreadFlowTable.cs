using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ThreadFlow'
    /// </summary>
    internal partial class ThreadFlowTable : Table<ThreadFlow>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Id;
        internal RefColumn Message;
        internal IColumn<IDictionary<string, MultiformatMessageString>> InitialState;
        internal IColumn<IDictionary<string, MultiformatMessageString>> ImmutableState;
        internal RefListColumn Locations;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ThreadFlowTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>());
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            InitialState = AddColumn(nameof(InitialState), new DictionaryColumn<string, MultiformatMessageString>(new StringColumn(), new MultiformatMessageStringColumn(this.Database)));
            ImmutableState = AddColumn(nameof(ImmutableState), new DictionaryColumn<string, MultiformatMessageString>(new StringColumn(), new MultiformatMessageStringColumn(this.Database)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override ThreadFlow Get(int index)
        {
            return (index == -1 ? null : new ThreadFlow(this, index));
        }
    }
}