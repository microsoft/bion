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
    }
}
