// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BSOA.Generator.Extensions
{
    // Similar to https://github.com/microsoft/jschema/blob/master/src/Json.Schema.ToDotNet/StringExtensions.cs
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            return Char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string ToPascalCase(this string value)
        {
            return Char.ToUpperInvariant(value[0]) + value.Substring(1);
        }
    }
}
