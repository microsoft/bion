using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.GC
{
    /// <summary>
    ///  RowUpdater tracks rows in a Garbage-Collection-updated table.
    ///  Rows may have been swapped to new indices or copied to a temporary
    ///  table. RowUpdator updates object model instances to point to
    ///  the updated index in the updated table.
    /// </summary>
    public class RowUpdater
    {
        private ITable Successor { get; }
        private ITable Temp { get; }
        private Dictionary<int, Mapping> Mappings { get; }

        public RowUpdater(ITable successor, ITable temp)
        {
            Successor = successor;
            Temp = temp;
            Mappings = new Dictionary<int, Mapping>();
        }

        public void AddMapping(int oldIndex, int newIndex, bool movedToTemp)
        {
            Mappings[oldIndex] = new Mapping(newIndex, movedToTemp);
        }

        public void Update(IRow caller)
        {
            if (!Mappings.TryGetValue(caller.Index, out Mapping mapping))
            {
                // Row was not swapped or removed - current table, same index
                caller.Remap(Successor, caller.Index);
            }
            else if (mapping.MovedToTemp)
            {
                // Row was removed - temp table at new index
                caller.Remap(Temp, mapping.NewIndex);
            }
            else
            {
                // Row was swapped but kept - current table, new index
                caller.Remap(Successor, mapping.NewIndex);
            }

            // Call EnsureCurrent again with updated table; once up-to-date, table will have no RowUpdater.
            caller.Table.EnsureCurrent(caller);
        }

        private struct Mapping
        {
            public int NewIndex { get; }
            public bool MovedToTemp { get; }

            public Mapping(int newIndex, bool movedToTemp)
            {
                NewIndex = newIndex;
                MovedToTemp = movedToTemp;
            }
        }
    }
}
