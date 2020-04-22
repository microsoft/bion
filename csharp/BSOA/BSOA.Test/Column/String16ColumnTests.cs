using BSOA.Column;
using Xunit;

namespace BSOA.Test
{
    public class String16ColumnTests
    {
        [Fact]
        public void String16Column_Basics()
        {
            Column.Basics<string>(
                () => new String16Column(),
                null,
                "AnotherValue",
                (i) => i.ToString()
            );

            // Test String.Empty handling
            String16Column column = new String16Column();
            column[0] = null;
            column[1] = "Something";
            column[2] = string.Empty;

            Assert.Null(column[0]);
            Assert.Equal("Something", column[1]);
            Assert.Equal("", column[2]);
        }
    }
}
