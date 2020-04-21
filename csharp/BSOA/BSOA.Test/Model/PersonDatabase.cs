using BSOA.Model;

namespace BSOA.Test.Model
{
    public class PersonDatabase : Database
    {
        public PersonTable People { get; }

        public PersonDatabase()
        {
            People = AddTable(nameof(People), new PersonTable(this));
        }
    }
}
