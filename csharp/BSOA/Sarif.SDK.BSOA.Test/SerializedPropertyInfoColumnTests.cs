
using BSOA.Column;
using BSOA.Test;

using Microsoft.CodeAnalysis.Sarif;

using Xunit;

namespace Sarif.SDK.BSOA.Test
{
    public class SerializedPropertyInfoColumnTests
    {
        [Fact]
        public void SerializedPropertyInfoColumn_Basics()
        {
            SerializedPropertyInfo other = new SerializedPropertyInfo("Sample", true);

            Column.Basics(
                () => new SerializedPropertyInfoColumn(),
                null,
                other,
                (index) => new SerializedPropertyInfo(index.ToString(), false)
            );
        }
    }
}
