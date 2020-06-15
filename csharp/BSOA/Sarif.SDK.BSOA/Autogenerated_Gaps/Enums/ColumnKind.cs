// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// A value that specifies the unit in which the tool measures columns.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.0.0")]
    public enum ColumnKind
    {
        None,
        Utf16CodeUnits,
        UnicodeCodePoints
    }
}