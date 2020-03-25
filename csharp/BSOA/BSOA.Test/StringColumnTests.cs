using Xunit;

namespace BSOA.Test
{
    public class StringColumnTests
    {
        [Fact]
        public void StringColumn_Basics()
        {
            Column.Basics<string>(
                () => new StringColumn(),
                string.Empty,
                "AnotherValue",
                (i) => i.ToString()
            );
        }

        [Fact]
        public void StringColumn_LongValuesAndMerging()
        { 
            StringColumn column = new StringColumn();
            StringColumn neverConverted = new StringColumn();
            StringColumn roundTripped;

            // Test values just at and above LargeValue limit
            column[0] = new string(' ', 2047);
            column[1] = string.Empty;
            column[2] = new string(' ', 2048);

            for (int i = 0; i < column.Count; ++i)
            {
                neverConverted[i] = column[i];
            }

            // Verify RoundTrip
            roundTripped = BinarySerializable.RoundTrip<StringColumn>(column, () => new StringColumn());
            Assert.Equal(neverConverted, roundTripped);

            // Verify column converted for round tripped also still reports the same values (after the 'Trim()' conversion)
            Assert.Equal(neverConverted, column);

            // Set a short value to long and a long value to short, and add another value
            column[0] = new string(':', 2400);
            column[2] = "MuchShorter";
            column[3] = "Simple";

            for (int i = 0; i < column.Count; ++i)
            {
                neverConverted[i] = column[i];
            }

            // Verify values read back correctly immediately
            Assert.Equal(new string(':', 2400), column[0]);
            Assert.Equal("MuchShorter", column[2]);
            Assert.Equal("Simple", column[3]);

            // Verify values re-roundtrip again properly (merging old and new immutable values)
            roundTripped = BinarySerializable.RoundTrip<StringColumn>(column, () => new StringColumn());
            Assert.Equal(neverConverted, roundTripped);
            Assert.Equal(neverConverted, column);
        }
    }
}
