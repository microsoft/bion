using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  PersonTable (V2) shows a change to Person - replacing the 'Age' column with 'Birthdate'
    /// </summary>
    public class PersonTable : Table<Person>
    {
        internal PersonDatabase Database;

        //internal IColumn<byte> Age;
        internal IColumn<DateTime> Birthdate;
        internal IColumn<string> Name;

        public PersonTable(PersonDatabase database) : base()
        {
            Database = database;

            //Age = AddColumn(nameof(Age), new NumberColumn<byte>(0));
            Birthdate = AddColumn(nameof(Birthdate), new DateTimeColumn(DateTime.MinValue));
            Name = AddColumn(nameof(Name), new StringColumn());
        }

        protected override Person Get(int index)
        {
            return new Person(this, index);
        }
    }
}
