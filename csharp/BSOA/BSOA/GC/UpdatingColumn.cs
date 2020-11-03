using BSOA.Column;
using BSOA.Model;

using System;

namespace BSOA.GC
{
    /// <summary>
    ///  UpdatingColumn handles updating BSOA object model instances when a BSOA Garbage Collection
    ///  had to move them. When the object model calls a column getter or setter, this column updates
    ///  the object model instance to point to an updated row index on the real current Table.
    /// </summary>
    /// <typeparam name="T">Column value type</typeparam>
    internal class UpdatingColumn<T> : WrappingColumn<T, T>
    {
        public const string ErrorMessage = "Callers must call this[index, caller] overload on RevisedColumn for trap behavior to work.";
        private IColumn<T> _temp;
        private RowUpdater _updater;

        public UpdatingColumn(IColumn current, IColumn temp, RowUpdater updater) : base((IColumn<T>)current)
        {
            _temp = (IColumn<T>)temp;
            _updater = updater;
        }

        public override T this[int index]
        {
            get => throw new InvalidOperationException(ErrorMessage);
            set => throw new InvalidOperationException(ErrorMessage);
        }

        public override T this[int index, IRow caller]
        {
            get
            {
                _updater.Update(caller, out bool movedToTemp);

                if (movedToTemp)
                {
                    return _temp[caller.Index];
                }
                else
                {
                    return Inner[caller.Index];
                }
            }

            set
            {
                _updater.Update(caller, out bool movedToTemp);

                if (movedToTemp)
                {
                    _temp[caller.Index] = value;
                }
                else
                {
                    Inner[caller.Index] = value;
                }
            }
        }
    }
}
