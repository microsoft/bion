using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  WrappingColumn wraps a single inner column, and redirects all non-type-specific members to
    ///  the inner column.
    /// </summary>
    public abstract class WrappingColumn<TOuter, TInner> : LimitedList<TOuter>, IColumn<TOuter>
    {
        protected IColumn<TInner> Inner { get; }

        public override int Count => Inner.Count;

        protected WrappingColumn(IColumn<TInner> inner)
        {
            Inner = inner;
        }

        public override void Swap(int index1, int index2)
        {
            // Swap directly in the inner column.
            Inner.Swap(index1, index2);
        }

        public override void RemoveFromEnd(int count)
        {
            Inner.RemoveFromEnd(count);
        }

        public override void Clear()
        {
            Inner.Clear();
        }

        public virtual void Trim()
        {
            Inner.Trim();
        }

        public virtual void Write(ITreeWriter writer)
        {
            Inner.Write(writer);
        }

        public virtual void Read(ITreeReader reader)
        {
            Inner.Read(reader);
        }
    }
}
