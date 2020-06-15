// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  NumberListColumn adds mutability to the lists in an ArraySliceColumn by returning
    ///  NumberLists, which remember the column and row they represent and update the value on changes.
    /// </summary>
    public class NumberListColumn<T> : WrappingColumn<NumberList<T>, ArraySlice<T>>, INumberColumn<T> where T : unmanaged
    {
        public NumberListColumn() : base(new ArraySliceColumn<T>())
        { }

        public override NumberList<T> this[int index] 
        {
            get => new NumberList<T>(Inner, index);
            set => Inner[index] = value.Slice;
        }

        public void ForEach(Action<ArraySlice<T>> action)
        {
            ((ArraySliceColumn<T>)Inner).ForEach(action);
        }
    }
}