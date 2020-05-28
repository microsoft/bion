using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    public partial class PersonDatabase
    {
        public LimitedList<Person> People => this.Person;
    }
}
