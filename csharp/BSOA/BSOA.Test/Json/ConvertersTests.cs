using BSOA.Json.Converters;
using BSOA.Model;

using Newtonsoft.Json;

using System;
using System.IO;

using Xunit;

namespace BSOA.Test.Json
{
    public class ConvertersTests
    {
        [Fact]
        public void JsonToBool_Basics()
        {
            JsonRoundTrip_ValueOnly(true, JsonToBool.Write, JsonToBool.Read);

            JsonRoundTrip(true, false, JsonToBool.Write, JsonToBool.Read<Database>);
            JsonRoundTrip(false, true, JsonToBool.Write, JsonToBool.Read<Database>);
        }

        [Fact]
        public void JsonToDateTime_Basics()
        {
            // BSOA DateTime format is precise to the millisecond only
            DateTime now = ToMillisecond(DateTime.UtcNow);

            JsonRoundTrip_ValueOnly(now, JsonToDateTime.Write, JsonToDateTime.Read);

            JsonRoundTrip(now, DateTime.MinValue, JsonToDateTime.Write, JsonToDateTime.Read);
            JsonRoundTrip(ToMillisecond(DateTime.MinValue.ToUniversalTime()), now, JsonToDateTime.Write, JsonToDateTime.Read);
            JsonRoundTrip(ToMillisecond(DateTime.MaxValue.ToUniversalTime()), now, JsonToDateTime.Write, JsonToDateTime.Read);

            DateTime nonStandardFormat = JsonParse("\"01/01/2020\"", JsonToDateTime.Read);
            Assert.Equal(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), nonStandardFormat);
        }

        private static DateTime ToMillisecond(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind);
        }

        [Fact]
        public void JsonToEnum_Basics()
        {
            JsonRoundTrip_ValueOnly(DayOfWeek.Thursday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);

            JsonRoundTrip(DayOfWeek.Thursday, DayOfWeek.Sunday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);
            JsonRoundTrip(DayOfWeek.Wednesday, DayOfWeek.Thursday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);

            // Flags enum
            JsonRoundTrip(FileAttributes.ReadOnly | FileAttributes.Directory, default(FileAttributes), JsonToEnum<FileAttributes>.Write, JsonToEnum<FileAttributes>.Read);
        }

        [Fact]
        public void JsonToString_Basics()
        {
            JsonRoundTrip_ValueOnly(null, JsonToString.Write, JsonToString.Read);
            JsonRoundTrip_ValueOnly("", JsonToString.Write, JsonToString.Read);
            JsonRoundTrip_ValueOnly("simple", JsonToString.Write, JsonToString.Read);
            JsonRoundTrip_ValueOnly("'\"\\\"'", JsonToString.Write, JsonToString.Read);

            JsonRoundTrip("Simple", null, JsonToString.Write, JsonToString.Read);
            JsonRoundTrip(null, "NonNullDefault", JsonToString.Write, JsonToString.Read);
        }

        [Fact]
        public void JsonToUri_Basics()
        {
            JsonRoundTrip_ValueOnly(null, JsonToUri.Write, JsonToUri.Read);
            JsonRoundTrip_ValueOnly(new Uri("https://www.github.com/Microsoft/bion"), JsonToUri.Write, JsonToUri.Read);

            // Null default
            JsonRoundTrip(new Uri("https://www.github.com/Microsoft/bion"), null, JsonToUri.Write, JsonToUri.Read);

            // Non-null default
            JsonRoundTrip(new Uri("https://www.github.com/Microsoft/bion"), new Uri("src/Program.cs", UriKind.Relative), JsonToUri.Write, JsonToUri.Read);

            // Null when default is not null
            JsonRoundTrip(null, new Uri("src/Program.cs", UriKind.Relative), JsonToUri.Write, JsonToUri.Read);
        }

        [Fact]
        public void JsonToNumbers_Basics()
        {
            JsonRoundTrip_ValueOnly((byte)4, JsonToByte.Write, JsonToByte.Read);
            JsonRoundTrip(byte.MaxValue, byte.MinValue, JsonToByte.Write, JsonToByte.Read);

            JsonRoundTrip_ValueOnly((sbyte)4, JsonToSbyte.Write, JsonToSbyte.Read);
            JsonRoundTrip(sbyte.MaxValue, sbyte.MinValue, JsonToSbyte.Write, JsonToSbyte.Read);

            JsonRoundTrip_ValueOnly((short)-4, JsonToShort.Write, JsonToShort.Read);
            JsonRoundTrip(short.MaxValue, short.MinValue, JsonToShort.Write, JsonToShort.Read);

            JsonRoundTrip_ValueOnly((ushort)4, JsonToUshort.Write, JsonToUshort.Read);
            JsonRoundTrip(ushort.MaxValue, ushort.MinValue, JsonToUshort.Write, JsonToUshort.Read);

            JsonRoundTrip_ValueOnly(int.MaxValue / 2, JsonToInt.Write, JsonToInt.Read);
            JsonRoundTrip(int.MaxValue, int.MinValue, JsonToInt.Write, JsonToInt.Read);

            JsonRoundTrip_ValueOnly((uint)4, JsonToUint.Write, JsonToUint.Read);
            JsonRoundTrip(uint.MaxValue, uint.MinValue, JsonToUint.Write, JsonToUint.Read);

            JsonRoundTrip_ValueOnly((long.MinValue / 2), JsonToLong.Write, JsonToLong.Read);
            JsonRoundTrip(long.MaxValue, long.MinValue, JsonToLong.Write, JsonToLong.Read);

            JsonRoundTrip_ValueOnly((ulong)4, JsonToUlong.Write, JsonToUlong.Read);
            JsonRoundTrip(ulong.MaxValue, ulong.MinValue, JsonToUlong.Write, JsonToUlong.Read);

            JsonRoundTrip_ValueOnly(3.14f, JsonToFloat.Write, JsonToFloat.Read);
            JsonRoundTrip(float.MaxValue, float.MinValue, JsonToFloat.Write, JsonToFloat.Read);

            JsonRoundTrip_ValueOnly(2.16789480d, JsonToDouble.Write, JsonToDouble.Read);
            JsonRoundTrip(double.MaxValue, double.MinValue, JsonToDouble.Write, JsonToDouble.Read);
        }

        internal static void JsonRoundTrip<T>(T value, T defaultValue, Action<JsonWriter, string, T, T> write, Func<JsonReader, Database, T> read)
        {
            T roundTripped = default;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(stream, leaveOpen: true))
                using (JsonTextWriter jtw = new JsonTextWriter(sw))
                {
                    jtw.WriteStartObject();

                    // Write PropertyName and Value
                    write(jtw, "propertyName", value, defaultValue);

                    // Write PropertyName and Default (nothing should be written)
                    write(jtw, "propertyWithDefault", defaultValue, defaultValue);

                    jtw.WriteEndObject();
                }

                long bytesWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader sr = new StreamReader(stream, leaveOpen: true))
                using (JsonTextReader jtr = new JsonTextReader(sr))
                {
                    jtr.Read();
                    Assert.Equal(JsonToken.StartObject, jtr.TokenType);

                    // Confirm Property Name was written
                    jtr.Read();
                    Assert.Equal(JsonToken.PropertyName, jtr.TokenType);
                    Assert.Equal("propertyName", (string)jtr.Value);

                    // Read value
                    jtr.Read();
                    roundTripped = read(jtr, null);

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

        internal static void JsonRoundTrip_ValueOnly<T>(T value, Action<JsonWriter, T> write, Func<JsonReader, Database, T> read)
        {
            T roundTripped = default;

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
                    roundTripped = read(jtr, null);
                }

                // Ensure everything written is read back
                Assert.Equal(bytesWritten, stream.Position);

                // Ensure value returned matches value written
                Assert.Equal(value, roundTripped);
            }
        }

        internal static T JsonParse<T>(string jsonText, Func<JsonReader, Database, T> read)
        {
            T roundTripped = default;

            using (StringReader sr = new StringReader(jsonText))
            using (JsonTextReader jtr = new JsonTextReader(sr))
            {
                jtr.Read();

                // Read value
                roundTripped = read(jtr, null);

                // Ensure no remaining tokens
                Assert.False(jtr.Read());
            }

            return roundTripped;
        }
    }
}
