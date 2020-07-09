// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Test.Model.V1;

using System.Collections.Generic;

using Xunit;

namespace BSOA.Test.Model
{
    public class TableTests
    {
        [Fact]
        public void Table_AsColumn()
        {
            // Tables are usable as Columns, allowing nesting a full table of columns as a single named column.
            // Test Table ILimitedList members by using the Column.Basics testing.

            V1.PersonDatabase db = new V1.PersonDatabase();
            V1.PersonTable other = db.Person;
            V1.Person defaultPerson = new Person(other);
            V1.Person nonDefaultPerson = new Person(other) { Age = 39, Name = "Scott" };

            Column.Basics<V1.Person>(
                () => new PersonTable(new PersonDatabase()),
                defaultPerson,
                nonDefaultPerson,
                (i) => new Person(other) { Age = (byte)(i % byte.MaxValue) }
            );

            // Set to already correct instance
            string name = other[1].Name;
            other[1] = other[1];
            Assert.Equal(name, other[1].Name);

            // Verify Add detects already is last Table item
            int count = other.Count;
            other.Add(other[count - 1]);
            Assert.Equal(count, other.Count);
        }

        [Fact]
        public void Table_Basics()
        {
            V1.Community other = new V1.Community();
            V1.Community v1 = new V1.Community();

            v1.People = new List<Person>();
            IList<Person> people = v1.People;

            // Add item already in correct instance
            people.Add(new V1.Person(v1) { Age = 39, Name = "Scott" });
            Assert.Equal("Scott", people[0].Name);

            // Add item copied from other instance
            people.Add(new V1.Person(other) { Age = 36, Name = "Adam" });
            Assert.Equal("Adam", people[1].Name);

            // Try setter from other DB
            V1.Person dave = new V1.Person(other) { Age = 45, Name = "Dave" };
            Assert.Equal(2, people.Count);
            people[1] = dave;
            Assert.Equal(2, people.Count);
            Assert.Equal("Dave", people[1].Name);

            // Set to already correct instance
            v1.People[1] = v1.People[1];

            // Set and verify null
            v1.People[1] = null;
            Assert.Null(v1.People[1]);
        }
    }
}
