using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  GENERATED: BSOA Database
    /// </summary>
    public partial class PersonDatabase : Database
    {
        internal static PersonDatabase Current { get; private set; }

        internal PersonTable Person { get; }

        public PersonDatabase()
        {
            Current = this;

            Person = AddTable(nameof(Person), new PersonTable(this));
        }
    }
}
