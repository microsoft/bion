// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

using BSOA.Json.Converters;
using BSOA.Model;

using Newtonsoft.Json;

using Xunit;

namespace BSOA.Test.Json
{
    public class JsonToIDictionaryTests
    {
        [Fact]
        public void JsonToIDictionary_Basics()
        {
            Action<JsonWriter, IDictionary<string, string>> writeValueOnly = (w, v) => JsonToIDictionary<string, string>.Write(w, v, JsonToString.Write);
            Action<JsonWriter, string, IDictionary<string, string>, IDictionary<string, string>> writeNameAndValue = (w, pn, v, dv) => JsonToIDictionary<string, string>.Write(w, pn, v, JsonToString.Write);

            // Dictionaries can be read by taking the return value or passing a Dictionary to fill as the argument
            Func<JsonReader, Database, IDictionary<string, string>> readViaReturnValue = (r, db) => JsonToIDictionary<string, string>.Read(r, db, null, JsonToString.Read);
            Func<JsonReader, Database, IDictionary<string, string>> readViaArgument = (r, db) =>
            {
                IDictionary<string, string> result = new Dictionary<string, string>();
                JsonToIDictionary<string, string>.Read(r, db, result, JsonToString.Read);
                return result;
            };

            Dictionary<string, string> empty = new Dictionary<string, string>();
            Dictionary<string, string> single = new Dictionary<string, string>()
            {
                ["Name"] = "single"
            };

            // Test null(only readable via return value, as argument must be initialized to be passed)
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
            Dictionary<string, string> several = new Dictionary<string, string>()
            {
                ["Null"] = null,
                ["Empty"] = "",
                ["Simple"] = "Value"
            };

            JsonRoundTrip.ValueOnly(several, writeValueOnly, readViaReturnValue);
            JsonRoundTrip.ValueOnly(several, writeValueOnly, readViaArgument);
            JsonRoundTrip.NameAndValue(several, null, writeNameAndValue, readViaReturnValue);
            JsonRoundTrip.NameAndValue(several, null, writeNameAndValue, readViaArgument);
        }
    }
}
