// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Extensions;
using BSOA.Model;

namespace BSOA.GC
{
    public class GarbageCollector
    {
        public static bool FindUnusedAndCollect<T>(IColumn values, INumberColumn<T> indices) where T : unmanaged, IEquatable<T>
        {
            return FindUnusedAndCollect(values, indices, new BitVector(false, values.Count));
        }

        public static bool FindUnusedAndCollect<T>(IColumn values, INumberColumn<T> indices, BitVector rowsToKeep) where T : unmanaged, IEquatable<T>
        {
            IRemapper<T> remapper = RemapperFactory.Build<T>();

            // Trim indices first to consolidate references into a new consolidated array as much as possible
            indices.Trim();

            // Find all value indices which are no longer referenced by any row
            indices.ForEach((slice) => remapper.AddValues(slice, rowsToKeep));

            return Collect(values, new[] { indices }, rowsToKeep);
        }

        public static bool Collect<T>(IColumn column, IEnumerable<INumberColumn<T>> refsToColumn, IReadOnlyList<bool> rowsToKeep, RowUpdater updater = null, int[] rowIndexToTempIndex = null) where T : unmanaged, IEquatable<T>
        {
            int[] replacements = null;

            int smallestToRemove = 0;
            int biggestToKeep = column.Count - 1;
            int removeCount = 0;

            while (smallestToRemove <= biggestToKeep)
            {
                // Walk backward, finding the first row to keep
                while (smallestToRemove <= biggestToKeep && rowsToKeep[biggestToKeep] == false)
                {
                    // While rows already at the end are being removed, tell the updater where in the temp table they've gone
                    updater?.AddMapping(biggestToKeep, rowIndexToTempIndex[biggestToKeep], movedToTemp: true);

                    biggestToKeep--;
                    removeCount++;
                }

                // Walk forward, finding the first row to be removed
                while (smallestToRemove < biggestToKeep && rowsToKeep[smallestToRemove] == true)
                {
                    smallestToRemove++;
                }

                if (smallestToRemove >= biggestToKeep) { break; }
                removeCount++;

                // Swap these (the first row to remove with the last row to keep)
                column.Swap(smallestToRemove, biggestToKeep);

                // Tell the updater about the kept row (moved from biggestToKeep to smallestToRemove)
                updater?.AddMapping(biggestToKeep, smallestToRemove, movedToTemp: false);

                // Tell the updater about the removed row (originally at smallestToRemove, now in temp
                updater?.AddMapping(smallestToRemove, rowIndexToTempIndex[smallestToRemove], movedToTemp: true);

                // Set up a map array identifying the new row index for each previous row index
                if (refsToColumn != null)
                {
                    if (replacements == null)
                    {
                        replacements = new int[column.Count];

                        for (int i = 0; i < replacements.Length; ++i)
                        {
                            replacements[i] = i;
                        }
                    }

                    replacements[biggestToKeep] = smallestToRemove;
                    replacements[smallestToRemove] = biggestToKeep;
                }

                smallestToRemove++;
                biggestToKeep--;
            }

            // Remove the rows (now swapped to the end) to be removed
            if (removeCount > 0)
            {
                column.RemoveFromEnd(removeCount);
            }

            // Update columns to refer to the updated indices
            if (refsToColumn != null && replacements != null)
            {
                IRemapper<T> remapper = RemapperFactory.Build<T>();
                refsToColumn.ForEach((column) => column.ForEach((slice) => remapper.Remap(slice, replacements)));
            }

            return (removeCount > 0);
        }
    }
}