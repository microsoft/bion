// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CodeDom.Compiler;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Values specifying the importance of an "threadFlowLocation" within the "codeFlow" in which it occurs
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.0.0")]
    public enum ThreadFlowLocationImportance
    {
        Important,
        Essential,
        Unimportant
    }
}