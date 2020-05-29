using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Person' entity.
    /// </summary>
    public partial class PersonTable : Table<Person>
    {
        internal PersonDatabase Database;

        internal IColumn<byte> Age;
        internal IColumn<string> Name;

        public PersonTable(PersonDatabase database) : base()
        {
            Database = database;

            Age = AddColumn(nameof(Age), ColumnFactory.Build<byte>());
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>());
        }

        public override Person Get(int index)
        {
            return (index == -1 ? null : new Person(this, index));
        }
    }
}
