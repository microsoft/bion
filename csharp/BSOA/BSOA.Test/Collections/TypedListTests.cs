using BSOA.Collections;
using BSOA.Column;
using BSOA.Test.Model.V1;

using System;
using System.Collections.Generic;

using Xunit;

namespace BSOA.Test.Collections
{
    public class TypedListTests
    {
        [Fact]
        public void TypedList_Basics()
        {
            Community c = new Community();

            List<Person> people = new List<Person> 
            {
                new Person(c) { Name = "One" },
                new Person(c) { Name = "Two" },
                new Person(c) { Name = "Three" }
            };

            // Null by default
            Assert.Null(c.People);
            
            // Settable to Empty
            c.People = Array.Empty<Person>();
            
            TypedList<Person> list = (TypedList<Person>)c.People;
            Assert.Empty(list);

            list.Add(people[0]);
            Assert.Single(list);

            list.Add(people[1]);
            list.Add(people[2]);
            CollectionReadVerifier.VerifySame(people, list);

            // SetTo self works properly
            list.SetTo(list);
            CollectionReadVerifier.VerifySame(people, list);

            // SetTo null works
            list.SetTo(null);
            Assert.Empty(list);

            // SetTo other works
            list.SetTo(people);
            CollectionReadVerifier.VerifySame(people, list);

            // SetTo empty works
            list.SetTo(new List<Person>());
            Assert.Empty(list);

            CollectionChangeVerifier.VerifyList(c.People, (i) => new Person(c) { Age = (byte)i });
        }
    }
}
