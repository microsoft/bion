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
        /// <summary>
        ///  FindUnusedAndCollect walks all values in the 'indices' column to find any indices with
        ///  no remaining references. It then swaps and removes them from the 'values' column and
        ///  updates indices in the 'indices' column to the updated values for all swapped rows.
        /// </summary>
        /// <typeparam name="T">Type of index used (int or byte)</typeparam>
        /// <param name="values">Column containing the values themselves</param>
        /// <param name="indices">Column containing indices referring to rows in the values column</param>
        /// <returns>True if any values were swapped and removed; false otherwise.</returns>
        public static bool FindUnusedAndCollect<T>(IColumn values, INumberColumn<T> indices) where T : unmanaged, IEquatable<T>
        {
            if (values.Count == 0) { return false; }
            return FindUnusedAndCollect(values, indices, new BitVector(false, values.Count));
        }

        /// <summary>
        ///  FindUnusedAndCollect walks all values in the 'indices' column to find any indices with
        ///  no remaining references. It then swaps and removes them from the 'values' column and
        ///  updates indices in the 'indices' column to the updated values for all swapped rows.
        ///  
        ///  This overload uses the provided BitVector to mark used rows. The caller can set BitVector 
        ///  entries true to keep specific rows even if they aren't referenced in the indices column.
        /// </summary>
        /// <typeparam name="T">Type of index used (int or byte)</typeparam>
        /// <param name="values">Column containing the values themselves</param>
        /// <param name="indices">Column containing indices referring to rows in the values column</param>
        /// <param name="rowsToKeep">BitVector to use to track referenced rows; values already set true will be kept even if unreferenced.</param>
        /// <returns>True if any values were swapped and removed; false otherwise.</returns>
        public static bool FindUnusedAndCollect<T>(IColumn values, INumberColumn<T> indices, BitVector rowsToKeep) where T : unmanaged, IEquatable<T>
        {
            IRemapper<T> remapper = RemapperFactory.Build<T>();

            // Trim indices first to consolidate references into a new consolidated array as much as possible
            indices.Trim();

            // Find all value indices which are no longer referenced by any row
            indices.ForEach((slice) => remapper.AddValues(slice, rowsToKeep));

            return Collect(values, new[] { indices }, rowsToKeep);
        }

        /// <summary>
        ///  Garbage Collect a column (or table) given a pre-filled set of rowsToKeep.
        ///  Used both to clean up unused values in columns, and to clean up unused rows in tables.
        ///  
        ///   - Swap and Remove unused rows in the column itself.
        ///   - Ensure no row is ever swapped more than once.
        ///   - Update references in all 'refsToColumn' to new indices for all swapped rows.
        ///   - Populate RowUpdater, if passed, with new indices for swapped and kept rows.
        ///   - Populate RowUpdater, if passed, with new external indices for non-kept rows.
        /// </summary>
        /// <typeparam name="T">Type of indices used to refer to rows</typeparam>
        /// <param name="column">Column (or Table) containing rows to Garbage Collect</param>
        /// <param name="refsToColumn">Set of all columns pointing to 'column'; indices will be updated for all swapped rows</param>
        /// <param name="rowsToKeep">Booleans identifying which rows to keep</param>
        /// <param name="updater">Optional RowUpdater; if passed, mappings are added for every swapped row kept and removed row with the new index for the row.</param>
        /// <param name="rowIndexToTempIndex">Optional array providing the index in another table for each removed row; used to add mapping to RowUpdater to correctly redirect removed rows.</param>
        /// <returns></returns>
        public static bool Collect<T>(IColumn column, IEnumerable<INumberColumn<T>> refsToColumn, IReadOnlyList<bool> rowsToKeep, RowUpdater updater = null, int[] rowIndexToTempIndex = null) where T : unmanaged, IEquatable<T>
        {
            int[] replacements = null;

            int smallestToRemove = 0;
            int biggestToKeep = column.Count - 1;
            int removeCount = 0;

            // Walk backward and forward in rowsToKeep, swapping the earliest row to remove with the latest row to keep.
            // This logic is needed to ensure rows are never swapped more than once, so that indices to them are updated correctly.
            // This logic also ensures the minimum number of swaps are made. Swaps can be expensive for tables.
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

                // Tell the updater about the removed row (originally at smallestToRemove, now in temp)
                updater?.AddMapping(smallestToRemove, rowIndexToTempIndex?[smallestToRemove] ?? -1, movedToTemp: true);

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