using BSOA.Collections;
using BSOA.Column;
using BSOA.Model;
using System.Linq;

namespace BSOA
{
    public class GarbageCollector
    {
        public static bool Collect<T, U>(INumberColumn<T> indices, IColumn<U> values) where T : unmanaged
        {
            return Collect(indices, values, new BitVector(true, values.Count));
        }

        public static bool Collect<T, U>(INumberColumn<T> indices, IColumn<U> values, BitVector unusedValues) where T : unmanaged
        {
            IRemapper<T> remapper = RemapperFactory.Build<T>();

            // Trim indices first to consolidate references into a new consolidated array as much as possible
            indices.Trim();

            // Find all value indices which are no longer referenced by any row
            indices.ForEach((slice) => remapper.RemoveValues(slice, unusedValues));

            // If there are unused values, ...
            int[] remapped = unusedValues.ToArray();
            if (remapped.Length > 0)
            {
                int remapFrom = (values.Count - remapped.Length);

                // Swap the *values* to the end of the values array
                for (int i = 0; i < remapped.Length; ++i)
                {
                    values.Swap(remapFrom + i, remapped[i]);
                }

                // Swap indices using those values to use the new ones
                indices.ForEach((slice) => remapper.RemapAbove(slice, remapFrom, remapped));

                // Remove the unused values that are now at the end of the array
                values.RemoveFromEnd(remapped.Length);
            }

            // Trim values afterward to clean up any newly unused space
            values.Trim();

            // Return whether anything was remapped
            return remapped.Length > 0;
        }
    }
}