// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    public partial class Location
    {
        public LogicalLocation LogicalLocation
        {
            get { return LogicalLocations?[0]; }
            set
            {
                if (value != null)
                {
                    LogicalLocations = new List<LogicalLocation> { value };
                }
                else
                {
                    LogicalLocations = null;
                }
            }
        }
    }
}
