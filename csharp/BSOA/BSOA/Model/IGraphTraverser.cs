// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Model
{
    /// <summary>
    ///  To enable Garbage Collection, we must be able to traverse references from one table
    ///  to another to find all referenced rows across tables. IRefColumns must implement
    ///  traversing the references a given row refers to given an IGraphTraverser from the
    ///  table referenced.
    /// </summary>
    public interface IGraphTraverser
    {
        long Traverse(int index);
    }
}
