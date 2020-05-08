using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    public class PersonDatabase : Database
    {
        public static PersonDatabase Current { get; private set; }
        public PersonTable Person { get; }

        public PersonDatabase()
        {
            Current = this;
            Person = AddTable(nameof(Person), new PersonTable(this));
        }
    }
}
