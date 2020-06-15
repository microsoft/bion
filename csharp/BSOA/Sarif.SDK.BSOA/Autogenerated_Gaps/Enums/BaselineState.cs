// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// The state of a result relative to a baseline of a previous run.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.0.0")]
    public enum BaselineState
    {
        None,
        Unchanged,
        Updated,
        New,
        Absent
    }
}