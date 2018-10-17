using Bion.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test.Text
{
    [TestClass]
    public class WordCompressorTests
    {
        [TestMethod]
        public void WordCompressor_RoundTrip()
        {
            string originalPath = @"Content\Medium.json";
            string compressedPath = "Medium.compressed.bin";
            string dictionaryPath = "Medium.compressed.dict";
            string comparePath = "Medium.roundtrip.json";

            // Roundtrip without optimization; verify files equal
            WordCompressor.Compress(originalPath, compressedPath, dictionaryPath, false);
            WordCompressor.Expand(compressedPath, comparePath, dictionaryPath);
            Verify.FilesEqual(originalPath, comparePath);
            Verify.SizeRatioUnder(originalPath, compressedPath, 0.5f);

            File.Delete(compressedPath);
            File.Delete(dictionaryPath);
            File.Delete(comparePath);

            // Roundtrip *with* optimization; verify files equal
            WordCompressor.Compress(originalPath, compressedPath, dictionaryPath, true);
            WordCompressor.Expand(compressedPath, comparePath, dictionaryPath);
            Verify.FilesEqual(originalPath, comparePath);
            Verify.SizeRatioUnder(originalPath, compressedPath, 0.5f);
        }
    }
}
