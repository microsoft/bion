// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Collections;

namespace BSOA.Model
{
    public interface INumberColumn<T> : IColumn where T : unmanaged
    {
        // Expose an action on each slice of values.
        // This is used to facilitate garbage collection.
        // The fundamental value of the column isn't the same as the type exposed.

        // TODO: Expose as a more basic interface for other fast bulk operations.
        void ForEach(Action<ArraySlice<T>> action);
    }
}
