using System;
using System.Diagnostics;
using System.IO;

namespace BSOA.Diagnostics
{
    public static class Measure
    {
        private const double Megabyte = 1024 * 1024;

        public static TimeSpan Time(string description, Action method, int iterations = 1)
        {
            Console.WriteLine();
            Console.WriteLine(description);

            Stopwatch w = Stopwatch.StartNew();
            TimeSpan elapsedAfterFirst = TimeSpan.Zero;

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                GC.Collect();

                w.Restart();
                method();
                w.Stop();

                Console.Write($"{(iteration > 0 ? " | " : "")}{w.Elapsed.TotalSeconds:n2}s");
                if (iteration > 0) { elapsedAfterFirst += w.Elapsed; }
            }

            Console.WriteLine();
            return (iterations == 1 ? w.Elapsed : TimeSpan.FromTicks(elapsedAfterFirst.Ticks / (iterations - 1)));
        }

        public static T LoadPerformance<T>(Func<string, T> loader, string path, string description, int iterations = 5)
        {
            T result = default(T);
            double ramBeforeMB = GC.GetTotalMemory(true) / Megabyte;

            // Run and time the method
            TimeSpan averageRuntime = Time(description, () => result = loader(path), iterations);

            double ramAfterMB = GC.GetTotalMemory(true) / Megabyte;
            double fileSizeMB = new FileInfo(path).Length / Megabyte;
            double loadMegabytesPerSecond = fileSizeMB / averageRuntime.TotalSeconds;

            Console.WriteLine($" -> Read {result} in {fileSizeMB:n1} MB at {loadMegabytesPerSecond:n1} MB/s into {(ramAfterMB - ramBeforeMB):n1} MB RAM");

            return result;
        }
    }
}
