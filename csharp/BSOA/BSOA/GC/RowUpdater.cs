using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.GC
{
    /// <summary>
    ///  RowUpdater tracks rows in a Garbage Collection updated table which
    ///  were swapped or which moved to a temporary table. The original table instance
    ///  has all columns wrapped with UpdatingColumns, which update the object model
    ///  instances when they try to call to get or set data.
    /// </summary>
    public class RowUpdater
    {
        private ITable Current { get; }
        private ITable Temp { get; }
        private Dictionary<int, Mapping> Mappings { get; }

        public RowUpdater(ITable current, ITable temp)
        {
            Current = current;
            Temp = temp;
            Mappings = new Dictionary<int, Mapping>();
        }

        public void AddMapping(int oldIndex, int newIndex, bool movedToTemp)
        {
            Mappings[oldIndex] = new Mapping(newIndex, movedToTemp);
        }

        public void Update(IRow caller, out bool movedToTemp)
        {
            if (!Mappings.TryGetValue(caller.Index, out Mapping mapping))
            {
                // Row was not swapped or removed - current table, same index
                movedToTemp = false;
                caller.Remap(Current, caller.Index);
            }
            else if (mapping.MovedToTemp)
            {
                // Row was removed - temp table at new index
                movedToTemp = true;
                caller.Remap(Temp, mapping.NewIndex);
            }
            else
            {
                // Row was swapped but kept - current table, new index
                movedToTemp = false;
                caller.Remap(Current, mapping.NewIndex);
            }
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
