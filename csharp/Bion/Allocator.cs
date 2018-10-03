namespace Bion
{
    internal static class Allocator
    {
        /// <summary>
        ///  Ensure an array is at least the required length, growing it by a minimum amount each time. 
        /// </summary>
        /// <param name="buffer">Array to check</param>
        /// <param name="length">Required Length</param>
        public static void EnsureBufferLength<T>(ref T[] buffer, int length)
        {
            if (buffer != null)
            {
                if (buffer.Length >= length) return;

                int minGrowthLength = buffer.Length + buffer.Length / 4;
                if (minGrowthLength > length) length = minGrowthLength;
            }

            buffer = new T[length];
        }
    }
}
