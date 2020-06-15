// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.CodeAnalysis.Sarif
{
    public partial class Message
    {
        public bool ShouldSerializeArguments() { return this.Arguments.HasAtLeastOneNonNullValue(); }
    }
}
