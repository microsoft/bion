namespace BSOA
{
    public class Remapper
    {
        // Remove each distinct value in values from remaining.
        public static void ExceptWith(BitVector remaining, ArraySlice<byte> values)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                remaining[array[i]] = false;
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