// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Defines methods to support the comparison of objects of type SerializedPropertyInfo for equality.
    /// </summary>
    internal sealed class SerializedPropertyInfoEqualityComparer : IEqualityComparer<SerializedPropertyInfo>
    {
        internal static readonly SerializedPropertyInfoEqualityComparer Instance = new SerializedPropertyInfoEqualityComparer();

        public bool Equals(SerializedPropertyInfo left, SerializedPropertyInfo right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            return left.SerializedValue.Equals(right.SerializedValue);
        }

        public int GetHashCode(SerializedPropertyInfo serializedPropertyInfo)
        {
            if (serializedPropertyInfo.SerializedValue == null)
            {
                return 0;
            }

            return serializedPropertyInfo.SerializedValue.GetHashCode();
        }
    }
}
