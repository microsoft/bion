// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using BSOA.Json.Converters;
using BSOA.Model;

using Xunit;

namespace BSOA.Test.Json
{
    public class ConvertersTests
    {
        [Fact]
        public void JsonToBool_Basics()
        {
            JsonRoundTrip.ValueOnly(true, JsonToBool.Write, JsonToBool.Read);

            JsonRoundTrip.NameAndValue(true, false, JsonToBool.Write, JsonToBool.Read<Database>);
            JsonRoundTrip.NameAndValue(false, true, JsonToBool.Write, JsonToBool.Read<Database>);
        }

        [Fact]
        public void JsonToDateTime_Basics()
        {
            // BSOA DateTime format is precise to the millisecond only
            DateTime now = ToMillisecond(DateTime.UtcNow);

            JsonRoundTrip.ValueOnly(now, JsonToDateTime.Write, JsonToDateTime.Read);

            JsonRoundTrip.NameAndValue(now, DateTime.MinValue.ToUniversalTime(), JsonToDateTime.Write, JsonToDateTime.Read);
            JsonRoundTrip.NameAndValue(ToMillisecond(DateTime.MinValue.ToUniversalTime()), now, JsonToDateTime.Write, JsonToDateTime.Read);
            JsonRoundTrip.NameAndValue(ToMillisecond(DateTime.MaxValue.ToUniversalTime()), now, JsonToDateTime.Write, JsonToDateTime.Read);

            DateTime nonStandardFormat = JsonRoundTrip.Parse<Database, DateTime>("\"01/01/2020\"", JsonToDateTime.Read);
            Assert.Equal(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), nonStandardFormat);
        }

        private static DateTime ToMillisecond(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind);
        }

        [Fact]
        public void JsonToEnum_Basics()
        {
            JsonRoundTrip.ValueOnly(DayOfWeek.Thursday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);

            JsonRoundTrip.NameAndValue(DayOfWeek.Thursday, DayOfWeek.Sunday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);
            JsonRoundTrip.NameAndValue(DayOfWeek.Wednesday, DayOfWeek.Thursday, JsonToEnum<DayOfWeek>.Write, JsonToEnum<DayOfWeek>.Read);

            // Flags enum
            JsonRoundTrip.NameAndValue(FileAttributes.ReadOnly | FileAttributes.Directory, default(FileAttributes), JsonToEnum<FileAttributes>.Write, JsonToEnum<FileAttributes>.Read);
        }

        [Fact]
        public void JsonToString_Basics()
        {
            JsonRoundTrip.ValueOnly(null, JsonToString.Write, JsonToString.Read);
            JsonRoundTrip.ValueOnly("", JsonToString.Write, JsonToString.Read);
            JsonRoundTrip.ValueOnly("simple", JsonToString.Write, JsonToString.Read);
            JsonRoundTrip.ValueOnly("'\"\\\"'", JsonToString.Write, JsonToString.Read);

            JsonRoundTrip.NameAndValue("Simple", null, JsonToString.Write, JsonToString.Read);
            JsonRoundTrip.NameAndValue(null, "NonNullDefault", JsonToString.Write, JsonToString.Read);
        }

        [Fact]
        public void JsonToUri_Basics()
        {
            JsonRoundTrip.ValueOnly(null, JsonToUri.Write, JsonToUri.Read);
            JsonRoundTrip.ValueOnly(new Uri("https://www.github.com/Microsoft/bion"), JsonToUri.Write, JsonToUri.Read);

            // Null default
            JsonRoundTrip.NameAndValue(new Uri("https://www.github.com/Microsoft/bion"), null, JsonToUri.Write, JsonToUri.Read);

            // Non-null default
            JsonRoundTrip.NameAndValue(new Uri("https://www.github.com/Microsoft/bion"), new Uri("src/Program.cs", UriKind.Relative), JsonToUri.Write, JsonToUri.Read);

            // Null when default is not null
            JsonRoundTrip.NameAndValue(null, new Uri("src/Program.cs", UriKind.Relative), JsonToUri.Write, JsonToUri.Read);
        }

        [Fact]
        public void JsonToNumbers_Basics()
        {
            JsonRoundTrip.ValueOnly((byte)4, JsonToByte.Write, JsonToByte.Read);
            JsonRoundTrip.NameAndValue(byte.MaxValue, byte.MinValue, JsonToByte.Write, JsonToByte.Read);

            JsonRoundTrip.ValueOnly((sbyte)4, JsonToSbyte.Write, JsonToSbyte.Read);
            JsonRoundTrip.NameAndValue(sbyte.MaxValue, sbyte.MinValue, JsonToSbyte.Write, JsonToSbyte.Read);

            JsonRoundTrip.ValueOnly((short)-4, JsonToShort.Write, JsonToShort.Read);
            JsonRoundTrip.NameAndValue(short.MaxValue, short.MinValue, JsonToShort.Write, JsonToShort.Read);

            JsonRoundTrip.ValueOnly((ushort)4, JsonToUshort.Write, JsonToUshort.Read);
            JsonRoundTrip.NameAndValue(ushort.MaxValue, ushort.MinValue, JsonToUshort.Write, JsonToUshort.Read);

            JsonRoundTrip.ValueOnly(int.MaxValue / 2, JsonToInt.Write, JsonToInt.Read);
            JsonRoundTrip.NameAndValue(int.MaxValue, int.MinValue, JsonToInt.Write, JsonToInt.Read);

            JsonRoundTrip.ValueOnly((uint)4, JsonToUint.Write, JsonToUint.Read);
            JsonRoundTrip.NameAndValue(uint.MaxValue, uint.MinValue, JsonToUint.Write, JsonToUint.Read);

            JsonRoundTrip.ValueOnly((long.MinValue / 2), JsonToLong.Write, JsonToLong.Read);
            JsonRoundTrip.NameAndValue(long.MaxValue, long.MinValue, JsonToLong.Write, JsonToLong.Read);

            JsonRoundTrip.ValueOnly((ulong)4, JsonToUlong.Write, JsonToUlong.Read);
            JsonRoundTrip.NameAndValue(ulong.MaxValue, ulong.MinValue, JsonToUlong.Write, JsonToUlong.Read);

            JsonRoundTrip.ValueOnly(3.14f, JsonToFloat.Write, JsonToFloat.Read);
            JsonRoundTrip.NameAndValue(float.MaxValue, float.MinValue, JsonToFloat.Write, JsonToFloat.Read);

            JsonRoundTrip.ValueOnly(2.16789480d, JsonToDouble.Write, JsonToDouble.Read);
            JsonRoundTrip.NameAndValue(double.MaxValue, double.MinValue, JsonToDouble.Write, JsonToDouble.Read);
        }
    }
}
