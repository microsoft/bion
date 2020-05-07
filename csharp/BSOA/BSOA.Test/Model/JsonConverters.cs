using Newtonsoft.Json.Converters;
using System;

namespace BSOA.Test.Model
{
    public class PersonConverter : CustomCreationConverter<Person>
    {
        public override Person Create(Type objectType)
        {
            return new Person(PersonDatabaseConverter.Current);
        }
    }

    public class PersonTableConverter : CustomCreationConverter<PersonTable>
    {
        public override PersonTable Create(Type objectType)
        {
            return PersonDatabaseConverter.Current.Person;
        }
    }

    public class PersonDatabaseConverter : CustomCreationConverter<PersonDatabase>
    {
        private static PersonDatabase _current;
        internal static PersonDatabase Current
        {
            get { return _current ??= new PersonDatabase(); }
        }

        public override PersonDatabase Create(Type objectType)
        {
            PersonDatabase db = new PersonDatabase();
            _current = db;
            return db;
        }
    }
}
