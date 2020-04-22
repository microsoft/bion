using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    public class PersonDatabase : Database
    {
        public PersonTable Person { get; }

        public PersonDatabase()
        {
            Person = AddTable(nameof(Person), new PersonTable(this));
        }
    }
}
