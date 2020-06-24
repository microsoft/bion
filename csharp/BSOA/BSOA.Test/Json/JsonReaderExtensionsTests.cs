// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

using BSOA.Test.Model.V2;

using Newtonsoft.Json;

using Xunit;

namespace BSOA.Test.Json
{
    public class JsonReaderExtensionsTests
    {
        [Fact]
        public void JsonReaderExtensions_Basics()
        {
            Person result;
            
            // Verify 'null' allowed (no setters set)
            result = JsonRoundTrip.Parse<Community, Person>("null", JsonToPerson.Read);
            Assert.Null(result);

            // Verify empty allowed (no setters set)
            result = JsonRoundTrip.Parse<Community, Person>("{ }", JsonToPerson.Read);
            Assert.Equal(new Person(), result);

            // Verify some properties may be ommitted
            result = JsonRoundTrip.Parse<Community, Person>("{ \"name\": \"Scott\" }", JsonToPerson.Read);
            Assert.Equal("Scott", result.Name);
            Assert.Equal(new Person().Birthdate, result.Birthdate);

            // Verify Read/Write happy path
            Person p = new Person() { Name = "Scott", Birthdate = new DateTime(1981, 01, 01).ToUniversalTime() };
            JsonRoundTrip.ValueOnly(p, JsonToPerson.Write, (r, db) => JsonToPerson.Read(r));

            if (!Debugger.IsAttached)
            {
                // Verify ReadObject validates that JSON is an object
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<Community, Person>("[ \"Scott\" ])", JsonToPerson.Read));

                // Verify ReadObject validates that JSON object is closed properly
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<Community, Person>("{ \"name\": \"Scott\" ", JsonToPerson.Read));

                // Verify ReadObject throws for unknown Property Names
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<Community, Person>("{ \"name\": \"Scott\", \"age\": 39 }", JsonToPerson.Read));

                // Verify Expect/Throw handles non-JsonTextReader
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Bson_ValueOnly(p, JsonToPerson.Write, (r, db) =>
                {
                    r.Expect(JsonToken.StartArray);
                    return null;
                }));
            }
        }
    }
}
