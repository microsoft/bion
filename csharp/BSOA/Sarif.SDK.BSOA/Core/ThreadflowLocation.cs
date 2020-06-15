// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    /// A thread flow location of a SARIF thread flow.
    /// </summary>
    public partial class ThreadFlowLocation
    {
        public bool ShouldSerializeKinds() { return this.Kinds.HasAtLeastOneNonNullValue(); }

        public bool ShouldSerializeTaxa() { return this.Taxa.HasAtLeastOneNonDefaultValue(ReportingDescriptorReference.ValueComparer); }
    }
}
