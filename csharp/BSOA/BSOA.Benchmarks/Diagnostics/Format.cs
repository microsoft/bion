using System;

namespace BSOA.Benchmarks
{
    /// <summary>
    ///  Format provides helper methods to human-format common values like
    ///  file sizes, rates, and time intervals.
    /// </summary>
    public static class Format
    {
        public const double Kilobyte = 1024;
        public const double Megabyte = 1024 * 1024;
        public const double Gigabyte = 1024 * 1024 * 1024;

        public static string Rate(long sizeInBytes, double elapsedSeconds, long iterations = 1)
        {
            return $"{Size((long)((iterations * sizeInBytes) / elapsedSeconds))}/s";
        }

        public static string Size(long sizeInBytes)
        {
            if (sizeInBytes < Kilobyte)
            {
                return $"{sizeInBytes:n0} b";
            }
            else if (sizeInBytes < 10 * Kilobyte)
            {
                return $"{sizeInBytes / Kilobyte:n2} KB";
            }
            else if (sizeInBytes < 100 * Kilobyte)
            {
                return $"{sizeInBytes / Kilobyte:n1} KB";
            }
            else if (sizeInBytes < Megabyte)
            {
                return $"{sizeInBytes / Kilobyte:n0} KB";
            }
            else if (sizeInBytes < 10 * Megabyte)
            {
                return $"{sizeInBytes / Megabyte:n2} MB";
            }
            else if (sizeInBytes < 100 * Megabyte)
            {
                return $"{sizeInBytes / Megabyte:n1} MB";
            }
            else if (sizeInBytes < Gigabyte)
            {
                return $"{sizeInBytes / Megabyte:n0} MB";
            }
            else if (sizeInBytes < 10 * Gigabyte)
            {
                return $"{sizeInBytes / Gigabyte:n2} GB";
            }
            else if (sizeInBytes < 100 * Gigabyte)
            {
                return $"{sizeInBytes / Gigabyte:n1} GB";
            }
            else
            {
                return $"{sizeInBytes / Gigabyte:n0} GB";
            }
        }

        public static string Time(TimeSpan elapsed)
        {
            // Note: TimeSpan has 100ns granularity; use other overload to safely report smaller < 2 us times.
            return Time(elapsed.TotalSeconds);
        }

        /// <summary>
        ///  Return a formatted time.
        ///  Argument is fractional seconds to accurately support values too small for TimeSpan (< 2 us).
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string Time(double seconds)
        {
            if (seconds <= 0.0)
            {
                return "-";
            }
            else if (seconds < 0.00000001)
            {
                return $"{seconds * 1000 * 1000 * 1000:n2} ns";
            } 
            else if (seconds < 0.0000001)
            {
                return $"{seconds * 1000 * 1000 * 1000:n1} ns";
            }
            else if (seconds < 0.000001)
            {
                return $"{seconds * 1000 * 1000 * 1000:n0} ns";
            }
            else if (seconds < 0.00001)
            {
                return $"{seconds * 1000 * 1000:n2} us";
            }
            else if (seconds < 0.0001)
            {
                return $"{seconds * 1000 * 1000:n1} us";
            }
            else if (seconds < 0.001)
            {
                return $"{seconds * 1000 * 1000:n0} us";
            }
            else if (seconds < 0.01)
            {
                return $"{seconds * 1000:n2} ms";
            }
            else if (seconds < 0.1)
            {
                return $"{seconds * 1000:n1} ms";
            }
            else if (seconds < 1)
            {
                return $"{seconds * 1000:n0} ms";
            }
            else if (seconds < 10)
            {
                return $"{seconds:n2} s";
            }
            else if (seconds < 100)
            {
                return $"{seconds:n1} s";
            }
            else
            {
                return $"{(seconds / 60):n1} min";
            }
        }

        public static double ParseTime(string time)
        {
            string[] parts = time.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            string numberString = parts[0];
            if (numberString == "-") { return 0.0d; }

            double number = double.Parse(numberString);

            string units = parts[1];
            switch (units)
            {
                case "ns":
                    return number / (1000 * 1000 * 1000);
                case "us":
                    return number / (1000 * 1000);
                case "ms":
                    return number / 1000;
                case "s":
                    return number;
                case "min":
                    return number * 60;
                default:
                    throw new FormatException($"\"{time}\" units, '{units}', were not recognized. Expecting: (min, s, ms, us, ns)");
            }
        }

        public static string Percentage(double numerator, double denominator)
        {
            if (denominator == 0.0) { return "NaN"; }
            if (numerator < 0.0 || denominator < 0.0 || numerator > denominator) { return "Invalid"; }

            double ratio = numerator / denominator;

            if (ratio < 0.01)
            {
                return $"{ratio:p2}";
            }
            else if (ratio < 0.10)
            {
                return $"{ratio:p1}";
            }
            else
            {
                return $"{ratio:p0}";
            }
        }

        public static string Ratio(double numerator, double denominator)
        {
            if (denominator == 0.0 || numerator == 0.0)
            {
                return "-";
            }

            double ratio = (numerator / denominator);

            // Report two significant figures, but an extra digit if the first digit is one.
            // (155, 31, 20, 15.5, 9.1, 5.0, 2.0, 1.55, 0.97, 0.31, 0.199, 0.150, ...)

            if (ratio < 0.2)
            {
                return $"{ratio:n3}x";
            }
            else if (ratio < 2)
            {
                return $"{ratio:n2}x";
            }
            else if (ratio < 20)
            {
                return $"{ratio:n1}x";
            }
            else
            {
                return $"{ratio:n0}x";
            }
        }

        public static void HighlightLine(params string[] values)
        {
            System.ConsoleColor normal = Console.ForegroundColor;

            for (int i = 0; i < values.Length; ++i)
            {
                Console.Write(values[i]);
                Console.ForegroundColor = (i % 2 == 0 ? System.ConsoleColor.Green : normal);
            }

            Console.WriteLine();
            Console.ForegroundColor = normal;
        }
    }
}
