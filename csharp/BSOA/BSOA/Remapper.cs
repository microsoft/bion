using System.Collections.Generic;

namespace BSOA
{
    public class Remapper
    {
        // Remove each distinct value in values from remaining. Stop early if it becomes empty.
        public static void ExceptWith(HashSet<byte> remaining, ArraySlice<byte> values)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end && remaining.Count > 0; ++i)
            {
                remaining.Remove(array[i]);
            }
        }

        // Replace values >= remapFrom with replaceWith[value - remapFrom]
        public static void RemapAbove(ArraySlice<byte> values, byte remapFrom, byte[] replaceWith)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                if (array[i] >= remapFrom)
                {
                    array[i] = replaceWith[(array[i] - remapFrom)];
                }
            }
        }
    }
}