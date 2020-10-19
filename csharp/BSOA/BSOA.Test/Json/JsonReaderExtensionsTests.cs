// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

using V1 = BSOA.Test.Model.V1;
using V2 = BSOA.Test.Model.V2;

using Newtonsoft.Json;

using Xunit;

namespace BSOA.Test.Json
{
    public class JsonReaderExtensionsTests
    {
        [Fact]
        public void JsonReaderExtensions_Basics()
        {
            V1.Person result;
            
            // Verify 'null' allowed (no setters set)
            result = JsonRoundTrip.Parse<V1.Community, V1.Person>("null", V1.JsonToPerson.Read);
            Assert.Null(result);

            // Verify empty allowed (no setters set)
            result = JsonRoundTrip.Parse<V1.Community, V1.Person>("{ }", V1.JsonToPerson.Read);
            Assert.Equal(new V1.Person(), result);

            // Verify some properties may be ommitted
            result = JsonRoundTrip.Parse<V1.Community, V1.Person>("{ \"name\": \"Scott\" }", V1.JsonToPerson.Read);
            Assert.Equal("Scott", result.Name);
            Assert.Equal(new V1.Person().Age, result.Age);

            // Verify Read/Write happy path
            V1.Person p = new V1.Person() { Name = "Scott", Age = 39 };
            JsonRoundTrip.ValueOnly(p, V1.JsonToPerson.Write, (r, db) => V1.JsonToPerson.Read(r));

            // Verify ReadObject skips unknown Property Names when configured to (see V2 Model postReplacements)
            JsonRoundTrip.Parse<V2.Community, V2.Person>("{ \"name\": \"Scott\", \"age\": 39 }", V2.JsonToPerson.Read);

            if (!Debugger.IsAttached)
            {
                // Verify ReadObject validates that JSON is an object
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<V1.Community, V1.Person>("[ \"Scott\" ])", V1.JsonToPerson.Read));

                // Verify ReadObject validates that JSON object is closed properly
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<V1.Community, V1.Person>("{ \"name\": \"Scott\" ", V1.JsonToPerson.Read));

                // Verify ReadObject throws for unknown Property Names
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Parse<V1.Community, V1.Person>("{ \"name\": \"Scott\", \"birthdate\": \"2000-01-01\" }", V1.JsonToPerson.Read));

                // Verify Expect/Throw handles non-JsonTextReader
                Assert.Throws<JsonReaderException>(() => JsonRoundTrip.Bson_ValueOnly(p, V1.JsonToPerson.Write, (r, db) =>
                {
                    r.Expect(JsonToken.StartArray);
                    return null;
                }));
            }
        }
    }
}
