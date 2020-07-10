// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;

using BSOA.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using Xunit;

namespace BSOA.Test.Json
{
    public static class JsonRoundTrip
    {
        public static void NameAndValue<TItem>(TItem value, TItem defaultValue, Action<JsonWriter, string, TItem, TItem, bool> write, Func<JsonReader, Database, TItem> read)
        {
            TItem roundTripped = default;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream, leaveOpen: true))
                using (JsonTextWriter jtw = new JsonTextWriter(sw))
                {
                    jtw.WriteStartObject();

                    // Write PropertyName and Value
                    write(jtw, "propertyName", value, defaultValue, false);

                    // Write PropertyName and Default (nothing should be written)
                    write(jtw, "propertyWithDefault", defaultValue, defaultValue, false);

                    // Write PropertyName and Default, Required (default should be written)
                    write(jtw, "requiredWithDefault", defaultValue, defaultValue, true);

                    jtw.WriteEndObject();
                }

                long bytesWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader sr = new StreamReader(stream, leaveOpen: true))
                using (JsonTextReader jtr = new JsonTextReader(sr))
                {
                    jtr.Read();
                    Assert.Equal(JsonToken.StartObject, jtr.TokenType);

                    // Confirm Property Name and value
                    jtr.Read();
                    Assert.Equal(JsonToken.PropertyName, jtr.TokenType);
                    Assert.Equal("propertyName", (string)jtr.Value);
                    jtr.Read();
                    roundTripped = read(jtr, default(Database));

                    // Confirm RequiredWithDefault written (and PropertyWithDefault *not*)
                    jtr.Read();
                    Assert.Equal(JsonToken.PropertyName, jtr.TokenType);
                    Assert.Equal("requiredWithDefault", (string)jtr.Value);
                    jtr.Read();
                    TItem roundTrippedDefault = read(jtr, default(Database));
                    Assert.Equal(defaultValue, roundTrippedDefault);

                    // Confirm end object (propertyWithDefault not written)
                    jtr.Read();
                    Assert.Equal(JsonToken.EndObject, jtr.TokenType);
                }

                // Ensure everything written is read back
                Assert.Equal(bytesWritten, stream.Position);

                // Ensure value returned matches value written
                Assert.Equal(value, roundTripped);
            }
        }

        public static void ValueOnly<TItem>(TItem value, Action<JsonWriter, TItem> write, Func<JsonReader, Database, TItem> read)
        {
            TItem roundTripped = default;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream, leaveOpen: true))
                using (JsonTextWriter jtw = new JsonTextWriter(sw))
                {
                    // Write value alone
                    write(jtw, value);
                }

                long bytesWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader sr = new StreamReader(stream, leaveOpen: true))
                using (JsonTextReader jtr = new JsonTextReader(sr))
                {
                    jtr.Read();

                    // Read value
                    roundTripped = read(jtr, default(Database));
                }

                // Ensure everything written is read back
                Assert.Equal(bytesWritten, stream.Position);

                // Ensure value returned matches value written
                Assert.Equal(value, roundTripped);
            }
        }

        public static void Bson_ValueOnly<TItem>(TItem value, Action<JsonWriter, TItem> write, Func<JsonReader, Database, TItem> read)
        {
            TItem roundTripped = default;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                using (BsonDataWriter bdw = new BsonDataWriter(bw))
                {
                    // Write value alone
                    write(bdw, value);
                }

                long bytesWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                using (BsonDataReader bdr = new BsonDataReader(br))
                {
                    bdr.Read();

                    // Read value
                    roundTripped = read(bdr, default(Database));
                }

                // Ensure everything written is read back
                Assert.Equal(bytesWritten, stream.Position);

                // Ensure value returned matches value written
                Assert.Equal(value, roundTripped);
            }
        }

        public static TItem Parse<TRoot, TItem>(string jsonText, Func<JsonReader, TRoot, TItem> read)
        {
            TItem roundTripped = default;

            using (StringReader sr = new StringReader(jsonText))
            using (JsonTextReader jtr = new JsonTextReader(sr))
            {
                jtr.Read();

                // Read value
                roundTripped = read(jtr, default(TRoot));

                // Ensure no remaining tokens
                Assert.False(jtr.Read());
            }

            return roundTripped;
        }
    }
}
