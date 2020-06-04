using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Person' entity.
    /// </summary>
    public partial class PersonTable : Table<Person>
    {
        internal PersonDatabase Database;

        internal IColumn<DateTime> Birthdate;
        internal IColumn<string> Name;

        public PersonTable(PersonDatabase database) : base()
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
