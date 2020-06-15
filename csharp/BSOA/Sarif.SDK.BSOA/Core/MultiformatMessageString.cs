// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.CodeAnalysis.Sarif
{
    public partial class MultiformatMessageString
    {
        public bool ShouldSerializeMarkdown() { return !string.IsNullOrWhiteSpace(this.Markdown); }

        public bool ShouldSerializeText() { return !string.IsNullOrWhiteSpace(this.Text); }
    }
}
