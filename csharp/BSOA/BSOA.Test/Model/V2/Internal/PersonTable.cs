using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Person'
    /// </summary>
    internal partial class PersonTable : Table<Person>
    {
        internal PersonDatabase Database;

        internal IColumn<DateTime> Birthdate;
        internal IColumn<string> Name;

        internal PersonTable(PersonDatabase database) : base()
        {
            Database = database;

            Birthdate = AddColumn(nameof(Birthdate), ColumnFactory.Build<DateTime>());
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>());
        }

        public override Person Get(int index)
        {
            return (index == -1 ? null : new Person(this, index));
        }
    }
}
