using BSOA.Column;
using BSOA.Model;

using System;

namespace BSOA.GC
{
    /// <summary>
    ///  UpdatingColumn implements a "trap" for BSOA object model objects, updating them to point to
    ///  an updated table and row index when any column property is get or set. This is done because
    ///  keeping a list of all object model instances in memory is too expensive. Updating them
    ///  "just in time" avoids slowing down other scenarios to support Garbage Collection.
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
