using BSOA.Model;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Community'
    /// </summary>
    internal partial class PersonDatabase : Database
    {
        internal static PersonDatabase Current { get; private set; }
        
        internal PersonTable Person { get; }
        internal CommunityTable Community { get; }

        public PersonDatabase()
        {
            Current = this;

            Person = AddTable(nameof(Person), new PersonTable(this));
            Community = AddTable(nameof(Community), new CommunityTable(this));
        }
    }
}
