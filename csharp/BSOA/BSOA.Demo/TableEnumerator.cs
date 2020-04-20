using System.Collections;
using System.Collections.Generic;

namespace ScaleDemo.SoA
{
    public class TableEnumerator<T> : IEnumerator<T>
    {
        private ITable<T> _table;
        private int _index;

        public T Current => _table[_index];
        object IEnumerator.Current => _table[_index];

        public TableEnumerator(ITable<T> table)
        {
            _table = table;
            _index = -1;
        }
        
        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        { }

        public bool MoveNext()
        {
            _index++;
            return (_index < _table.Count);
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
