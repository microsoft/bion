using BSOA.Model;

namespace BSOA.Test.Model.V2
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
