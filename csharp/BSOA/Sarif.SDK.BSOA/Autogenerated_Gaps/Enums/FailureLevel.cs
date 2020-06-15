// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Values specifying the failure level of a notification or result.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.0.0")]
    public enum FailureLevel
    {
        None,
        Note,
        Warning,
        Error
    }
}