using System.IO;

namespace Bion.IO
{
    public class FileLength
    {
        public const float BytesPerMB = 1024 * 1024;

        private static long Bytes(string filePath)
        {
            return (File.Exists(filePath) ? new FileInfo(filePath).Length : 0);
        }

        public static string MB(string filePath)
        {
            return $"{Bytes(filePath) / BytesPerMB:n3}MB";
        }

        public static string MB(long lengthBytes)
        {
            return $"{lengthBytes / BytesPerMB:n3}MB";
        }

        public static string Percentage(string originalPath, string compressedPath, string dictionaryPath = null)
        {
            long originalLength = Bytes(originalPath);
            long compressedLength = Bytes(compressedPath);

            if (dictionaryPath != null)
            {
                compressedLength += Bytes(dictionaryPath);
            }

            return $"{(float)compressedLength / (float)originalLength:p0}";
        }
    }
}
