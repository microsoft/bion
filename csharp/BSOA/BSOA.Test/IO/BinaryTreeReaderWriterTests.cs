using BSOA.IO;
using BSOA.Test.Components;
using Xunit;

namespace BSOA.Test.IO
{
    public class BinaryTreeReaderWriterTests
    {
        [Fact]
        public void BinaryTreeReaderWriter_Basics()
        {
            // Run ITreeSerializable suite on BinaryTreeReader and BinaryTreeWriter
            TreeSerializable.Basics(TreeFormat.Binary);
        }
    }
}
