// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.CodeAnalysis.Sarif
{
    public partial class LogicalLocation
    {
        public LogicalLocation Resolve(Run run)
        {
            return Index >= 0 && Index < run?.LogicalLocations?.Count
                ? run.LogicalLocations[Index]
                : this;
        }
    }
}
