using System.Collections.Generic;

namespace BSOA.Extensions
{
    public static class ReadOnlyListExtensions
    {
        public static bool AreEqual<T>(this IReadOnlyList<T> left, IReadOnlyList<T> right)
        {
            if (left == null || right == null) { return left == null && right == null; }

            if (left.Count != right.Count) { return false; }
            for (int i = 0; i < left.Count; ++i)
            {
                if (!object.Equals(left[i], right[i])) { return false; }
            }

            return true;
        }

        public static int GetHashCode<T>(this IReadOnlyList<T> me)
        {
            int hashCode = 17;

            for (int i = 0; i < me.Count; ++i)
            {
                hashCode = unchecked(hashCode * 31) + me[i]?.GetHashCode() ?? 0;
            }

            return hashCode;
        }
    }
}
