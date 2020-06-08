using BSOA.Demo.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Converters
{
    public static class ConverterExtensions
    {
        public static void ConvertList<T, U>(this IList<U> target, IList<T> source, SarifLog database, Func<T, SarifLog, U> convertSingle)
        {
            if (source != null)
            {
                foreach (T item in source)
                {
                    target.Add(convertSingle(item, database));
                }
            }
        }

        public static bool CompareList<T, U>(this IList<T> expected, IList<U> actual, Func<T, U, bool> compareSingle)
        {
            if (expected == null) { return (actual == null || actual.Count == 0); }
            if (expected.Count != actual.Count) { return false; }

            for (int i = 0; i < expected.Count; ++i)
            {
                if (!compareSingle(expected[i], actual[i])) { return false; }
            }

            return true;
        }
    }
}
