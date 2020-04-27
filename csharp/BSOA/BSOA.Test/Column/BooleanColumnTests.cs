using BSOA.Column;
using BSOA.IO;
using BSOA.Test.Components;
using Xunit;

namespace BSOA.Test
{
    public class BooleanColumnTests
    {
        [Fact]
        public void BooleanColumn_Basics()
        {
            // Test with both default options
            Column.Basics<bool>(
                () => new BooleanColumn(true),
                true,
                false,
                (i) => (i % 3 == 0)
            );

            Column.Basics<bool>(
                () => new BooleanColumn(false),
                false,
                true,
                (i) => (i % 4 == 0)
            );
        }

        [Fact]
        public void BooleanColumn_AllDefault()
        {
            BooleanColumn c = new BooleanColumn(true);

            // Set a large number of values all to the default
            for (int i = 0; i < 8192; ++i)
            {
                c[i] = true;
            }

            // Verify the column serializes small (not to one bit per row)
            TreeDiagnostics diagnostics = TreeSerializer.Diagnostics(c, () => new BooleanColumn(true), TreeFormat.Binary);
            Assert.True(diagnostics.Length < 100);
        }
    }
}
