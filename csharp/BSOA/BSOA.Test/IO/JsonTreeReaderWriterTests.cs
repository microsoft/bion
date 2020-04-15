using BSOA.Json;
using BSOA.Test.Components;
using Xunit;

namespace BSOA.Test.IO
{
    public class JsonTreeReaderWriterTests
    {
        [Fact]
        public void JsonTreeReaderWriter_Basics()
        {
            // Run ITreeSerializable suite on JsonTreeReader and JsonTreeWriter
            TreeSerializable.Basics(
                (stream, settings) => new JsonTreeWriter(stream, settings),
                (stream, settings) => new JsonTreeReader(stream, settings));
        }
    }
}
