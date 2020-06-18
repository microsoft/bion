// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// Possible values for the SARIF schema version.
    /// </summary>
    [GeneratedCode("Microsoft.Json.Schema.ToDotNet", "1.1.0.0")]
    public enum SarifVersion
    {
        Unknown,

        [EnumMember(Value = SarifUtilities.V1_0_0)]
        OneZeroZero,

        [EnumMember(Value = VersionConstants.StableSarifVersion)]
        Current
    }
}