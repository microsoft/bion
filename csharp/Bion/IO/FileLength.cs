using System.IO;

namespace Bion.IO
{
    public class FileLength
    {
        public const float BytesPerMB = 1024 * 1024;

        public static string MB(string filePath)
        {
            return $"{new FileInfo(filePath).Length / BytesPerMB:n3}MB";
        }

        public static string MB(long lengthBytes)
        {
            return $"{lengthBytes / BytesPerMB:n3}MB";
        }

        public static string Percentage(string originalPath, string compressedPath, string dictionaryPath = null)
        {
            long originalLength = new FileInfo(originalPath).Length;
            long compressedLength = new FileInfo(compressedPath).Length;

            if (dictionaryPath != null)
            {
                compressedLength += new FileInfo(dictionaryPath).Length;
            }

            return $"{(float)compressedLength / (float)originalLength:p0}";
        }
    }
}
