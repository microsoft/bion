namespace BSOA.Console.Extensions
{
    public static class LongExtensions
    {
        public const double Megabyte = 1024 * 1024;

        public static double Megabytes(this long bytes)
        {
            return bytes / Megabyte;
        }
    }
}
