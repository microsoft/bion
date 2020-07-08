// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json.Converters;
using BSOA.Model;

using Newtonsoft.Json;

using Xunit;

namespace BSOA.Test.Json
{
    public class JsonToIListTests
    {
        [Fact]
        public void JsonToIList_Basics()
        {
            Action<JsonWriter, IList<int>> writeValueOnly = (w, v) => JsonToIList<int>.Write(w, v, JsonToInt.Write);
            Action<JsonWriter, string, IList<int>, IList<int>, bool> writeNameAndValue = (w, pn, v, dv, r) => JsonToIList<int>.Write(w, pn, v, JsonToInt.Write, r);

            // Lists can be read either by taking the return value, or by passing a list to fill as the argument
            Func<JsonReader, Database, IList<int>> readViaReturnValue = (r, db) => JsonToIList<int>.Read(r, db, null, JsonToInt.Read);
            Func<JsonReader, Database, IList<int>> readViaArgument = (r, db) =>
            {
                if (r.TokenType == JsonToken.Null) { return null; }
                IList<int> result = new List<int>();
                JsonToIList<int>.Read(r, db, result, JsonToInt.Read);
                return result;
            };

            List<int> empty = new List<int>();
            List<int> single = new List<int>() { -55 };

            // Test null (only readable via return value, as argument must be initialized to be passed)
            JsonRoundTrip.ValueOnly(null, writeValueOnly, readViaReturnValue);
            JsonRoundTrip.NameAndValue(single, null, writeNameAndValue, readViaReturnValue);

            // Test empty (must be padded to RoundTrip as default, so underlying code knows to expect no output written)
            JsonRoundTrip.ValueOnly(empty, writeValueOnly, readViaReturnValue);
            JsonRoundTrip.ValueOnly(empty, writeValueOnly, readViaArgument);
            JsonRoundTrip.NameAndValue(single, empty, writeNameAndValue, readViaReturnValue);
            JsonRoundTrip.NameAndValue(single, empty, writeNameAndValue, readViaArgument);

            // Single element array
            JsonRoundTrip.ValueOnly(single, writeValueOnly, readViaReturnValue);
            JsonRoundTrip.ValueOnly(single, writeValueOnly, readViaArgument);

            // Multiple element array
            List<int> several = new List<int>() { 0, 44, 0, -55 };
            JsonRoundTrip.ValueOnly(several, writeValueOnly, readViaReturnValue);
            JsonRoundTrip.ValueOnly(several, writeValueOnly, readViaArgument);
            JsonRoundTrip.NameAndValue(several, null, writeNameAndValue, readViaReturnValue);
            JsonRoundTrip.NameAndValue(several, null, writeNameAndValue, readViaArgument);
        }
    }
}
