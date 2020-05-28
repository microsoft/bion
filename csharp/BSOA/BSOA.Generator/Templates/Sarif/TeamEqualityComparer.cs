// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Comparer for 'Team'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    internal sealed class TeamEqualityComparer : IEqualityComparer<Team>
    {
        internal static readonly TeamEqualityComparer Instance = new TeamEqualityComparer();

        public bool Equals(Team left, Team right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            // <ComparisonList>
            if (left.EmployeeId != right.EmployeeId)
            {
                return false;
            }
            // </ComparisonList>

            return true;
        }

        public int GetHashCode(Team obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            int result = 17;
            unchecked
            {
                result = (result * 31) + obj.EmployeeId.GetHashCode();
            }

            return result;
        }
    }
}
