// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  NumberListColumn adds mutability to the lists in an ArraySliceColumn by returning
    ///  NumberLists, which remember the column and row they represent and update the value on changes.
    /// </summary>
    public class NumberListColumn<T> : WrappingColumn<NumberList<T>, ArraySlice<T>>, INumberColumn<T> where T : unmanaged, IEquatable<T>
    {
        public NumberListColumn() : base(new ArraySliceColumn<T>())
        { }

        public override NumberList<T> this[int index]
        {
            get => new NumberList<T>(Inner, index);
            set => Inner[index] = value?.Slice ?? ArraySlice<T>.Empty;
        }

        public void ForEach(Action<ArraySlice<T>> action)
        {
            ((ArraySliceColumn<T>)Inner).ForEach(action);
        }
    }

    /// <summary>
    ///  GenericNumberListColumn exposes NumberListColumn as an IColumn&lt;IList&lt;T&gt;&gt; for use
    ///  within an object model which needs to expose generic lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericNumberListColumn<T> : WrappingColumn<IList<T>, NumberList<T>>, INumberColumn<T> where T : unmanaged, IEquatable<T>
    {
        public GenericNumberListColumn() : this(Nullability.DefaultToNull)
        { }

        public GenericNumberListColumn(Nullability nullability) : base(NullableColumn<NumberList<T>>.Wrap(new NumberListColumn<T>(), nullability))
        { }

        public override IList<T> this[int index]
        {
            get => Inner[index];
            set
            {
                if (value == null)
                {
                    Inner[index] = null;
                }
                else
                {
                    NumberList<T> item = Inner[index];
                    if (item == null)
                    {
                        Inner[index] = NumberList<T>.Empty;
                        item = Inner[index];
                    }

                    item.SetTo(new ArraySlice<T>(value.ToArray()));
                }
            }
        }

        public void ForEach(Action<ArraySlice<T>> action)
        {
            IColumn inner = Inner;
            if (inner is NullableColumn<NumberList<T>>)
            {
                inner = ((NullableColumn<NumberList<T>>)inner).Values;
            }

            ((NumberListColumn<T>)inner).ForEach(action);
        }
    }
}