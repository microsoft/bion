// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.CodeAnalysis.Sarif
{
    public static class SarifConstants
    {
        /// <summary>
        /// The standard file extension for SARIF files.
        /// </summary>
        public const string SarifFileExtension = ".sarif";

        // <summary>
        // When used with Guid.ToString(string format), this format produces the
        // GUID format required by SARIF: 32 digits separated by hyphens.
        // </summary>
        public const string GuidFormat = "D";

        public const string RedactedMarker = "[REDACTED]";

        // <summary>
        // The character that separates the components of a SARIF hierarchical string.
        // </summary>
        public const char HierarchicalComponentSeparator = '/';
    }
}
